﻿using FluentValidation;
using HospitalApp.Contracts.ClientContracts;
using HospitalApp.Domain.Model;
using HospitalApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientService _clientService;
        private readonly ReminderService _reminderService;
        private readonly IValidator<CreateClientRequest> _validator;

        public ClientController(ClientService clientService, ReminderService reminderService, IValidator<CreateClientRequest> validator)
        {
            _clientService = clientService;
            _reminderService = reminderService;
            _validator = validator;
        }



        // GET: Client/List
        public async Task<IActionResult> List()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return View(clients);
        }

        // GET: Client/Create
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientRequest request)
        {
            var validationResult = _validator.Validate(request);
            
            if (!validationResult.IsValid)
                return View(request);
            
            var client = request.MapToClient();
            await _clientService.AddClientAsync(client);
            
            return RedirectToAction(nameof(List));
        }

        // GET: Client/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var updated = await _clientService.UpdateClientAsync(client);
                if (updated)
                {
                    return RedirectToAction(nameof(List));
                }
                ModelState.AddModelError("", "Unable to update client. Please try again.");
            }
            return View(client);
        }

        // GET: Client/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction(nameof(List));
        }
    }
}

