using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Identity.Requests;

namespace TaskManagement.Api.Examples
{
    public sealed class RegisterRequestExamples : IExamplesProvider<RegisterRequest>
    {
        public RegisterRequest GetExamples()
        {
            return new RegisterRequest
            {
                FirstName = "user firstname",
                LastName = "user last name",
                Email = "user@mail.com",
                Password = "P@$$w0rd",
                Role = "Admin,Manager,Member"
            };
        }
    }
}
