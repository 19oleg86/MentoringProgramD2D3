/*
 * 3. Write a program, which multiplies two matrices and uses class Parallel.
 * a. Implement logic of MatricesMultiplierParallel.cs
 *    Make sure that all the tests within MultiThreading.Task3.MatrixMultiplier.Tests.csproj run successfully.
 * b. Create a test inside MultiThreading.Task3.MatrixMultiplier.Tests.csproj to check which multiplier runs faster.
 *    Find out the size which makes parallel multiplication more effective than the regular one.
 */

using System;
using System.Diagnostics;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("3. Write a program, which multiplies two matrices and uses class Parallel. ");
            Console.WriteLine();

            const byte matrixSize = byte.MaxValue; // todo: use any number you like or enter from console
            CreateAndProcessMatrices(matrixSize);
            Console.ReadLine();
        }

        private static void CreateAndProcessMatrices(byte sizeOfMatrix)
        {
            Console.WriteLine("Multiplying Sequential Loop...");
            var firstMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            var secondMatrix = new Matrix(sizeOfMatrix, sizeOfMatrix, true);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IMatrix resultMatrix = new MatricesMultiplier().Multiply(firstMatrix, secondMatrix);
            stopwatch.Stop();
            
            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();
            Console.WriteLine("resultMatrix:");
            resultMatrix.Print();
            Console.WriteLine($"Sequential Loop execution time: {stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            Console.WriteLine();
            Console.WriteLine("Multiplying Parallel Loop...");
            stopwatch.Start();
            IMatrix resultMatrixParallel = new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);
            stopwatch.Stop();

            Console.WriteLine("firstMatrix:");
            firstMatrix.Print();
            Console.WriteLine("secondMatrix:");
            secondMatrix.Print();
            Console.WriteLine("resultMatrix:");
            resultMatrixParallel.Print();
            Console.WriteLine($"Parallel Loop execution time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
