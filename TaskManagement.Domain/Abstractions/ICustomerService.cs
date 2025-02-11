using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Commands;

namespace TaskManagement.Domain.Abstractions
{
    public interface ICustomerService
    {
        Task<int> ExecuteAsync(RegisterCustomerCommand command);
        Task ExecuteAsync(OpenCustomerCommand command);
        Task ExecuteAsync(CloseCustomerCommand command);
        Task ExecuteAsync(SuspendCustomerCommand command);

    }
}
