using System.Threading.Tasks;

namespace UnofficialDevryIT.Architecture.Scheduler
{
    public interface IScheduledTaskExecutor
    {
        /// <summary>
        /// Executes a given task
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if handled, otherwise false</returns>
        Task<bool> Execute<T>(T task) where T : IScheduledTask;
    }
}