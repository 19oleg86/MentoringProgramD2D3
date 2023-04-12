/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> collection = new List<int>();
        static ManualResetEventSlim event1 = new ManualResetEventSlim(false);
        static ManualResetEventSlim event2 = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            Thread thread1 = new Thread(AddElements) { Name = "Thread 1", };
            Thread thread2 = new Thread(PrintElements) { Name = "Thread 2" };
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Console.WriteLine("Finished!");

            Console.ReadLine();
        }

        static void AddElements()
        {
            while (collection.Count <= 10)
            {
                for (int i = 1; i <= 10; i++)
                {
                    event1.Wait();
                    collection.Add(i);
                    Console.WriteLine($"{Thread.CurrentThread.Name} Added element {i} to the collection");
                    event1.Reset();
                    event2.Set();
                }
            }
        }

        static void PrintElements()
        {
            while (collection.Count <= 10)
            {
                Console.Write($"{Thread.CurrentThread.Name} prints collection: ");
                for (int i = 0; i < collection.Count; i++)
                {
                    Console.Write($"{collection[i]}, ");
                }
                Console.WriteLine();
                event1.Set();
                event2.Wait();
                event2.Reset();
            }
        }
    }
}
