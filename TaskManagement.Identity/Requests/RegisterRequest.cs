using System.Text.RegularExpressions;
using TaskManagement.Identity.Exceptions;

namespace TaskManagement.Identity.Requests;

public sealed class RegisterRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }

    public void Validate()
    {
        if (FirstName.Length is < 1 or > 100)
            throw new UserValidationException("First name must contain minimum 1 symbol and maximum 100 symbol");
        if (LastName.Length is < 1 or > 100)
            throw new UserValidationException("Last name must contain minimum 1 symbol and maximum 100 symbol");
       
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(emailPattern);
       
        if ((!regex.IsMatch(Email)))
            throw new UserValidationException("Enter correct Email format");
        if (Password.Length is < 1 or > 100)
            throw new UserValidationException("Password must contain minimum 1 symbol and maximum 100 symbol");
        if (Role.Length is < 1 or > 100)
            throw new UserValidationException("Role must contain minimum 1 symbol and maximum 100 symbol");
    }
}