using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public abstract class Publication
    {
        private string title;
        private string author;
        private int year;
        private bool isIssued;

        public string Title
        {
            get { return title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название не может быть пустым");
                title = value;
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Автор не может быть пустым");
                author = value;
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1500 || value > 2026)
                    throw new ArgumentOutOfRangeException(nameof(value), "Неверный год издания");
                year = value;
            }
        }
        public bool IsIssued
        {
            get { return isIssued; }
            private set { isIssued = value; }
        }
        public Publication(string title, string author, int year)
        {
            Title = title;
            Author = author; 
            Year = year;
            IsIssued = false;
        }
        public abstract string GetInfo();
        public override string ToString()
        {
            string status;
            if (IsIssued)
            {
                status = "выдано";
            }
            else
            {
                status = "в наличии";
            }
            return $"{Author} ({Year} г.) — {Title} ({status})";
        }
        public void Issue()
        {
            if (IsIssued)
                throw new InvalidOperationException("Издание выдано");
            IsIssued = true;
        }

        public void Return()
        {
            if (!IsIssued)
                throw new InvalidOperationException("Издание в наличии");
            IsIssued = false;
        }
    }
}
