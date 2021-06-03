using System;
using System.Collections.Generic;
using System.IO;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите директорию для очистки: ");
            string dirName = Console.ReadLine();
            long pre;
            long post;

            while (!Directory.Exists(dirName))
            {
                Console.WriteLine("Каталог не найден, попробуйте снова.\n");
                dirName = Console.ReadLine();
            }

            pre = DirSize(dirName);
            Console.WriteLine($"Исходный размер папки: {pre} байт");

            DeleteFilesAndFolders(dirName);

            post = DirSize(dirName);
            Console.WriteLine($"Освобождено {pre - post} байт");
            Console.WriteLine();
            Console.WriteLine($"Текущий размер папки - {post} байт");
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

            foreach (string s in notUsedFiles.ToArray())
            {
                try
                {
                    File.Delete(s);
                    Console.WriteLine($"Файл {s} удален");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удалось удалить файл. Причина: " + ex.Message);
                    notUsedFiles.Remove(s);
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

            foreach (string s in notUsedFolders.ToArray())
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
                    notUsedFolders.Remove(s);
                }
            }

            Console.WriteLine($"Всего удалено файлов - {notUsedFiles.Count}");
            Console.WriteLine($"Всего удалено папок - {notUsedFolders.Count}");

        }
    }
}
