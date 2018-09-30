using System;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Library.Extensions
{
    public static class Extensions
    {
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationTokenSource taskCTS)
        {
            using (var timeoutCTS = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCTS.Token));
                if (completedTask == task)
                {
                    timeoutCTS.Cancel();
                    return await task;
                }
                else
                {
                    taskCTS.Cancel();
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }
}