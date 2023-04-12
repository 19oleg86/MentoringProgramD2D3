/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Random random = new Random();

            Task<int[]> firstTask = new Task<int[]>(() => GenerateArray(random));

            Task<int[]> secondTask = firstTask.ContinueWith(prevTask => MultiplyArray(prevTask.Result, random));

            Task<int[]> thirdTask = secondTask.ContinueWith(prevTask => SortArray(prevTask.Result));

            Task fourthTask = thirdTask.ContinueWith(prevTask => CalculateAverage(prevTask.Result));

            firstTask.Start();
            fourthTask.Wait();

            Console.ReadLine();
        }

        static int[] GenerateArray(Random random)
        {
            var array = new int[10];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(1, 11);
                Console.WriteLine($"Array's {i+1} element: {array[i]}");
            }
            Console.WriteLine();
            return array;
        }

        static int[] MultiplyArray(int[] inputArray, Random random) 
        {
            int randomInteger = random.Next(2, 11);
            Console.WriteLine($"Multiplier is: {randomInteger}");
            for (int i = 0; i < inputArray.Length; i++)
            {
                inputArray[i] *= randomInteger;
                Console.WriteLine($"Multiplied Array's {i + 1} element: {inputArray[i]}");
            }
            Console.WriteLine();
            return inputArray;
        }

        static int[] SortArray(int[] inputArray)
        {
            Array.Sort(inputArray);
            for (int i = 0; i < inputArray.Length; i++)
            {
                Console.WriteLine($"Sorted Array's {i + 1} element: {inputArray[i]}");
            }
            Console.WriteLine();
            return inputArray;
        }

        static void CalculateAverage(int[] inputArray)
        {
            Console.WriteLine($"Average of array's elements is: {inputArray.Average()}");
        }
    }
}
