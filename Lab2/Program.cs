using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2;
class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();

        library.AddPublication(new Book("Евгнений Онегин", "Пушкин", 1831, "Роман"));
        library.AddPublication(new Book("Капитанская дочка", "Пушкин", 1836, "Историческая повесть"));
        library.AddPublication(new Book("Мастер и Маргарита", "Булгаков", 1967, "Мистика"));
        library.AddPublication(new Jurnal("The Voice", "Редакция", 2026, 100));
        library.AddPublication(new El_Isdanie("Безмолвный пациент", "Михаэлидеса", 2019, "PDF"));
        Console.WriteLine();


        Console.WriteLine("Поиск по автору \"Пушкин\":");
        List<Publication> found = library.FindByAuthor("Пушкин");
        if (found.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
        }
        else
        {
            foreach (Publication p in found)
            {
                Console.WriteLine(p.GetInfo());
            }
        }

        Console.WriteLine();
        Console.WriteLine("Выдача книги \"Капитанская дочка\":");
        library.GetAll()[1].Issue();
        Console.WriteLine(library.GetAll()[1].GetInfo());

        Console.WriteLine();
        Console.WriteLine("Возврат книги \"Капитанская дочка\":");
        library.GetAll()[1].Return();
        Console.WriteLine(library.GetAll()[1].GetInfo());

        Console.ReadLine();
    }
}