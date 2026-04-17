using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace _4_3_a_
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("procfile_string", 10000))
            {
                bool mutexCreated;
                Console.WriteLine("\t\t Процес А.");
                Console.WriteLine("Створення м'ютекса.");
                Mutex mutex = new Mutex(true, "testmapmutex_string", out mutexCreated);
                
                Console.WriteLine("Непостійний зіставлений у пам'яті файл.");
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    Console.WriteLine("Запис першого слова: 'Hello, '");
                    writer.Write("Hello, ");
                }
                
                mutex.ReleaseMutex();
                
                Process procB = new Process();
                procB.StartInfo.FileName = @"C:\Users\makov\Desktop\PR4\3.4.3_b\bin\Debug\net9.0\3.4.3_b.exe"; 
                procB.Start();
                
                Console.WriteLine("Запуск процеса В. Для продовження натиснути Enter.");
                Console.ReadLine();
                
                mutex.WaitOne();
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryReader reader = new BinaryReader(stream);
                    string word1 = reader.ReadString();
                    string word2 = reader.ReadString();
                    Console.WriteLine($"Фінальний результат: {word1}{word2}");
                }
                mutex.ReleaseMutex();
            }
            Console.ReadKey();
        }
    }
}