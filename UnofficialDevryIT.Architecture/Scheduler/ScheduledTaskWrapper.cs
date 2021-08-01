using System;
using NCrontab;

namespace UnofficialDevryIT.Architecture.Scheduler
{
    public class SchedulerTaskWrapper
    {
        public CrontabSchedule Schedule { get; set; }
        public IScheduledTask Task { get; set; }
        
        /// <summary>
        /// The time in which this task will execute
        /// </summary>
        public DateTimeOffset NextRunTime { get; set; }
        
        /// <summary>
        /// Time in which this task last executed
        /// </summary>
        public DateTimeOffset LastRunTime { get; set; }

        /// <summary>
        /// Increment the next run time to whenever it should run next (if applicable)
        /// </0summary>
        public void Increment()
        {
            LastRunTime = NextRunTime;
            NextRunTime = Schedule.GetNextOccurrence(DateTime.Now);
        }

        /// <summary>
        /// Should this task be executed given the <paramref name="referenceTime"/>
        /// </summary>
        /// <param name="referenceTime"></param>
        /// <returns></returns>
        public bool ShouldRun(DateTime referenceTime) => NextRunTime < referenceTime && LastRunTime != NextRunTime;

        /// <summary>
        /// Will this instance ever run again?
        /// </summary>
        public bool WillNeverRunAgain => LastRunTime == NextRunTime;
    }
}