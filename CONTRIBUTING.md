# Contributing to WinUI Calculator

Thank you for your interest in contributing! This document provides guidelines and instructions for contributing.

## Code of Conduct

Please be respectful and constructive in all interactions. We welcome contributors of all experience levels.

## How to Contribute

### Reporting Bugs

1. Check if the issue already exists in [Issues](https://github.com/YOUR_USERNAME/winui-calculator/issues)
2. If not, create a new issue with:
   - Clear, descriptive title
   - Steps to reproduce
   - Expected vs actual behavior
   - Windows version and .NET version
   - Screenshots if applicable

### Suggesting Features

1. Check existing issues for similar suggestions
2. Create a new issue with the `enhancement` label
3. Describe the feature and its use case
4. Explain how it fits with the existing calculator

### Pull Requests

1. **Fork & Clone**
   ```bash
   git clone https://github.com/YOUR_USERNAME/winui-calculator.git
   cd winui-calculator
   ```

2. **Create a Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Changes**
   - Follow the existing code style
   - Add unit tests for new functionality
   - Ensure all tests pass: `dotnet test`

4. **Commit**
   ```bash
   git commit -m "Add: brief description of changes"
   ```
   
   Commit message prefixes:
   - `Add:` New features
   - `Fix:` Bug fixes
   - `Update:` Changes to existing features
   - `Refactor:` Code improvements without functionality changes
   - `Docs:` Documentation updates
   - `Test:` Test additions or modifications

5. **Push & Create PR**
   ```bash
   git push origin feature/your-feature-name
   ```
   Then open a Pull Request on GitHub.

## Development Setup

### Prerequisites

- Windows 10 (1809+) or Windows 11
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Building

```bash
dotnet build
```

### Running

```bash
dotnet run
```

### Testing

```bash
dotnet test calculator.Tests
```

## Code Style Guidelines

### C# Conventions

- Use `var` when the type is obvious
- Prefer expression-bodied members for simple methods
- Use `#region` for logical groupings
- XML documentation for public APIs

### XAML Conventions

- Use `x:Bind` over `Binding` for performance
- Add `AutomationProperties.Name` for accessibility
- Follow Fluent Design guidelines

### Architecture

- Follow MVVM pattern
- Keep ViewModels free of UI dependencies
- Use `ICommand` for button actions
- Services should be injected via `ServiceLocator`

## Testing Guidelines

- Write tests for all calculator operations
- Use `[Theory]` with `[InlineData]` for parameterized tests
- Test edge cases (division by zero, negative square root, etc.)
- Aim for high coverage of `CalculatorEngine`

## Questions?

Feel free to open an issue with the `question` label if you need help!
