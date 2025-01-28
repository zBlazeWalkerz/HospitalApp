namespace HospitalApp.Models
{
    public class ReminderConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int MonthQuantity { get; set; } 
        public bool IsActive { get; set; } = true;
        public ICollection<ReminderSchedule> ReminderSchedules { get; set; }
    }
}
