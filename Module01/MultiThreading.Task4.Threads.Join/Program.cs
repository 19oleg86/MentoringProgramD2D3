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
    class Program
    {
        static readonly Semaphore semaphore = new Semaphore(0, 10);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code
            
            CreateThreadRecursivelyWithThreadAndJoin(11);

            Console.WriteLine("All Threads with Thread and Join have finished execution");

            Console.WriteLine();

            CreateThreadRecursivelyWithThreadPoolAndSemaphore(11);
            for (int i = 0; i < 10; i++)
            {
                semaphore.WaitOne();
            }
            Console.WriteLine("All Threads with ThreadPool and Semaphore have finished execution");

            Console.ReadLine();
        }

        #region With Thread class and Join for waiting threads
        static void CreateThreadRecursivelyWithThreadAndJoin(int threadNumber)
        {
            if (threadNumber == 1)
            {
                return;
            }
            Thread thread = new Thread(() =>
            {
                threadNumber--;
                Console.WriteLine($"Thread {threadNumber} is executing");
                CreateThreadRecursivelyWithThreadAndJoin(threadNumber);
            });

            thread.Start();
            thread.Join();
        }
        #endregion

        #region With ThreadPool class and Semaphore for waiting threads
        static void CreateThreadRecursivelyWithThreadPoolAndSemaphore(int threadNumber)
        {
            if (threadNumber == 1)
            {
                semaphore.Release();
                return;
            }

            ThreadPool.QueueUserWorkItem(x =>
            {
                threadNumber--;
                Console.WriteLine($"Thread {threadNumber} is executing");
                CreateThreadRecursivelyWithThreadPoolAndSemaphore(threadNumber);
                semaphore.Release();
            });
        }
        #endregion

    }
}
