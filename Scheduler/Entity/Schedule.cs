using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Entity
{
    public class Schedule
    {
        public int Id { get; set; }
        public string? Cron { get; set; }
        public string? Route { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public DateTime NextOccurrence { get; set; }
        public void AddNextOccurrence(DateTime nextOccurrence)
        {
            NextOccurrence = nextOccurrence;
        }
    }
}