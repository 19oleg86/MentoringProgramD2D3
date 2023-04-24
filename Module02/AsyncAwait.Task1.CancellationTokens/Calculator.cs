using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    // todo: change this method to support cancellation token
    public static async Task<long> CalculateAsync(int n, CancellationToken token)
    {
        long sum = 0;

        for (var i = 0; i < n; i++)
        {
            sum += i + 1; // i + 1 is to allow 2147483647 (Max(Int32)) 

            try
            {
                //Waiting for a long process
                await Task.Delay(10, token);
            }
            catch
            {
                //Do not care about exceptions just checking is it cancellation request
                //if it's true throw the OperationCanceledException (stopping our loop)
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"({n}) => Calculation process CANCELED.");
                    token.ThrowIfCancellationRequested();
                }
            }
        }

        return sum;
    }
}
