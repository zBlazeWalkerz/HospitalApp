using System.Text.RegularExpressions;
using FluentValidation;
using HospitalApp.Domain.Model;

namespace HospitalApp.Contracts.ClientContracts;

public record CreateClientRequest(
    string PersonalNumber,
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateTime SurgeryDate
);

public static class CreateRequestExtensions
{
    public static Client MapToClient(this CreateClientRequest clientRequest)
    {
        return new Client
        {
            PersonalNumber = clientRequest.PersonalNumber,
            FirstName = clientRequest.FirstName,
            LastName = clientRequest.LastName,
            PhoneNumber = clientRequest.PhoneNumber,
            SurgeryDate = clientRequest.SurgeryDate
        };
    }
}

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(clientRequest => clientRequest.PersonalNumber).NotEmpty().Length(11).WithMessage("Please specify a Personal Number");;
        RuleFor(clientRequest => clientRequest.FirstName).NotEmpty().Length(1, 20).WithMessage("First name must be less than or equal to 20 characters.");
        RuleFor(clientRequest => clientRequest.LastName).NotEmpty().Length(1, 20).WithMessage("Last name must be less than or equal to 20 characters.");
        RuleFor(clientRequest => clientRequest.PhoneNumber).NotEmpty().Must(BeValidPhoneNumber).WithMessage("Please specify a phone number");
        RuleFor(clientRequest => clientRequest.SurgeryDate).NotEmpty().LessThan(DateTime.Now).WithMessage("Please specify a date");
    }

    private bool BeValidPhoneNumber(string phoneNumber)
    {
        const string pattern = @"^5\d{2}\s?\d{3}\s?\d{3}$";

        return Regex.IsMatch(phoneNumber, pattern);
    }
}
