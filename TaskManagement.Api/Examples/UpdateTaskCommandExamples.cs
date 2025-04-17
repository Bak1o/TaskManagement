using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Domain.Commands;

namespace TaskManagement.Api.Examples
{
    public sealed class UpdateTaskCommandExamples : IExamplesProvider<UpdateTaskCommand>
    {
        public UpdateTaskCommand GetExamples()
        {

            return new UpdateTaskCommand
            {
                Id = 1,
                Title = "Task title",
                Description = "Task description",
                Status = Domain.Models.Enums.Status.ToDo,
                Priority = Domain.Models.Enums.Priority.High,
                DeadLine = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))



            };
        }
    }
}
