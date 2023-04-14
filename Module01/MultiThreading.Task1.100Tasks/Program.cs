/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    public static class Program
    {
        private static void Main()
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1. Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            const int taskAmount = 100;
            const int maxIterationsCount = 1000;

            IterationTasksHandler(taskAmount, maxIterationsCount);
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }

        /// <summary>
        /// Добавил 2 параметра что бы не хардкодить
        /// </summary>
        /// <param name="numberOfTasks"></param>
        /// <param name="numberOfIterationsPerTask"></param>
        private static void IterationTasksHandler(int numberOfTasks, int numberOfIterationsPerTask)
        {
            Task[] taskArray = new Task[numberOfTasks];

            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() => DoIterations(numberOfIterationsPerTask));
            }

            Task.WaitAll(taskArray);
        }

        /// <summary>
        /// Вынес в отдельный метод что бы не засарять <see cref="IterationTasksHandler"/>
        /// </summary>
        /// <param name="numberOfIterations">Сделал входящиц параметр что бы не хардкодить</param>
        private static void DoIterations(int numberOfIterations)
        {
            //Начал итерацию с 1 а не с 0, что бы каждый раз не суммировать
            for (int i = 1; i <= numberOfIterations; i++)
            {
                Console.WriteLine($"Task #{Task.CurrentId} – {i}; ");
            }
        }
    }
}
