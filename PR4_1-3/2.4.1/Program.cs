using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace l_4_1_String
{
    class Program
    {
        static void Main(string[] args)
        {
            string textData = "Привіт! Це тестовий рядок для запису у пам'ять.";
            
            Console.WriteLine("Дані для запису: " + textData);
            Console.WriteLine("\nЗіставлений у пам'яті файл з файлу на диску.");

            using (MemoryMappedFile mnf = MemoryMappedFile.CreateFromFile("a1_string.dta", FileMode.OpenOrCreate, "fileString", 1024))
            {
                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(textData);
                    writer.Close();
                    Console.WriteLine("Файл на диску створений та закритий.");
                }

                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    string readText = reader.ReadString();
                    Console.WriteLine("\nЗчитано з файлу: " + readText);
                }
            }
            Console.ReadKey();
        }
    }
}