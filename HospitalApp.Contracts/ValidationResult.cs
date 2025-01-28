using System.Runtime.InteropServices.JavaScript;

namespace HospitalApp.Contracts;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
}