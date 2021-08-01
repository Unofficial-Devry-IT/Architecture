using System;
using System.Threading.Tasks;
using UnofficialDevryIT.Architecture.Models;

namespace UnofficialDevryIT.Architecture.Scheduler
{
    /// <summary>
    /// Service contract to process <see cref="IScheduledTask"/>s
    /// </summary>
    public interface IScheduledTaskService
    {
        /// <summary>
        /// Systems within the architecture can subscribe to this event
        /// to be notified when a task gets added
        /// </summary>
        event Action<IScheduledTask> OnScheduledTaskAdded;
        
        /// <summary>
        /// Systems within the architecture can subscribe to this event
        /// to be notified when a task gets removed
        /// </summary>
        event Action<IScheduledTask> OnScheduledTaskRemoved;
        
        /// <summary>
        /// Systems within the architecture can subscribe to this event
        /// to be notified when a task has been invoked
        /// </summary>
        event Action<IScheduledTask> OnScheduledTaskExecuted;

        /// <summary>
        /// Manually invoke a task via <see cref="id"/>
        /// </summary>
        /// <param name="id">Unique ID for task to execute</param>
        /// <returns></returns>
        Task<ResultObject> ManuallyInvoke(string id);
        
        /// <summary>
        ///  Add a task to be handled by this service
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<ResultObject> AddTask<T>(T task) where T : IScheduledTask;
        
        /// <summary>
        /// Using <paramref name="task"/> -- remove it from service
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Anonymous type of bool (success), and string (error message)</returns>
        Task<ResultObject> RemoveTask<T>(T task) where T : IScheduledTask;

        /// <summary>
        /// Update a task that's currently being tracked by this scheduling service
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<ResultObject> UpdateTask<T>(T task) where T : IScheduledTask;
        
        /// <summary>
        /// Remove task from service with matching id
        /// </summary>
        /// <param name="id">Id of task to remove</param>
        /// <returns>Anonymous type of bool (success), and string (error message)</returns>
        Task<ResultObject> RemoveTask(Guid id);
    }
}