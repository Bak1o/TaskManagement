using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Api.Examples
{
    public sealed class OpenTaskCommandExamples : IExamplesProvider<OpenTaskCommand>
    {
        public OpenTaskCommand GetExamples()
        {
            return new OpenTaskCommand
            {
                Title = "TestTitle",
                Description = "TestDescription",
                ProjectId = 1,
                AssignedUsersEmails = new List<string>
                {
                    "user1@mail.com", "user2@mail.com"

                },
                Priority = Priority.High,
                DeadLine = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                CreatedByUserMail = "creator@mail.com"
                
            };
        }
    }
}
