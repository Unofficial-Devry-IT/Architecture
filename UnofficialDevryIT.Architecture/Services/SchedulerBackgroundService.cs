using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;
using UnofficialDevryIT.Architecture.Models;
using UnofficialDevryIT.Architecture.Scheduler;

namespace UnofficialDevryIT.Architecture.Services
{
    public class SchedulerBackgroundService : BaseBackgroundService, IScheduledTaskService
    {
        #region Events

        public event Action<IScheduledTask> OnScheduledTaskAdded;
        public event Action<IScheduledTask> OnScheduledTaskRemoved;
        public event Action<IScheduledTask> OnScheduledTaskExecuted;
        
        #endregion

        #region Cache

        private readonly List<IScheduledTaskExecutor> _executors = new();
        private readonly List<SchedulerTaskWrapper> _scheduledTasks = new();

        #endregion

        #region Dependency Injection
        
        private readonly IScheduleDbContext _context;
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private readonly IServiceProvider _provider;
        
        #endregion
        
        public SchedulerBackgroundService(ILogger<SchedulerBackgroundService> logger, IServiceProvider provider)
        {
            _logger = logger;
            _context = provider.CreateScope().ServiceProvider.GetRequiredService<IScheduleDbContext>();
            _provider = provider;
        }

        protected override async Task LongRunningTask()
        {
            var referenceTime = DateTime.Now;

            var tasksThatShouldRun = _scheduledTasks.Where(x => x.ShouldRun(referenceTime)).ToList();

            foreach (var task in tasksThatShouldRun)
            {
                // the task should be incremented to the next runnable time based on cron-string
                task.Increment();

                try
                {
                    await ExecuteTask(task.Task);
                }
                catch (Exception)
                {
                    _logger.LogError($"Exception occurred while processing task | {task.Task.Name} | {task.Task.Id}");
                }
            }

            await Task.Delay(1);
        }
        
        protected override async Task InitializeService()
        {
            if (_context == null)
                throw new ArgumentNullException(nameof(_context));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var temp = assembly.ExportedTypes
                    .Where(x => !x.IsInterface && !x.IsAbstract && x == typeof(IScheduledTaskExecutor));

                foreach (var type in temp)
                    _executors.Add((IScheduledTaskExecutor)Activator.CreateInstance(type, _provider));
            }

            foreach (IScheduledTask item in await _context.GetScheduledItems<IScheduledTask>())
            {
                var response = await AddTask(item);
            
                if (response.Succeeded)
                    _logger.LogDebug(response.ToString());
                else
                    _logger.LogError(response.ToString());
            }
        }

        async Task ExecuteTask<T>(T task) where T : IScheduledTask
        {
            foreach (var executor in _executors)
            {
                bool handled = await executor.Execute(task);

                if (handled)
                {
                    _logger.LogDebug($"Task {task.Name} has been executed by {nameof(executor)}");

                    OnScheduledTaskExecuted?.Invoke(task);
                    return;
                }
            }
        }

        public async Task<ResultObject> ManuallyInvoke(ulong id)
        {
            var task = _scheduledTasks.FirstOrDefault(x => x.Task.Id.Equals(id));

            if (task == null)
                return ResultObject.Failure($"Could not locate task with id {id}");

            await ExecuteTask(task.Task);
            
            return ResultObject.Success();
        }

        public Task<ResultObject> AddTask<T>(T task) where T : IScheduledTask
        {
            if (_scheduledTasks.All(x => x.Task.Id != task.Id))
            {
                _scheduledTasks.Add(new SchedulerTaskWrapper()
                {
                    Schedule = CrontabSchedule.Parse(task.Schedule),
                    Task = task,
                    NextRunTime = task.NextRunTime
                });

                OnScheduledTaskAdded?.Invoke(task);
                return Task.FromResult(ResultObject.Success());
            }

            return Task.FromResult(ResultObject.Failure("Task already exists"));
        }

        public Task<ResultObject> RemoveTask<T>(T task) where T : IScheduledTask
        {
            var entry = _scheduledTasks.FirstOrDefault(x => x.Task.Id == task.Id);

            if (entry == null)
                return Task.FromResult(ResultObject.Failure(
                    $"Task {task.Name} | {task.Id} -- does not exist within {nameof(SchedulerBackgroundService)}"));

            _scheduledTasks.Remove(entry);
            OnScheduledTaskRemoved?.Invoke(task);

            return Task.FromResult(ResultObject.Success());
        }

        public Task<ResultObject> UpdateTask<T>(T task) where T : IScheduledTask
        {
            foreach (var item in _scheduledTasks)
            {
                if (!item.Task.Id.Equals(task.Id))
                    continue;

                item.Task = task;
                return Task.FromResult(ResultObject.Success());
            }

            return Task.FromResult(ResultObject.Failure());
        }

        public Task<ResultObject> RemoveTask(ulong id)
        {
            var entry = _scheduledTasks.FirstOrDefault(x => x.Task.Id.Equals(id));

            if (entry == null)
                return Task.FromResult(ResultObject.Failure(
                    $"Task with id {id.ToString()} does not exist within {nameof(SchedulerBackgroundService)}"));

            _scheduledTasks.Remove(entry);
            OnScheduledTaskRemoved?.Invoke(entry.Task);

            return Task.FromResult(ResultObject.Success());
        }
    }
}