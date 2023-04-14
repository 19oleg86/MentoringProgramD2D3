/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code
            
            CreateThreadRecursivelyWithThreadAndJoin(10);
            CreateThreadRecursivelyWithThreadPoolAndSemaphore(10);

            Console.ReadLine();
        }

        #region With Thread class and Join for waiting threads

        private static void CreateThreadRecursivelyWithThreadAndJoin(int threadNumber)
        {
            var threads = new Thread[threadNumber];
            var current = 0;
            
            while (current < threadNumber)
            {
                threads[current] = new Thread(number =>
                {
                    Console.WriteLine($"Thread {number} is executing");
                });
                threads[current].Start(current + 1);
                current++;
            };
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine($"All ({threads.Length}) Threads with Thread and Join have finished execution");
            Console.WriteLine();
        }
        #endregion

        #region With ThreadPool class and Semaphore for waiting threads

        private static void CreateThreadRecursivelyWithThreadPoolAndSemaphore(int threadNumber)
        {
            var semaphore = new Semaphore(0, threadNumber);
            var current = 1;
            while(current <= threadNumber)
            {
                ThreadPool.QueueUserWorkItem(number =>
                {
                    Console.WriteLine($"Thread {number} is executing");
                    semaphore.Release();
                }, current);
                current++;
            };

            for (int i = 0; i < threadNumber; i++)
            {
                semaphore.WaitOne();
            }
            
            Console.WriteLine($"All ({threadNumber}) Threads with ThreadPool and Semaphore have finished execution");
            Console.WriteLine();
        }
        #endregion

    }
}
