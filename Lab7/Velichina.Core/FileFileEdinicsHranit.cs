using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Velichina.Core
{// сохраняет данные в текстовый файл на жестком диске, нужно в реальной программе — чтобы твои созданные единицы не пропадали после перезапуска
    public class FileEdinicsHranit : IEdinicsHranit
    {
        private readonly string pathFile;

        public FileEdinicsHranit(string path)
        {
            pathFile = path;
        }

        public List<SvojaEdinica> Load()
        {
            List<SvojaEdinica> result = new List<SvojaEdinica>(); // создали пустой список, сюда вложим единицы, прочитанные из файла

            if (!File.Exists(pathFile))
            {
                return result;
            }

            string[] lines = File.ReadAllLines(pathFile);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length != 3)
                {
                    continue;
                }

                double znach;
                if (double.TryParse(parts[2], out znach))
                {
                    result.Add(new SvojaEdinica(parts[0], parts[1], znach));
                }
            }

            return result;
        }

        public void Add(SvojaEdinica edinica)
        {
            string znachText = edinica.ZnachKoeff.ToString(); //число в строку
            string line = edinica.Category + ";" + edinica.Name + ";" + znachText;
            File.AppendAllText(pathFile, line + Environment.NewLine);
        }

    }
}
