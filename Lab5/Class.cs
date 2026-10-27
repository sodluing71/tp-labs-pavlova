using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public class Class
    {
        private static char[] Alphabet = {
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й',
            'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф',
            'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я'
        };

        public static string Schifr(string cipherText, int sdvig)
        {
            char[] result = new char[cipherText.Length];
            int n = Alphabet.Length;

            for (int i = 0; i < cipherText.Length; i++)
            {
                char ch = char.ToUpper(cipherText[i]);
                int index = Array.IndexOf(Alphabet, ch);

                if (index != -1)
                {
                    int newIndex = index - sdvig;
                    if (newIndex < 0)
                    {
                        newIndex = newIndex + n;
                    }

                    result[i] = Alphabet[newIndex];
                }
                else
                {
                    result[i] = ch;
                }
            }
            return new string(result);
        }
    }
}
