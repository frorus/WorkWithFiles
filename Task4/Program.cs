using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите директорию файла: ");
            string dirName = Console.ReadLine();

            while (!File.Exists(dirName))
            {
                Console.WriteLine("Файл не найден, попробуйте снова.\n");
                dirName = Console.ReadLine();
            }

            var students = ReadFile(dirName);
            SaveData(students);
        }

        public static Student[] ReadFile(string s)
        {

            BinaryFormatter formatter = new();

            Console.WriteLine($"Данные из файла:\n");
            using (var fs = new FileStream(s, FileMode.Open))
            {
                var newStudent = (Student[])formatter.Deserialize(fs);
                foreach (var item in newStudent)
                {
                    Console.WriteLine($"Студент: {item.Name}\t{item.Group}\t{item.DateOfBirth}");
                }
                return newStudent;
            }

        }

        public static bool ShowDirInfo(string s)
        {
            DirectoryInfo dir = new(s);

            if (dir.Exists)
            {
                var filelist = dir.GetFiles();

                foreach (var item in filelist)
                {
                    Console.WriteLine(item.Name);
                }
                return true;
            }
            else
            {
                Console.WriteLine("Указанный каталог не существует:");
                return false;
            }
        }

        public static void SaveData(Student[] students)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Students";
            Directory.CreateDirectory(path);

            var groupList = new List<string>();
            foreach (var item in students)
            {
                if (!groupList.Contains(item.Group))
                {
                    groupList.Add(item.Group);
                }
            }

            foreach (var item in groupList)
            {
                using (StreamWriter sw = File.CreateText(path + "\\" + item + ".txt"))
                {
                    foreach (var student in students)
                    {
                        if (student.Group == item)
                        {
                            sw.WriteLine($"{student.Name}\t{student.DateOfBirth}");
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Данные о студентах распределены по следующим файлам: \n");
            ShowDirInfo(path);
        }
    }


    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Student(string name, string group, DateTime dateofbirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateofbirth;
        }
    }
}
