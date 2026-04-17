using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace _4_3_a
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("procfile", 10000))
            {
               bool mutexCreated;
               Console.WriteLine("\t\t Процес А.");
               Console.WriteLine("Створення мютекса.");
               Mutex mutex = new Mutex(true, "testmapmutex", out mutexCreated);
               Console.WriteLine("Непостійний зіставлений у пам'яті файл.");
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    Console.WriteLine("Запис даних.");
                    writer.Write(true);
                }
                mutex.ReleaseMutex();
                Process procB = new Process();
                procB.StartInfo.FileName = @"C:\Users\makov\Desktop\PR4\4.3_b\bin\Debug\net9.0\4.3.exe";
                procB.Start();
                Console.WriteLine("Запуск процеса В. Для продовження натиснути Enter.");
                Console.ReadLine();
                mutex.WaitOne();
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    Console.WriteLine("Процес А передав: {0}", reader.ReadBoolean());
                    Console.WriteLine("Процес В передав: {0}", reader.ReadBoolean());
                }
                mutex.ReleaseMutex();
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }
}