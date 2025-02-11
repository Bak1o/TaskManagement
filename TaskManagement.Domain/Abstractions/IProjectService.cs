using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Commands;

namespace TaskManagement.Domain.Abstractions
{
    public interface IProjectService
    {

        Task<int> ExecuteAsync(RegisterProjectCommand command);
        Task ExecuteAsync(OpenProjectCommand command);
        Task ExecuteAsync(CloseProjectCommand command);
        Task ExecuteAsync(SuspendProjectCommand command);
    }
}
