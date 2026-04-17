using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace l_4_2_String
{
    class Program
    {
        static void Main(string[] args)
        {
            string textData = "Цей рядок живе лише у пам'яті!";
            Console.WriteLine("Дані для запису: " + textData);
            Console.WriteLine("\nНепостійний зіставлений у пам'яті файл.");

            using (MemoryMappedFile mnf = MemoryMappedFile.CreateNew("n_file_string", 4096))
            {
                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(textData);
                }

                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    string readText = reader.ReadString();
                    Console.WriteLine("\nЗчитано з пам'яті: " + readText);
                }
            }
            Console.ReadKey();
        }
    }
}