# WinUI 3 Calculator

A modern, open-source Windows calculator built with WinUI 3, featuring MVVM architecture, Fluent Design System, and full keyboard support.

![Windows](https://img.shields.io/badge/Windows-11%20%7C%2010-0078D4?logo=windows)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/License-MIT-green)

## Features

- 🧮 **Full Calculator Operations** - Addition, subtraction, multiplication, division, percentage
- 🔢 **Advanced Functions** - Square root, square, reciprocal, negate
- ⌨️ **Complete Keyboard Support** - Use numpad or keyboard for fast input
- 🎨 **Fluent Design** - Electric Blue accent with Mica backdrop
- 🌙 **Dark/Light Theme** - Follows Windows system theme automatically
- ♿ **Accessible** - Full keyboard navigation, screen reader support
- 📋 **Clipboard Integration** - Right-click to copy results

## Screenshots

*Coming soon*

## Requirements

- Windows 10 (1809+) or Windows 11
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Windows App SDK](https://learn.microsoft.com/windows/apps/windows-app-sdk/)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/winui-calculator.git
cd winui-calculator
```

### Build and Run

```bash
dotnet build
dotnet run
```

### Run Tests

```bash
dotnet test calculator.Tests
```

## Project Structure

```
calculator/
├── Commands/
│   └── RelayCommand.cs           # ICommand implementation for MVVM
├── Helpers/
│   └── ServiceLocator.cs         # Lightweight dependency injection
├── Services/
│   ├── ICalculatorEngine.cs      # Calculator logic interface
│   └── CalculatorEngine.cs       # Math operations implementation
├── ViewModels/
│   └── CalculatorViewModel.cs    # Main ViewModel with all commands
├── Styles/
│   └── CalculatorStyles.xaml     # Button styles, animations, colors
├── Views/
│   ├── MainPage.xaml             # Calculator UI layout
│   └── MainPage.xaml.cs          # Keyboard handling
├── calculator.Tests/             # Unit tests (xUnit)
├── App.xaml                      # Resource dictionary references
└── App.xaml.cs                   # Window configuration, Mica backdrop
```

## Architecture

This project follows the **MVVM (Model-View-ViewModel)** pattern:

- **Model**: `CalculatorEngine` handles all mathematical operations
- **ViewModel**: `CalculatorViewModel` manages UI state and commands
- **View**: `MainPage.xaml` defines the calculator UI

```
┌─────────────┐     ┌──────────────────┐     ┌──────────────────┐
│  MainPage   │────▶│CalculatorViewModel│────▶│ CalculatorEngine │
│   (View)    │     │   (ViewModel)     │     │    (Service)     │
└─────────────┘     └──────────────────┘     └──────────────────┘
      │                      │
      │      x:Bind          │      ICommand
      └──────────────────────┘
```

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `0-9` | Enter digits |
| `.` | Decimal point |
| `+` | Add |
| `-` | Subtract |
| `*` | Multiply |
| `/` | Divide |
| `Enter` | Calculate result |
| `Backspace` | Delete last digit |
| `Escape` | Clear all |
| `Delete` | Clear entry |
| `Ctrl+C` | Copy result |

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines

- Follow MVVM pattern
- Add unit tests for new calculator operations
- Use Fluent Design guidelines for UI changes
- Ensure accessibility (AutomationProperties, keyboard navigation)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [WinUI 3](https://github.com/microsoft/microsoft-ui-xaml)
- Inspired by the Windows Calculator
- Icons from [Segoe Fluent Icons](https://learn.microsoft.com/windows/apps/design/style/segoe-fluent-icons-font)
