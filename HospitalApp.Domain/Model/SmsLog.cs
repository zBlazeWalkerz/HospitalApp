namespace HospitalApp.Domain.Model;

public class SmsLog
{
    public int Id { get; set; } 
    public int ClientId { get; set; } 
    public string Message { get; set; } 
    public DateTime SentDate { get; set; } = DateTime.UtcNow; 
    public bool IsSuccessful { get; set; } 
    public string DeliveryStatus { get; set; } 
    public Client Client { get; set; } 
}