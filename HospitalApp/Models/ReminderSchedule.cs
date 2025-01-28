namespace HospitalApp.Models
{
    public class ReminderSchedule
    {
        public int Id { get; set; }
        public int ClientId { get; set; } 
        public DateTime ScheduledDate { get; set; } 
        public bool IsSent { get; set; } = false; 
        public int ReminderConfigId { get; set; } 
        public DateTime? LastAttemptDate { get; set; } 
        public Client Client { get; set; } 
        public ReminderConfiguration ReminderConfiguration { get; set; } 
    }

}
