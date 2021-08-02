using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnofficialDevryIT.Architecture.Scheduler
{
    /// <summary>
    /// Contractual obligation for a service/database to follow so <see cref="Services.SchedulerBackgroundService"/> works
    /// </summary>
    public interface IScheduleDbContext
    {
        /// <summary>
        /// Add a scheduled task to the database
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="TSchedule"></typeparam>
        /// <returns></returns>
        Task<TSchedule> AddSchedule<TSchedule>(TSchedule task) where TSchedule : class, IScheduledTask;

        /// <summary>
        /// Delete schedule from database
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="TSchedule"></typeparam>
        /// <returns></returns>
        Task<TSchedule> DeleteSchedule<TSchedule>(TSchedule task) where TSchedule : class, IScheduledTask;
        
        /// <summary>
        /// Get all scheduled items of a specific type from the database
        /// </summary>
        /// <typeparam name="TSchedule"></typeparam>
        /// <returns></returns>
        Task<IList<TSchedule>> GetScheduledItems<TSchedule>() where TSchedule : class, IScheduledTask;
        
        /// <summary>
        /// Get a list of scheduled tasks that are within the specified date range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <typeparam name="TSchedule"></typeparam>
        /// <returns></returns>
        Task<IList<TSchedule>> GetScheduledTimesInRange<TSchedule>(DateTime from, DateTime to) where TSchedule : class, IScheduledTask;
        
        /// <summary>
        /// Increment the <see cref="IScheduledTask"/> to the next iteration
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task UpdateSchedule<T>(T task) where T : class, IScheduledTask;
    }
}