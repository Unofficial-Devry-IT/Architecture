using System;

namespace UnofficialDevryIT.Architecture.Scheduler
{
    public interface IScheduledTask
    {
        /// <summary>
        /// Display name for this task
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// CRON string for this task
        /// </summary>
        string Schedule { get; set; }
        
        /// <summary>
        /// Unique Identifier for this task
        /// </summary>
        Guid Id { get; set; }
        
        /// <summary>
        /// Time in which this task should be ran next
        /// </summary>
        DateTimeOffset NextRunTime { get; set; }
        
    }
}