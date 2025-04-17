using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Identity.Requests;

namespace TaskManagement.Api.Examples
{
    public class ResetPasswordRequestExamples : IExamplesProvider<ResetPasswordRequest>
    {
        public ResetPasswordRequest GetExamples()
        {
            return new ResetPasswordRequest
            {
                Email = "user@mail.com"
            };
        }
    }
}
