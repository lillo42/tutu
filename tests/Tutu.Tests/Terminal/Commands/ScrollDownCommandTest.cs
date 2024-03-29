﻿using AutoFixture;
using FluentAssertions;
using Tutu.Terminal.Commands;

namespace Tutu.Tests.Terminal.Commands;

public class ScrollDownCommandTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void WriteAnsi_ShouldWriteCorrectAnsiSequence()
    {
        var lines = _fixture.Create<ushort>();
        var command = new ScrollDownCommand(lines);
        var writer = new StringWriter();

        command.WriteAnsi(writer);

        var expected = $"{AnsiCodes.CSI}{lines}T";
        writer.ToString().Should().Be(expected);
    }
}
