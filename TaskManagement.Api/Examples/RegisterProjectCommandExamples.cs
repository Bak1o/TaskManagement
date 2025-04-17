using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Domain.Commands;

namespace TaskManagement.Api.Examples
{
    public sealed class RegisterProjectCommandExamples : IExamplesProvider<RegisterProjectCommand>
    {
        public RegisterProjectCommand GetExamples()
        {
            return new RegisterProjectCommand
            {
                Name = "Project name",
                Description = "Project description",
                CreatedByUserMail = "user@mail.com"

            };
        }
    }
}
