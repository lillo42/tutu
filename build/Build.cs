using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitReleaseManager;
using Nuke.Common.Tools.GitVersion;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.GitReleaseManager.GitReleaseManagerTasks;

[DotNetVerbosityMapping]
[UnsetVisualStudioEnvironmentVariables]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Nuget API key", Name = "api-key")] readonly string NugetApiKey;

    [Parameter("NuGet Source for Packages", Name = "nuget-source")]
    readonly string NugetSource = "https://api.nuget.org/v3/index.json";

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion(NoFetch = true, Framework = "net5.0")] readonly GitVersion GitVersion;
    [CI] GitHubActions GitHubActions;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    AbsolutePath PackageDirectory => ArtifactsDirectory / "packages";

    AbsolutePath TestResultDirectory => ArtifactsDirectory / "test-results";

    AbsolutePath CoverageReportDirectory => ArtifactsDirectory / "coverage-report";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration));
            SourceDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            TestsDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            ArtifactsDirectory.GlobDirectories().DeleteDirectories();
        });

    Target Restore => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target Coverage => _ => _
        .Produces(CoverageReportDirectory)
        .Executes(() => { });

    Target Tests => _ => _
        .DependsOn(Compile)
        .Produces(TestResultDirectory / "*.trx")
        .Produces(TestResultDirectory / "*.xml")
        .Executes(() =>
        {
            var testProjects = new List<Project>();
            testProjects.AddRange(Solution.GetAllProjects("Tutu.Tests"));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                testProjects.AddRange(Solution.GetAllProjects("Tutu.Unix.Integration.Tests"));
            }

            TestResultDirectory.CreateOrCleanDirectory();
            CoverageReportDirectory.CreateOrCleanDirectory();

            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetResultsDirectory(TestResultDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .When(InvokedTargets.Contains(Coverage) || IsServerBuild, _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .When(IsServerBuild, _ => _.EnableUseSourceLink()))
                .CombineWith(testProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLoggers($"trx;LogFileName={v.Name}.trx")
                    .SetCoverletOutput(CoverageReportDirectory / $"{v.Name}.xml")));
        });

    Target IntegrationTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testProjects = new List<Project>();

            var os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "Unix";
            testProjects.Add(Solution.GetProject($"Tutu.{os}.Integration.Tests"));
            testProjects.Add(Solution.GetProject("Tutu.Integration.Tests"));

            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetResultsDirectory(TestResultDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .When(InvokedTargets.Contains(Coverage) || IsServerBuild, _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .When(IsServerBuild, _ => _.EnableUseSourceLink()))
                .CombineWith(testProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLoggers($"trx;LogFileName={v.Name}.trx")
                    .SetCoverletOutput(TestResultDirectory / $"{v.Name}.xml")));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Produces(PackageDirectory / "*.nupkg")
        .Produces(PackageDirectory / "*.snupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackageDirectory)
                .SetVersion(GitVersion.NuGetVersionV2)
                .EnableIncludeSource()
                .EnableIncludeSymbols()
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Consumes(Pack)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .SetSource(NugetSource)
                .SetApiKey(NugetApiKey)
                .EnableSkipDuplicate()
                .CombineWith(
                    PackageDirectory.GlobFiles("*.nupkg", "*.snupkg"),
                    (_, v) => _.SetTargetPath(v)));
        });

    Target CreateRelease => _ => _
        .After(Publish)
        .Requires(() => GitHubActions)
        .Executes(() =>
        {
            try
            {
                GitReleaseManagerCreate(release => release
                    .SetToken(GitHubActions.Token)
                    .SetRepositoryName(GitHubActions.Repository)
                    .SetRepositoryOwner(GitHubActions.RepositoryOwner)
                    .SetName(GitVersion.AssemblySemVer)
                    .SetTargetCommitish(GitHubActions.Sha)
                    .AddAssetPaths(PackageDirectory)
                );
            }
            catch (Exception e)
            {
                Log.Logger.Warning(e, "Failed to create release");
            }
        });
}
