using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.FileRepository.Abstractions
{
    public interface ISequenceProvider
    {
        Task<int> GetNextInteger(string sequenceName);

        Task<long> GetNextBigInteger(string sequenceName);
    }
}
