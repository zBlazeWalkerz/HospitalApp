using HospitalApp.Data;
using HospitalApp.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Services
{
    public class ReminderService
    {
        private readonly ApplicationDbContext _context;

        public ReminderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task GenerateRemindersForClientAsync(int clientId, DateTime surgeryDate)
        {
            var reminderConfigs = await _context.ReminderConfigurations
                .Where(rc => rc.IsActive)
                .ToListAsync();

            var reminders = reminderConfigs.Select(rc => new ReminderSchedule
            {
                ClientId = clientId,
                ScheduledDate = surgeryDate.AddMonths(rc.MonthQuantity),
                ReminderConfigId = rc.Id,
                IsSent = false
            });

            _context.ReminderSchedules.AddRange(reminders);
            await _context.SaveChangesAsync();
        }

        public async Task ProcessAndSendRemindersAsync()
        {
            var dueReminders = await _context.ReminderSchedules
                .Where(r => !r.IsSent && r.ScheduledDate <= DateTime.UtcNow)
                .Include(r => r.Client)
                .Include(r => r.ReminderConfiguration)
                .ToListAsync();

            foreach (var reminder in dueReminders)
            {
                // sms send logic
                SendSms(reminder.Client.PhoneNumber,"zd bratishka");
            }

            await _context.SaveChangesAsync();
        }

        private bool SendSms(string phoneNumber, string message)
        {
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");
            return true; 
        }
    }
}
