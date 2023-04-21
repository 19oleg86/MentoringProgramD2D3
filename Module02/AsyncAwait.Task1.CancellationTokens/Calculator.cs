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
            if (!token.IsCancellationRequested)
            {
                sum += (i + 1); // i + 1 is to allow 2147483647 (Max(Int32)) 
                await Task.Delay(10, token);
            }
            else
            {
                token.ThrowIfCancellationRequested();
            }
        }

        return sum;
    }
}
