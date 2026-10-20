using System;
using Lab4.Core;

namespace Lab4.Tests;

public class BookTests
{   
    //Проверка на содержание
    [Fact]
    public void GetInfo_ReturnsCorrectString()
    {
        var book = new Book("Евгнений Онегин", "Пушкин", 1831, "Роман");

        string info = book.GetInfo();

        Assert.Contains("Евгнений Онегин", info);
        Assert.Contains("Пушкин", info);
        Assert.Contains("Роман", info);
    }

    //Проверка года
    [Fact]
    public void Constructor_InvalidYear_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Book("Технологии программирования. Часть 2", "Порохин", 2027, "Триллер"));
    }
}
