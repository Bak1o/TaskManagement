using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Exceptions
{
    public class ObjectNotFoundException : DomainException
    {
        public ObjectNotFoundException(string id, string objectType)
       : base($"{objectType} with id: '{id}' not found")
        {
        }
    }
}
