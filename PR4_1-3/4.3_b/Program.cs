using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace _4_3_b
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("procfile"))
                {
                    Console.WriteLine("\t\t Процес В.");
                    Console.WriteLine("Відкривається мютекс.");
                    Mutex mutex = Mutex.OpenExisting("testmapmutex");
                    mutex.WaitOne();
                    Console.WriteLine("Неепостійний зіставлений у пам'яті файл.");
                    using (MemoryMappedViewStream stream = mmf.CreateViewStream(1, 0))
                    {
                        BinaryWriter writer = new BinaryWriter(stream);
                        Console.WriteLine("Запис даних.");
                        writer.Write(false);
                    }
                    mutex.ReleaseMutex();
                
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Непостійний зіставлений у пам'яті файл не існує! Запустіть процес А першим!.");
                Console.ReadKey();
            }
        }
    }
}