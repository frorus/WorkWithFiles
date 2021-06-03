using System;
using System.Collections.Generic;
using System.IO;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите директорию для очистки: ");
            string dirName = Console.ReadLine();

            while(!Directory.Exists(dirName))
            {
                Console.WriteLine("Каталог не найден, попробуйте снова.\n");
                dirName = Console.ReadLine();
            }

                DeleteFilesAndFolders(dirName);
        }


        static void DeleteFilesAndFolders(string dirName)
        {
            DateTime fileLastWriteTime;
            DateTime dirLastWriteTime;
            List<string> notUsedFiles = new();
            List<string> notUsedFolders = new();

            //Delete files
            string[] files = Directory.GetFiles(dirName);

            foreach (string s in files)
            {
                fileLastWriteTime = File.GetLastWriteTime(s);
                if ((DateTime.Now - fileLastWriteTime) > TimeSpan.FromMinutes(5))
                {
                    notUsedFiles.Add(s);
                }
            }

            Console.WriteLine("\nПолучен список неиспользуемых файлов.");
            Console.WriteLine("Производится удаление...\n");

            foreach (string s in notUsedFiles)
            {
                try
                {
                    File.Delete(s);
                    Console.WriteLine($"Файл {s} удален");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удалось удалить файл. Причина: " + ex.Message);
                }
            }

            //Delete folders
            string[] dirs = Directory.GetDirectories(dirName);

            foreach (string s in dirs)
            {
                dirLastWriteTime = Directory.GetLastWriteTime(s);
                if ((DateTime.Now - dirLastWriteTime) > TimeSpan.FromMinutes(5))
                {
                    notUsedFolders.Add(s);
                }
            }

            Console.WriteLine("\nПолучен список неиспользуемых папок.");
            Console.WriteLine("Производится удаление...\n");

            foreach (string s in notUsedFolders)
            {
                try
                {
                    DirectoryInfo dirInfo = new(s);
                    dirInfo.Delete(true);
                    Console.WriteLine($"Каталог {s} удален");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удалось удалить папку. Причина: " + ex.Message);
                }
            }
        }
    }
}
