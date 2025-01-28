using System.ComponentModel.DataAnnotations;

namespace HospitalApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string PersonalNumber { get; set; } = string.Empty;
        public string? EmailAddress { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime SurgeryDate { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public ICollection<ReminderSchedule> ReminderSchedules { get; set; } = new List<ReminderSchedule>();
        public ICollection<SmsLog> SmsLogs { get; set; } = new List<SmsLog>();
    }
}
