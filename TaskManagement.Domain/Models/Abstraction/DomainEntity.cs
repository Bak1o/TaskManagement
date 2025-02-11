using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models.Abstraction
{
    public class DomainEntity<TId> where TId : IComparable<TId>
    {
        public TId Id { get; set; } = default!;
    }
}
