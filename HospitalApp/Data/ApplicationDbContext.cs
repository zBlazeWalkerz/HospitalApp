using HospitalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ReminderConfiguration> ReminderConfigurations { get; set; }
        public DbSet<ReminderSchedule> ReminderSchedules { get; set; }
        public DbSet<SmsLog> SmsLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SmsLog Relationship
            modelBuilder.Entity<SmsLog>()
                .HasOne(sms => sms.Client)
                .WithMany(client => client.SmsLogs)
                .HasForeignKey(sms => sms.ClientId);

            // ReminderSchedule Relationships
            modelBuilder.Entity<ReminderSchedule>()
                .HasOne(rs => rs.Client)
                .WithMany(client => client.ReminderSchedules)
                .HasForeignKey(rs => rs.ClientId);

            modelBuilder.Entity<ReminderSchedule>()
                .HasOne(rs => rs.ReminderConfiguration)
                .WithMany(rc => rc.ReminderSchedules)
                .HasForeignKey(rs => rs.ReminderConfigId);

            // Seed ReminderConfiguration
            modelBuilder.Entity<ReminderConfiguration>().HasData(
                new ReminderConfiguration { Id = 1, Name = "1-Month Reminder", MonthQuantity = 1 },
                new ReminderConfiguration { Id = 2, Name = "6-Month Reminder", MonthQuantity = 6 },
                new ReminderConfiguration { Id = 3, Name = "1-Year Reminder", MonthQuantity = 12 }
            );
        }
    }
}
