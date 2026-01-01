namespace calculator.Services;

/// <summary>
/// Defines the calculator computation engine interface.
/// </summary>
public interface ICalculatorEngine
{
    /// <summary>
    /// Performs a binary operation on two operands.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="operatorSymbol">The operator symbol (+, −, ×, ÷).</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operation.</returns>
    double Calculate(double left, string operatorSymbol, double right);

    /// <summary>
    /// Performs a unary operation on a single operand.
    /// </summary>
    /// <param name="operatorSymbol">The unary operator (√, ±, 1/x, x²).</param>
    /// <param name="value">The operand value.</param>
    /// <returns>The result of the operation.</returns>
    double CalculateUnary(string operatorSymbol, double value);

    /// <summary>
    /// Formats a number for display with appropriate precision.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A formatted string representation.</returns>
    string FormatNumber(double value);

    /// <summary>
    /// Checks if the last operation resulted in an error.
    /// </summary>
    bool HasError { get; }

    /// <summary>
    /// Gets the error message from the last operation, if any.
    /// </summary>
    string? ErrorMessage { get; }

    /// <summary>
    /// Clears any error state.
    /// </summary>
    void ClearError();
}
