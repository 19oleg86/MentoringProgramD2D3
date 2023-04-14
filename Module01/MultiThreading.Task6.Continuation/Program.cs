/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    internal static class Program
    {
        private static void Main()
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
            Console.WriteLine();
            ExecuteOnParentFaultResult();
            Console.WriteLine();
            Console.WriteLine();
            ExecuteOnParentFaultWithParentThread();
            Console.WriteLine();
            Console.WriteLine();
            ExecuteOutsideOfThreadPoolWhenParentCancelled();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Finished!");

            Console.ReadLine();
        }

        /// <summary>
        /// Если <see cref="throwEx"/> = null, то исключения вызвано не будет, если false то на 8-й итарации будет вызвано исключение
        /// иначе исключение будет вызвано если был запрос на отмену
        /// </summary>
        private static Task CreateCalcFunc(CancellationToken token, bool throwEx)
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Display squares of numbers:");
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}. Square of {i} is equal to {Math.Pow(i, 2)}");
                    Thread.Sleep(300);

                    token.ThrowIfCancellationRequested();
                    
                    if (throwEx && i == 8)
                    {
                        throw new Exception("Operation is interrupted.");
                    }
                }
            }, token);
        }


        private static void ExecTask(Func<Task, Task> getContinueTask, bool? throwCancel)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

            Task mainTask = CreateCalcFunc(cancelTokenSource.Token, throwCancel != null);
            Task continueTask = getContinueTask(mainTask);

            try
            {
                if (throwCancel == true)
                {
                    Thread.Sleep(1000);
                    cancelTokenSource.Cancel();
                }

                mainTask.Wait(cancelTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine((ex.InnerException ?? ex).Message);
            }
            finally
            {
                cancelTokenSource.Dispose();
            }

            try
            {
                continueTask.Wait();
            }
            catch(AggregateException ex)
            {
                if (!ex.InnerExceptions.Any(_ => _ is TaskCanceledException))
                {
                    throw;
                }
                //Ничего не делаем ContinueTask не выполнился из-за условия
            }

            Console.WriteLine($"Antecedent Task status: {mainTask.Status}");
        }

        #region Task a

        private static void ExecuteRegardlessParentResult()
        {
            Console.WriteLine("Task a.");
            ExecTask(mainTask => mainTask.ContinueWith(antecedent => 
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                    "Continuation Task Completed regardless Antecedent Task execution result."),
                TaskContinuationOptions.None), false);

            Console.WriteLine();

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                                      "Continuation Task Completed regardless Antecedent Task execution result."),
                TaskContinuationOptions.None), null);
        }

        #endregion

        #region Task b

        private static void ExecuteOnParentFaultResult()
        {
            Console.WriteLine("Task b.");

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine(
                        $"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                        "Continuation Task Completed regardless Antecedent Task was executed without success"),
                TaskContinuationOptions.OnlyOnFaulted), false);
            
            Console.WriteLine();

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine(
                        $"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                        "Continuation Task Not Completed"),
                TaskContinuationOptions.OnlyOnFaulted), null);
        }

        #endregion

        #region Task c

        private static void ExecuteOnParentFaultWithParentThread()
        {
            Console.WriteLine("Task c.");

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                                      "Continuation Task executed on Antecedent Task's thread after it's failed "),
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted), false);

            Console.WriteLine();

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                                      "Continuation Task Not Completed"),
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted), null);
        }

        #endregion

        #region Task d

        private static void ExecuteOutsideOfThreadPoolWhenParentCancelled()
        {
            Console.WriteLine("Task d.");

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                                      "Continuation Task Completed outside of ThreadPool when Parent Task was cancelled"),
                TaskContinuationOptions.RunContinuationsAsynchronously | TaskContinuationOptions.OnlyOnCanceled), true);

            Console.WriteLine();

            ExecTask(mainTask => mainTask.ContinueWith(antecedent =>
                    Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}." +
                                      "Continuation Task Not Completed"),
                TaskContinuationOptions.RunContinuationsAsynchronously | TaskContinuationOptions.OnlyOnCanceled), null);
        }

        #endregion
    }
}
