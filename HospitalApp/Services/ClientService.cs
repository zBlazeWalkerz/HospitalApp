using HospitalApp.Data;
using HospitalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalApp.Services
{
    public class ClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ReminderService _reminderService;

        public ClientService(ApplicationDbContext context, ReminderService reminderService)
        {
            _context = context;
            _reminderService = reminderService;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task AddClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            await _reminderService.GenerateRemindersForClientAsync(client.Id, client.SurgeryDate);
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            var existingClient = await _context.Clients.FindAsync(client.Id);
            if (existingClient == null) return false;

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.SurgeryDate = client.SurgeryDate;
            existingClient.PhoneNumber = client.PhoneNumber;
            existingClient.PersonalNumber = client.PersonalNumber;

            _context.Clients.Update(existingClient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}
