using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Identity.Requests;

namespace TaskManagement.Api.Examples
{
    public class ConfirmEmailRequestExamples : IExamplesProvider<ConfirmEmailRequest>
    {
        public ConfirmEmailRequest GetExamples()
        {
            return new ConfirmEmailRequest
            {
                Email = "user@mail.com",
                Otp = "code that was send to Email"
            };
        }
    }
}
