using System;
using Lab4.Core;

namespace Lab4.Tests;

public class PublicationTests
{
    // Выдаем книгу
    [Fact]
    public void Issue_ChangesStateToIssued()
    {
        var book = new Book("Технологии программирования", "Порохин", 2026, "Триллер");
        book.Issue(); 
        Assert.True(book.IsIssued); 
    }

    // Возвращаем книгу
    [Fact]
    public void Return_ChangesStateToAvailable()
    {
        var book = new Book("Технологии программирования", "Порохин", 2026, "Триллер");
        book.Issue(); 

        book.Return(); 

        Assert.False(book.IsIssued);
    }
}
