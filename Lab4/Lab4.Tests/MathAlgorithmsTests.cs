using System;
using Lab4.Core;

namespace Lab4.Tests;

public class MathAlgorithmsTests
{   
    // Факториал
    [Fact]
    public void Factorial_OfZero_ReturnsOne()
    {
        // Arrange + Act
        long result = MathAlgorithms.Factorial(0);
        // Assert
        Assert.Equal(1, result);
    }
    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 120)]
    [InlineData(10, 3628800)]
    [InlineData(20, 2432902008176640000)]
    public void Factorial_ReturnsExpected(int n, long expected)
    {
        Assert.Equal(expected, MathAlgorithms.Factorial(n));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public void Factorial_OutOfRange_Throws(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Factorial(n));
    }


    // Фибоначчи
    [Fact]
    public void Metod_Fibonachi_First6_AreCorrect()
    {
        int[] seq = MathAlgorithms.Metod_Fibonachi(5);
        Assert.Equal(new int[] { 0, 1, 1, 2, 3, 5 }, seq);
    }

    [Fact]
    public void Metod_Fibonachi_Zero_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Metod_Fibonachi(0));
    }


    //Ряд Тейлора
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(-0.5)]
    [InlineData(0.9)]
    public void Metod_Teilora_MatchesMathLog(double x)
    {
        double expected = Math.Log(1 + x);
        double actual = MathAlgorithms.Metod_Teilora(x);
        Assert.Equal(expected, actual, 1e-5);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(1.5)]
    public void Metod_Teilora_OutOfRange_Throws(double x)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Metod_Teilora(x));
    }


    // Функция
    [Theory]
    [InlineData(54)]
    [InlineData(100)]
    [InlineData(200)]
    public void Function_ValidX_ReturnsValue(double x)
    {
        double result = MathAlgorithms.Function(x);
        Assert.False(double.IsNaN(result));
    }

    [Fact]
    public void Function_LessThan54_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Function(50));
    }
}
