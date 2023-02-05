# Erised

# Cross-platform Terminal Manipulation Library

Erised is a pure-csharp, terminal manipulation library that makes it possible to write cross-platform text-based interfaces. It supports all UNIX and Windows terminals down to Windows 7 (not all terminals are tested,
see [Tested Terminals](#tested-terminals) for more info). It is heavily inspired by the `Rust` library [cross-term](https://github.com/crossterm-rs/crossterm)


## Table of Contents

- [Cross-platform Terminal Manipulation Library](#cross-platform-terminal-manipulation-library)
    - [Table of Contents](#table-of-contents)
    - [Features](#features)
        - [Tested Terminals](#tested-terminals)
    - [Getting Started](#getting-started)
        - [Feature Flags](#feature-flags)
        - [Dependency Justification](#dependency-justification)
        - [Other Resources](#other-resources)
    - [Used By](#used-by)
    - [Contributing](#contributing)
    - [Authors](#authors)
    - [License](#license)

## Features

- Cross-platform
- Multi-threaded (send, sync)
- Detailed documentation
- Few dependencies
- Full control over writing and flushing output buffer
- Is tty
- Cursor
    - Move the cursor N times (up, down, left, right)
    - Move to previous / next line
    - Move to column
    - Set/get the cursor position
    - Store the cursor position and restore to it later
    - Hide/show the cursor
    - Enable/disable cursor blinking (not all terminals do support this feature)
- Styled output
    - Foreground color (16 base colors)
    - Background color (16 base colors)
    - 256 (ANSI) color support (Windows 10 and UNIX only)
    - RGB color support (Windows 10 and UNIX only)
    - Text attributes like bold, italic, underscore, crossed, etc
- Terminal
    - Clear (all lines, current line, from cursor down and up, until new line)
    - Scroll up, down
    - Set/get the terminal size
    - Exit current process
    - Alternate screen
    - Raw screen
    - Set terminal title
    - Enable/disable line wrapping
- Event
    - Input Events
    - Mouse Events (press, release, position, button, drag)
    - Terminal Resize Events
    - Advanced modifier (SHIFT | ALT | CTRL) support for both mouse and key events and
    - futures Stream  (feature 'event-stream')
    - Poll/read API

<!--
WARNING: Do not change following heading title as it's used in the URL by other crates!
-->

### Tested Terminals

- Console Host
    - Windows 10 (Pro)
    - Windows 8.1 (N)
- Ubuntu Desktop Terminal
    - Ubuntu 17.10
    - Pop!_OS ( Ubuntu ) 20.04
- (Arch, Manjaro) KDE Konsole
- (Arch) Kitty
- Linux Mint
- (OpenSuse) Alacritty
- (Chrome OS) Crostini

This crate supports all UNIX terminals and Windows terminals down to Windows 7; however, not all of the
terminals have been tested. If you have used this library for a terminal other than the above list without
issues, then feel free to add it to the above list - I really would appreciate it!

## Getting Started
_see the [examples directory](examples/) and [documentation](https://docs.rs/crossterm/) for more advanced examples._

```csharp
using Erised;
using static Erised.Commands.Style;

public static void Main()
{
    Console.Out
        .Execute(
            SetForegroundColor(Color.Blue),
            SetBackgroundColor(Color.Red),
            Print("Styled text here."),
            ResetColor
        );
}
```

Checkout this [list](https://docs.rs/crossterm/0.14.0/crossterm/index.html#supported-commands) with all possible commands.

### Other Resources

- [API documentation](https://docs.rs/crossterm/)
- [Deprecated examples repository](https://github.com/crossterm-rs/examples)

## Contributing

We highly appreciate when anyone contributes to this crate. Before you do, please,
read the [Contributing](docs/CONTRIBUTING.md) guidelines.

## Authors

* **Rafael Andrade** - *Project Owner & creator*