using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using calculator.Commands;
using calculator.Helpers;
using calculator.Services;
using Windows.ApplicationModel.DataTransfer;

namespace calculator.ViewModels;

/// <summary>
/// ViewModel for the Calculator, implementing MVVM pattern.
/// Manages display state, expression history, and all calculator commands.
/// </summary>
public class CalculatorViewModel : INotifyPropertyChanged
{
    private readonly ICalculatorEngine _engine;

    // Display state
    private string _displayValue = "0";
    private string _expressionHistory = "";

    // Calculator state
    private double _accumulator;
    private string? _pendingOperator;
    private bool _isNewEntry = true;
    private double _lastOperand;
    private string? _lastOperator;
    private bool _isEqualsPressed;

    public event PropertyChangedEventHandler? PropertyChanged;

    #region Properties

    /// <summary>
    /// The current value shown on the calculator display.
    /// </summary>
    public string DisplayValue
    {
        get => _displayValue;
        set
        {
            if (_displayValue != value)
            {
                _displayValue = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// The expression history shown above the display.
    /// </summary>
    public string ExpressionHistory
    {
        get => _expressionHistory;
        set
        {
            if (_expressionHistory != value)
            {
                _expressionHistory = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Indicates if the equals button was just pressed (for visual feedback).
    /// </summary>
    public bool IsEqualsPressed
    {
        get => _isEqualsPressed;
        set
        {
            if (_isEqualsPressed != value)
            {
                _isEqualsPressed = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Commands

    public ICommand DigitInputCommand { get; }
    public ICommand OperationCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ClearEntryCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand NegateCommand { get; }
    public ICommand PercentCommand { get; }
    public ICommand AdvancedCommand { get; }
    public ICommand CopyResultCommand { get; }

    #endregion

    public CalculatorViewModel() : this(ServiceLocator.Resolve<ICalculatorEngine>())
    {
    }

    public CalculatorViewModel(ICalculatorEngine engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));

        // Initialize commands
        DigitInputCommand = new RelayCommand<string>(ExecuteDigitInput);
        OperationCommand = new RelayCommand<string>(ExecuteOperation);
        EqualsCommand = new RelayCommand(ExecuteEquals);
        ClearCommand = new RelayCommand(ExecuteClear);
        ClearEntryCommand = new RelayCommand(ExecuteClearEntry);
        BackspaceCommand = new RelayCommand(ExecuteBackspace);
        NegateCommand = new RelayCommand(ExecuteNegate);
        PercentCommand = new RelayCommand(ExecutePercent);
        AdvancedCommand = new RelayCommand<string>(ExecuteAdvanced);
        CopyResultCommand = new RelayCommand(ExecuteCopyResult);
    }

    #region Command Implementations

    private void ExecuteDigitInput(string? digit)
    {
        if (string.IsNullOrEmpty(digit)) return;

        _engine.ClearError();
        IsEqualsPressed = false;

        if (_isNewEntry)
        {
            _displayValue = digit == "." ? "0." : digit;
            _isNewEntry = false;
        }
        else
        {
            // Handle decimal point
            if (digit == "." && _displayValue.Contains('.'))
                return;

            // Limit input length
            if (_displayValue.Replace(".", "").Replace("-", "").Length >= 16)
                return;

            // Handle leading zero
            if (_displayValue == "0" && digit != ".")
            {
                _displayValue = digit;
            }
            else
            {
                _displayValue += digit;
            }
        }

        OnPropertyChanged(nameof(DisplayValue));
    }

    private void ExecuteOperation(string? operatorSymbol)
    {
        if (string.IsNullOrEmpty(operatorSymbol)) return;

        _engine.ClearError();
        IsEqualsPressed = false;

        var currentValue = ParseDisplay();

        if (_pendingOperator != null && !_isNewEntry)
        {
            // Calculate pending operation
            var result = _engine.Calculate(_accumulator, _pendingOperator, currentValue);

            if (_engine.HasError)
            {
                DisplayValue = _engine.ErrorMessage ?? "Error";
                ExpressionHistory = "";
                ResetState();
                return;
            }

            _accumulator = result;
            DisplayValue = _engine.FormatNumber(result);
        }
        else
        {
            _accumulator = currentValue;
        }

        _pendingOperator = operatorSymbol;
        _isNewEntry = true;
        ExpressionHistory = $"{_engine.FormatNumber(_accumulator)} {operatorSymbol}";
    }

    private void ExecuteEquals()
    {
        _engine.ClearError();

        double currentValue;
        string operatorToUse;

        if (_isEqualsPressed && _lastOperator != null)
        {
            // Repeat last operation
            currentValue = _lastOperand;
            operatorToUse = _lastOperator;
            _accumulator = ParseDisplay();
        }
        else if (_pendingOperator != null)
        {
            currentValue = ParseDisplay();
            operatorToUse = _pendingOperator;
            _lastOperand = currentValue;
            _lastOperator = _pendingOperator;
        }
        else
        {
            // No pending operation, just show current value
            IsEqualsPressed = true;
            return;
        }

        var result = _engine.Calculate(_accumulator, operatorToUse, currentValue);

        if (_engine.HasError)
        {
            DisplayValue = _engine.ErrorMessage ?? "Error";
            ExpressionHistory = "";
            ResetState();
            return;
        }

        ExpressionHistory = $"{_engine.FormatNumber(_accumulator)} {operatorToUse} {_engine.FormatNumber(currentValue)} =";
        DisplayValue = _engine.FormatNumber(result);
        _accumulator = result;
        _pendingOperator = null;
        _isNewEntry = true;
        IsEqualsPressed = true;
    }

    private void ExecuteClear()
    {
        ResetState();
        DisplayValue = "0";
        ExpressionHistory = "";
        _engine.ClearError();
    }

    private void ExecuteClearEntry()
    {
        DisplayValue = "0";
        _isNewEntry = true;
        _engine.ClearError();
    }

    private void ExecuteBackspace()
    {
        if (_isNewEntry || _engine.HasError)
        {
            DisplayValue = "0";
            _isNewEntry = true;
            return;
        }

        if (_displayValue.Length > 1)
        {
            _displayValue = _displayValue[..^1];
            if (_displayValue == "-" || _displayValue == "")
            {
                _displayValue = "0";
                _isNewEntry = true;
            }
        }
        else
        {
            _displayValue = "0";
            _isNewEntry = true;
        }

        OnPropertyChanged(nameof(DisplayValue));
    }

    private void ExecuteNegate()
    {
        var currentValue = ParseDisplay();
        var result = _engine.CalculateUnary("±", currentValue);
        DisplayValue = _engine.FormatNumber(result);
        _isNewEntry = false;
    }

    private void ExecutePercent()
    {
        var currentValue = ParseDisplay();
        double result;

        if (_pendingOperator != null)
        {
            // Calculate percentage of accumulator
            result = _engine.Calculate(_accumulator, "%", currentValue);
        }
        else
        {
            result = _engine.CalculateUnary("%", currentValue);
        }

        DisplayValue = _engine.FormatNumber(result);
        _isNewEntry = false;
    }

    private void ExecuteAdvanced(string? function)
    {
        if (string.IsNullOrEmpty(function)) return;

        var currentValue = ParseDisplay();
        var result = _engine.CalculateUnary(function, currentValue);

        if (_engine.HasError)
        {
            DisplayValue = _engine.ErrorMessage ?? "Error";
            ResetState();
            return;
        }

        // Update expression history to show the operation
        string functionDisplay = function switch
        {
            "sqrt" => $"√({_engine.FormatNumber(currentValue)})",
            "square" => $"sqr({_engine.FormatNumber(currentValue)})",
            "reciprocal" => $"1/({_engine.FormatNumber(currentValue)})",
            _ => function
        };

        ExpressionHistory = functionDisplay;
        DisplayValue = _engine.FormatNumber(result);
        _isNewEntry = true;
    }

    private void ExecuteCopyResult()
    {
        var dataPackage = new DataPackage();
        dataPackage.SetText(DisplayValue);
        Clipboard.SetContent(dataPackage);
    }

    #endregion

    #region Helper Methods

    private double ParseDisplay()
    {
        if (double.TryParse(_displayValue, out var value))
            return value;
        return 0;
    }

    private void ResetState()
    {
        _accumulator = 0;
        _pendingOperator = null;
        _isNewEntry = true;
        _lastOperand = 0;
        _lastOperator = null;
        IsEqualsPressed = false;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
