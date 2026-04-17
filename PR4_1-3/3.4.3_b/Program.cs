using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace _4_3_b_
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("procfile_string"))
                {
                    Console.WriteLine("\t\t Процес В.");
                    Console.WriteLine("Відкривається м'ютекс.");
                    Mutex mutex = Mutex.OpenExisting("testmapmutex_string");
                    mutex.WaitOne();
                    
                    Console.WriteLine("Непостійний зіставлений у пам'яті файл.");
                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        string firstWord = reader.ReadString(); 
                        
                        BinaryWriter writer = new BinaryWriter(stream);
                        Console.WriteLine("Дописуємо друге слово: 'World!'");
                        writer.Write("World!");
                    }
                    mutex.ReleaseMutex();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Непостійний зіставлений у пам'яті файл не існує! Запустіть процес А першим!");
                Console.ReadKey();
            }
        }
    }
}