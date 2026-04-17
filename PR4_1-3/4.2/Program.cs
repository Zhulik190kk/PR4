using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace _4_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] mas = new int[] { 1, 32, 45, 11, 0, 78, 52 };
            List<int> inputMas = new List<int>();
            Console.WriteLine("Масив.");
            for (int i = 0; i < mas.Length; i++)
            {
                Console.Write(mas[i] + " ");
            }
            Console.WriteLine("\n Непостійний зіставлений у пам'яті файл.");

            using (MemoryMappedFile mnf = MemoryMappedFile.CreateNew("n_file", 4096))
            {
                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    foreach (int i in mas)
                    {
                        writer.Write(i);
                    }
                }
                using (MemoryMappedViewStream stream = mnf.CreateViewStream())
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (int i = 0; i < mas.Length; i++)
                    {
                        inputMas.Add(reader.ReadInt32());
                    }
                }
            }
            foreach (int i in inputMas)
            {
                Console.Write(i + " ");
            }
            Console.ReadKey();
        }
    }
}
