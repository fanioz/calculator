namespace calculator.Services;

/// <summary>
/// Implementation of the calculator computation engine.
/// Handles all mathematical operations with error handling.
/// </summary>
public class CalculatorEngine : ICalculatorEngine
{
    private const int MaxDisplayDigits = 16;

    public bool HasError { get; private set; }
    public string? ErrorMessage { get; private set; }

    public void ClearError()
    {
        HasError = false;
        ErrorMessage = null;
    }

    public double Calculate(double left, string operatorSymbol, double right)
    {
        ClearError();

        try
        {
            return operatorSymbol switch
            {
                "+" => left + right,
                "−" or "-" => left - right,
                "×" or "*" => left * right,
                "÷" or "/" => CalculateDivision(left, right),
                "%" => left * (right / 100),
                _ => throw new ArgumentException($"Unknown operator: {operatorSymbol}")
            };
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
            return double.NaN;
        }
    }

    private double CalculateDivision(double left, double right)
    {
        if (right == 0)
        {
            HasError = true;
            ErrorMessage = "Cannot divide by zero";
            return double.NaN;
        }
        return left / right;
    }

    public double CalculateUnary(string operatorSymbol, double value)
    {
        ClearError();

        try
        {
            return operatorSymbol switch
            {
                "√" or "sqrt" => CalculateSquareRoot(value),
                "±" or "negate" => -value,
                "1/x" or "reciprocal" => CalculateReciprocal(value),
                "x²" or "square" => value * value,
                "%" or "percent" => value / 100,
                _ => throw new ArgumentException($"Unknown unary operator: {operatorSymbol}")
            };
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = ex.Message;
            return double.NaN;
        }
    }

    private double CalculateSquareRoot(double value)
    {
        if (value < 0)
        {
            HasError = true;
            ErrorMessage = "Invalid input";
            return double.NaN;
        }
        return Math.Sqrt(value);
    }

    private double CalculateReciprocal(double value)
    {
        if (value == 0)
        {
            HasError = true;
            ErrorMessage = "Cannot divide by zero";
            return double.NaN;
        }
        return 1 / value;
    }

    public string FormatNumber(double value)
    {
        if (double.IsNaN(value))
            return ErrorMessage ?? "Error";

        if (double.IsInfinity(value))
            return "Overflow";

        // Handle very large or very small numbers with scientific notation
        if (Math.Abs(value) >= 1e16 || (Math.Abs(value) < 1e-16 && value != 0))
        {
            return value.ToString("G10");
        }

        // Format with appropriate precision
        string formatted = value.ToString("G16");

        // Limit display length
        if (formatted.Length > MaxDisplayDigits)
        {
            // Try to round to fit
            int decimalPlaces = MaxDisplayDigits - formatted.IndexOf('.') - 1;
            if (decimalPlaces > 0)
            {
                formatted = Math.Round(value, Math.Min(decimalPlaces, 15)).ToString("G16");
            }
        }

        return formatted;
    }
}
