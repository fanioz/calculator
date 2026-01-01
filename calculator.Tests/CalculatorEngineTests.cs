using Xunit;
using calculator.Services;

namespace calculator.Tests;

/// <summary>
/// Unit tests for the CalculatorEngine service.
/// Tests all mathematical operations and error handling.
/// </summary>
public class CalculatorEngineTests
{
    private readonly CalculatorEngine _engine;

    public CalculatorEngineTests()
    {
        _engine = new CalculatorEngine();
    }

    #region Binary Operations - Addition

    [Theory]
    [InlineData(5, 3, 8)]
    [InlineData(0, 0, 0)]
    [InlineData(-5, 3, -2)]
    [InlineData(-5, -3, -8)]
    [InlineData(1.5, 2.5, 4)]
    public void Calculate_Addition_ReturnsCorrectResult(double left, double right, double expected)
    {
        var result = _engine.Calculate(left, "+", right);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Binary Operations - Subtraction

    [Theory]
    [InlineData(5, 3, 2)]
    [InlineData(3, 5, -2)]
    [InlineData(0, 0, 0)]
    [InlineData(-5, -3, -2)]
    [InlineData(10.5, 0.5, 10)]
    public void Calculate_Subtraction_ReturnsCorrectResult(double left, double right, double expected)
    {
        var result = _engine.Calculate(left, "−", right);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void Calculate_Subtraction_WithDashSymbol_ReturnsCorrectResult()
    {
        var result = _engine.Calculate(10, "-", 3);
        Assert.Equal(7, result);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Binary Operations - Multiplication

    [Theory]
    [InlineData(5, 3, 15)]
    [InlineData(0, 100, 0)]
    [InlineData(-5, 3, -15)]
    [InlineData(-5, -3, 15)]
    [InlineData(2.5, 4, 10)]
    public void Calculate_Multiplication_ReturnsCorrectResult(double left, double right, double expected)
    {
        var result = _engine.Calculate(left, "×", right);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void Calculate_Multiplication_WithAsterisk_ReturnsCorrectResult()
    {
        var result = _engine.Calculate(6, "*", 7);
        Assert.Equal(42, result);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Binary Operations - Division

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(15, 3, 5)]
    [InlineData(-10, 2, -5)]
    [InlineData(7, 2, 3.5)]
    public void Calculate_Division_ReturnsCorrectResult(double left, double right, double expected)
    {
        var result = _engine.Calculate(left, "÷", right);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void Calculate_Division_WithSlash_ReturnsCorrectResult()
    {
        var result = _engine.Calculate(20, "/", 4);
        Assert.Equal(5, result);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void Calculate_DivisionByZero_ReturnsNaNAndSetsError()
    {
        var result = _engine.Calculate(10, "÷", 0);
        Assert.True(double.IsNaN(result));
        Assert.True(_engine.HasError);
        Assert.Equal("Cannot divide by zero", _engine.ErrorMessage);
    }

    #endregion

    #region Binary Operations - Percent

    [Theory]
    [InlineData(100, 10, 10)]
    [InlineData(200, 25, 50)]
    [InlineData(50, 50, 25)]
    public void Calculate_Percent_ReturnsCorrectResult(double left, double right, double expected)
    {
        var result = _engine.Calculate(left, "%", right);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Unary Operations - Square Root

    [Theory]
    [InlineData(4, 2)]
    [InlineData(9, 3)]
    [InlineData(16, 4)]
    [InlineData(0, 0)]
    public void CalculateUnary_SquareRoot_ReturnsCorrectResult(double value, double expected)
    {
        var result = _engine.CalculateUnary("sqrt", value);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void CalculateUnary_SquareRoot_WithSymbol_ReturnsCorrectResult()
    {
        var result = _engine.CalculateUnary("√", 25);
        Assert.Equal(5, result);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void CalculateUnary_SquareRootOfNegative_ReturnsNaNAndSetsError()
    {
        var result = _engine.CalculateUnary("sqrt", -4);
        Assert.True(double.IsNaN(result));
        Assert.True(_engine.HasError);
        Assert.Equal("Invalid input", _engine.ErrorMessage);
    }

    #endregion

    #region Unary Operations - Negate

    [Theory]
    [InlineData(5, -5)]
    [InlineData(-5, 5)]
    [InlineData(0, 0)]
    [InlineData(3.14, -3.14)]
    public void CalculateUnary_Negate_ReturnsCorrectResult(double value, double expected)
    {
        var result = _engine.CalculateUnary("±", value);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void CalculateUnary_Negate_WithText_ReturnsCorrectResult()
    {
        var result = _engine.CalculateUnary("negate", 7);
        Assert.Equal(-7, result);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Unary Operations - Reciprocal

    [Theory]
    [InlineData(2, 0.5)]
    [InlineData(4, 0.25)]
    [InlineData(0.5, 2)]
    [InlineData(-2, -0.5)]
    public void CalculateUnary_Reciprocal_ReturnsCorrectResult(double value, double expected)
    {
        var result = _engine.CalculateUnary("reciprocal", value);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void CalculateUnary_Reciprocal_WithSymbol_ReturnsCorrectResult()
    {
        var result = _engine.CalculateUnary("1/x", 5);
        Assert.Equal(0.2, result);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void CalculateUnary_ReciprocalOfZero_ReturnsNaNAndSetsError()
    {
        var result = _engine.CalculateUnary("reciprocal", 0);
        Assert.True(double.IsNaN(result));
        Assert.True(_engine.HasError);
        Assert.Equal("Cannot divide by zero", _engine.ErrorMessage);
    }

    #endregion

    #region Unary Operations - Square

    [Theory]
    [InlineData(2, 4)]
    [InlineData(3, 9)]
    [InlineData(-2, 4)]
    [InlineData(0, 0)]
    [InlineData(1.5, 2.25)]
    public void CalculateUnary_Square_ReturnsCorrectResult(double value, double expected)
    {
        var result = _engine.CalculateUnary("square", value);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    [Fact]
    public void CalculateUnary_Square_WithSymbol_ReturnsCorrectResult()
    {
        var result = _engine.CalculateUnary("x²", 5);
        Assert.Equal(25, result);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Unary Operations - Percent

    [Theory]
    [InlineData(50, 0.5)]
    [InlineData(100, 1)]
    [InlineData(25, 0.25)]
    public void CalculateUnary_Percent_ReturnsCorrectResult(double value, double expected)
    {
        var result = _engine.CalculateUnary("percent", value);
        Assert.Equal(expected, result, 10);
        Assert.False(_engine.HasError);
    }

    #endregion

    #region Error Handling

    [Fact]
    public void ClearError_ResetsErrorState()
    {
        _engine.Calculate(10, "÷", 0);
        Assert.True(_engine.HasError);

        _engine.ClearError();
        Assert.False(_engine.HasError);
        Assert.Null(_engine.ErrorMessage);
    }

    [Fact]
    public void Calculate_UnknownOperator_SetsError()
    {
        var result = _engine.Calculate(5, "^", 2);
        Assert.True(double.IsNaN(result));
        Assert.True(_engine.HasError);
    }

    #endregion

    #region Number Formatting

    [Theory]
    [InlineData(123, "123")]
    [InlineData(0, "0")]
    [InlineData(-456, "-456")]
    [InlineData(3.14159, "3.14159")]
    public void FormatNumber_ReturnsCorrectString(double value, string expected)
    {
        var result = _engine.FormatNumber(value);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatNumber_NaN_ReturnsErrorMessage()
    {
        _engine.Calculate(1, "÷", 0);
        var result = _engine.FormatNumber(double.NaN);
        Assert.Equal("Cannot divide by zero", result);
    }

    [Fact]
    public void FormatNumber_Infinity_ReturnsOverflow()
    {
        var result = _engine.FormatNumber(double.PositiveInfinity);
        Assert.Equal("Overflow", result);
    }

    #endregion
}
