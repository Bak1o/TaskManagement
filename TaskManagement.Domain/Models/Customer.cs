using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Abstraction;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Domain.Models
{
    public class Customer : DomainEntity<int>
    {
        //public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Role? Role { get; private set; }
        public UserStatus Status { get; private set; }

        //public int? TaskId { get; set; }
        //public DomainTask? Task { get; set; }

        public Project? Project { get; set; }
        public int? ProjectId { get; set; }

        public List<DomainTask> CreatedTasks { get; set; } = new();
        public List<DomainTask> AssignedTasks { get; set; } = new();
        public List<Project> CreatedProjects { get; set; } = new(); 



        public Customer(string userName, string email,
            string password, string firstName, string lastName)
        {
            UserName = userName;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;

            Status = UserStatus.Inactive;

        }

        public void open(Role role)
        {
            Status = UserStatus.Active;
            Role = role;
        }
        public void close()
        {
            Status = UserStatus.Closed;
        }
        public void suspend()
        {
            Status = UserStatus.Suspended;
        }




    }

}
