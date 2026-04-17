using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace l_4_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] mas = new int[] { 1, 32, 45, 11, -5, 0, 78, 52 };
            List<int> inputMas = new List<int>();

            Console.WriteLine("Масив.");
            for (int i = 0; i < mas.Length; i++)
                Console.Write(mas[i] + " ");

            Console.WriteLine("\n Зіставлений у пам'яті файл з файлу на диску.");

            using (MemoryMappedFile mnf =
                MemoryMappedFile.CreateFromFile("a1.dta", FileMode.OpenOrCreate, "file", 1024))
            {
                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);

                    foreach (int i in mas)
                        writer.Write(i);

                    writer.Close();
                    Console.WriteLine("\n Файл на диску створений та закритий.");
                }

                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);

                    for (int i = 0; i < mas.Length; i++)
                        inputMas.Add(reader.ReadInt32());
                }

                foreach (int i in inputMas)
                    Console.Write(i + " ");

                Console.ReadKey();
            }
        }
    }
}
