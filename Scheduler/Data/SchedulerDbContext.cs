using Microsoft.EntityFrameworkCore;
using Scheduler.Entity;

namespace Scheduler.Data
{
    public class SchedulerDbContext(DbContextOptions<SchedulerDbContext> options) : DbContext(options)
    {
        public DbSet<Schedule> Schedules { get; set; }
    }
}