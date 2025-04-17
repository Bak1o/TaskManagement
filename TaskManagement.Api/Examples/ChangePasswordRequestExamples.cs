using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Identity.Requests;

namespace TaskManagement.Api.Examples
{
    public class ChangePasswordRequestExamples : IExamplesProvider<ChangePasswordRequest>
    {
        public ChangePasswordRequest GetExamples()
        {
            return new ChangePasswordRequest
            {
                Email = "user@mail.com",
                CurrentPassword = "P@$$w0rd",
                NewPassword = "P@$$w0rd!"
            };
        }
    }
}
