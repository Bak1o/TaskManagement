using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Identity.Requests;

namespace TaskManagement.Api.Examples
{
    public class NewPasswordRequestExamples : IExamplesProvider<NewPasswordRequest>
    {
        public NewPasswordRequest GetExamples()
        {
            return new NewPasswordRequest
            {
                Email = "user@mail.com",
                NewPassword = "P@$$w0rd!",
                Otp = "code that was send to Email"
            };
        }
    }
}
