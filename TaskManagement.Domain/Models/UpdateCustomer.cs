using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Abstraction;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Domain.Models
{
    public class UpdateCustomer : DomainEntity<int>
    {

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Role Role { get; set; }

        public void Validate()
        {
            if (UserName.Length < 1 || UserName.Length > 100)
                throw new Exceptions.ValidationException(" User name must contain minimum on1 symbol and maximum 100 symbol ");

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);

            if (!regex.IsMatch(Email))
                throw new Exceptions.ValidationException(" enter correct email format ");


            if (string.IsNullOrWhiteSpace(Email))
                throw new System.ComponentModel.DataAnnotations.ValidationException("Email must not be empty");


            if (Password.Length < 8 || Password.Length > 16)
                throw new Exceptions.ValidationException(" password must contain minimum 8 symbols and maximum 16 symbols ");





        }
    }
}
