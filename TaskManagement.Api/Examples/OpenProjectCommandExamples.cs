using Swashbuckle.AspNetCore.Filters;
using TaskManagement.Domain.Commands;

namespace TaskManagement.Api.Examples
{
    public sealed class OpenProjectCommandExamples : IExamplesProvider<OpenProjectCommand>
    {
        public OpenProjectCommand GetExamples()
        {
            return new OpenProjectCommand
            {
                Id = 1,
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            };
        }
    }
}
