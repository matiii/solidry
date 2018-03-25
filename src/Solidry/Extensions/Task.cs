using System.Threading.Tasks;

namespace Solidry.Extensions
{
    public static class Task
    {
        /// <summary>
        /// Run task synchronously
        /// </summary>
        /// <param name="task"></param>
        public static void Run(this System.Threading.Tasks.Task task)
        {
            task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get result synchronously
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TResult Result<TResult>(this Task<TResult> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}