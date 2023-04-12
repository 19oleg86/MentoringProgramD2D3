/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            ExecuteRegardlessParentResult();
            Console.WriteLine();
            ExecuteOnParentFaultResult();
            Console.WriteLine();
            ExecuteOnParentFaultWithParentThread();
            Console.WriteLine();
            ExecuteOutsideOfThreadPoolWhenParentCancelled();

            Console.ReadLine();
        }

        #region Task a
        static void ExecuteRegardlessParentResult()
        {
            Console.WriteLine("Task a.");
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            Task task = new Task(() =>
            {
                Console.WriteLine("Display squares of numbers:");
                for (int i = 1; i <= 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    Console.WriteLine($"Square of {i} is equal to {i * i}");
                    Thread.Sleep(300);
                }
            }, token);
            try
            {
                task.Start();
                Thread.Sleep(1000);
                cancelTokenSource.Cancel();
                task.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("Operation is interrupted");
                    else
                        Console.WriteLine(e.Message);
                }
            }
            finally
            {
                cancelTokenSource.Dispose();
            }

            task.ContinueWith(antecedent => Console.WriteLine($"Continuation Task Completed regardless Antecedent Task execution result"), TaskContinuationOptions.None);

            Thread.Sleep(1000);
            Console.WriteLine($"Antecedent Task status: {task.Status}");
            cancelTokenSource.Dispose();
        }
        #endregion

        #region Task b
        static void ExecuteOnParentFaultResult()
        {
            Console.WriteLine("Task b.");
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            Task task = new Task(() =>
            {
                Console.WriteLine("Display squares of numbers:");
                for (int i = 1; i <= 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new Exception("Antecedent task failed.");
                    }
                    Console.WriteLine($"Square of {i} is equal to {i * i}");
                    Thread.Sleep(300);
                }
            }, token);
            try
            {
                task.Start();
                Thread.Sleep(1000);
                cancelTokenSource.Cancel();
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cancelTokenSource.Dispose();
            }

            task.ContinueWith(antecedent => Console.WriteLine($"Continuation Task Completed regardless Antecedent Task was executed without success"), TaskContinuationOptions.OnlyOnFaulted);

            Thread.Sleep(1000);
            Console.Write($"Antecedent Task status: {task.Status}");
            cancelTokenSource.Dispose();
        }
        #endregion

        #region Task c
        static void ExecuteOnParentFaultWithParentThread()
        {
            Console.WriteLine("Task c.");
            Task parentTask = new Task(() =>
            {
                Console.WriteLine();
                Console.WriteLine("Some work in Parent thread");
                throw new Exception("Parent task failed");
            });
            try
            {
                parentTask.Start();
                Task continuationTask = parentTask.ContinueWith(antecedentTask =>
                {
                    Console.WriteLine("Continuation Task executed on Antecedent Task's thread after it's failed ");
                    Console.WriteLine($"Antecedent Task status: {parentTask.Status}");
                }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

                continuationTask.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Task d
        static void ExecuteOutsideOfThreadPoolWhenParentCancelled()
        {
            Console.WriteLine("Task d.");
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            Task task = new Task(() =>
            {
                Console.WriteLine("Display squares of numbers:");
                for (int i = 1; i <= 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    Console.WriteLine($"Square of {i} is equal to {i * i}");
                    Thread.Sleep(300);
                }
            }, token);
            try
            {
                task.Start();
                Thread.Sleep(1000);
                cancelTokenSource.Cancel();
                task.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("Operation is interrupted");
                    else
                        Console.WriteLine(e.Message);
                }
            }
            finally
            {
                cancelTokenSource.Dispose();
            }

            task.ContinueWith(antecedent => Console.WriteLine($"Continuation Task Completed outside of ThreadPool when Parent Task was cancelled"), 
                TaskContinuationOptions.RunContinuationsAsynchronously | TaskContinuationOptions.OnlyOnCanceled);

            Thread.Sleep(1000);
            Console.WriteLine($"Antecedent Task status: {task.Status}");
            cancelTokenSource.Dispose();
        }
        #endregion
    }
}
