using Hangfire;
using HospitalApp.Data;
using HospitalApp.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using HospitalApp.Contracts.ClientContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure FluentValidation to scan the assembly for validators
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateClientRequestValidator>());

// Add DbContext for SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services for dependency injection
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<ReminderService>();

// Add Hangfire services
builder.Services.AddHangfire(configuration => 
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseColouredConsoleLogProvider()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Hangfire server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Add Hangfire Dashboard (for monitoring jobs)
app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=List}/{id?}");

// Register the recurring job for reminders
RecurringJob.AddOrUpdate<ReminderService>(
    "process-reminders",
    service => service.ProcessAndSendRemindersAsync(),
    "0 9-18 * * *" // This cron expression runs the job every day from 9 AM to 6 PM
);

app.Run();