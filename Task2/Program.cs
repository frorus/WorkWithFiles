using System;
using System.IO;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите директорию: ");
            string dirName = Console.ReadLine();

            while (!Directory.Exists(dirName))
            {
                Console.WriteLine("Каталог не найден, попробуйте снова.\n");
                dirName = Console.ReadLine();
            }

            Console.WriteLine($"Размер папки: {DirSize(dirName)} байт");

        }

        public static long DirSize(string d)
        {
            long size = 0;
            DirectoryInfo info = new(d);

            FileInfo[] fis = info.GetFiles();
            foreach (FileInfo fi in fis)
            {
                try
                {
                    size += fi.Length;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(fi.Name + $" - Не удалось посчитать размер: {ex.Message}");
                }
            }

            DirectoryInfo[] dis = info.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                try
                {
                    size += DirSize(di.FullName);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(di.Name + $" - Не удалось посчитать размер: {ex.Message}");
                }
            }
            return size;
        }
    }
}
