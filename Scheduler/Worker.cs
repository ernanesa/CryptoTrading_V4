using NCrontab;
using Scheduler.Data;
using Scheduler.Entity;

namespace Scheduler
{
    public class Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly HttpClient _httpClient = new();
        private const int ONE_SECOND = 1000;
        private CrontabSchedule _schedule;
        private bool _firstRun = true;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentDateTime = DateTime.Now;
            var schedules = new List<Schedule>();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_firstRun || currentDateTime.Second == 0)
                {
                    await UpdateSchedulesAsync(schedules, stoppingToken);
                    if (schedules.Count == 0) continue;
                    await ProcessSchedulesAsync(schedules, currentDateTime, stoppingToken);
                    _firstRun = false;
                }
                await Task.Delay(ONE_SECOND, stoppingToken);
                currentDateTime = DateTime.Now;
            }
        }

        private async Task UpdateSchedulesAsync(List<Schedule> schedules, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();
                var activeSchedules = dbContext.Schedules.Where(x => x.IsActive).ToList();

                foreach (var activeSchedule in activeSchedules)
                {
                    var existingSchedule = schedules.FirstOrDefault(x => x.Id == activeSchedule.Id);
                    if (existingSchedule != null)
                    {
                        existingSchedule.Cron = activeSchedule.Cron;
                        existingSchedule.Route = activeSchedule.Route;
                        existingSchedule.IsActive = activeSchedule.IsActive;
                    }
                    else
                    {
                        schedules.Add(activeSchedule);
                    }
                }

                schedules.RemoveAll(s => activeSchedules.All(x => x.Id != s.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating schedules");
            }
        }

        private async Task ProcessSchedulesAsync(List<Schedule> schedules, DateTime currentDateTime, CancellationToken stoppingToken)
        {
            foreach (var schedule in schedules.ToList())
            {
                _schedule = CrontabSchedule.Parse(schedule.Cron);

                if (currentDateTime.ToString("dd/MM/yyyy HH:mm") == schedule.NextOccurrence.ToString("dd/MM/yyyy HH:mm"))
                {
                    try
                    {
                        _logger.LogInformation($"Triggering endpoint: {schedule.Route}");
                        await _httpClient.GetAsync(schedule.Route, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error triggering endpoint");
                    }
                }
                schedule.AddNextOccurrence(_schedule.GetNextOccurrence(DateTime.Now));
            }
        }
    }
}
