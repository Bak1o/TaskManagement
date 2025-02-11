using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Service.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomeRepository _userRepository;


        public CustomerService(ICustomeRepository userRepository)
        {
            _userRepository = userRepository;

            #region commented code
            //_inMemoryDb = inMemoryDb ?? throw new ArgumentNullException(nameof(inMemoryDb));
            //var user1 = new User
            //{
            //    Email = "dgF4@gmail.com",
            //    Password = "password",
            //    Role = Role.Admin,
            //    UserName = "Bako"

            //};
            //CreateUserAsync(user1);

            //var user2 = new User
            //{
            //    Email = "dgFgg4@gmail.com",
            //    Password = "password",
            //    Role = Role.TeamMember,
            //    UserName = "Niko"

            //};
            //CreateUserAsync(user2);


            //var user3 = new User
            //{
            //    Email = "dhFyuyg4@gmail.com",
            //    Password = "password",
            //    Role = Role.TeamMember,
            //    UserName = "Nino"

            //};
            //CreateUserAsync(user3);


            //var user4 = new User
            //{
            //    Email = "dsssgF4@gmail.com",
            //    Password = "password",
            //    Role = Role.Admin,
            //    UserName = "Natia"

            //};
            //CreateUserAsync(user4);


            //var user5 = new User
            //{
            //    Email = "dsNyuussgF4@gmail.com",
            //    Password = "password",
            //    Role = Role.TeamMember,
            //    UserName = "Gela"

            //};
            //CreateUserAsync(user5);


            //var user6 = new User
            //{
            //    Email = "dssSweyttsgF4@gmail.com",
            //    Password = "password",
            //    Role = Role.Admin,
            //    UserName = "Ana"

            //};
            //CreateUserAsync(user6);
            #endregion

        }

       

        public async Task<int> ExecuteAsync(RegisterCustomerCommand command)
        {
            command.Validate();
            var customer = new Customer(
                command.UserName,
                command.Email,
                command.Password,
                command.FirstName,
                command.LastName
            );

            await _userRepository.CreateAsync(customer);
            return customer.Id; // Assuming Id is set after creation
        }
        public async Task ExecuteAsync(OpenCustomerCommand command)
        {
            command.Validate();
            var customer = await _userRepository.GetByIdAsync(command.Id);
           
            customer.open(command.Role);
            await _userRepository.UpdateAsync(customer);
        }

        public async Task ExecuteAsync(CloseCustomerCommand command)
        {
            command.Validate();
            var userExists = await _userRepository.GetByIdOrDefaultAsync(command.Id);
           

            userExists.close();

            await _userRepository.UpdateAsync(userExists);
        }

        public async Task ExecuteAsync(SuspendCustomerCommand command)
        {
            command.Validate();
            var userExists = await _userRepository.GetByIdOrDefaultAsync(command.Id);
           
            userExists.suspend();
            await _userRepository.UpdateAsync(userExists);
        }

        #region commented creating user validation
        //public bool ValidateCreateUser(User user)
        //{
        //    // Check if the email already exists



        //    if (_inMemoryDb.Users.Count > 0)

        //    {
        //        if (_inMemoryDb.Users.Any(u => u.Email.Equals(user.Email)))
        //        {
        //            throw new OwnValidationException("Email already exists.");
        //        }

        //        if (_inMemoryDb.Users.Any(i => i.Id == user.Id))
        //        {
        //            throw new OwnValidationException(" Id already exists. ");
        //        }
        //    }





        //    if (user.UserName.Length < 1 || user.UserName.Length > 100)
        //        throw new OwnValidationException(" User name must contain minimum on1 symbol and maximum 100 symbol ");

        //    string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        //    Regex regex = new Regex(emailPattern);

        //    if (!regex.IsMatch(user.Email))
        //        throw new OwnValidationException(" enter correct email format ");


        //    if (string.IsNullOrWhiteSpace(user.Email))
        //        throw new ValidationException("Email must not be empty");


        //    if (user.Password.Length < 8 || user.Password.Length > 16)
        //        throw new OwnValidationException(" password must contain minimum 8 symbols and maximum 16 symbols ");


        //    return true;
        //}
        #endregion

        //public Task CreateAsync(User userToCreate)
        //{
        //    if (ValidateCreateUser(userToCreate))
        //    {
        //        if (userToCreate.Id == 0)
        //        {
        //            userToCreate.Id = _inMemoryDb.Users.Count > 0 ? _inMemoryDb.Users.Max(u => u.Id) + 1 : 1;
        //        }
        //         _inMemoryDb.Users.Add(userToCreate);

        //    }
        //    return Task.CompletedTask;


        //}
        //public Task<List<User>> ListAsync()
        //{
        //     return Task.FromResult(_inMemoryDb.Users);
        //}


        //public async Task<User?> GetByIdAsync(int id)
        //{
        //   var user =  await GetByIdOrDefaultAsync(id);
        //    if (user == null)
        //    {
        //        throw new OwnValidationException($" user with id : {id} not found ");
        //    }
        //    return user;

        //}

        //public Task<User> GetByIdOrDefaultAsync(int id)
        //{

        //    return Task.FromResult(_inMemoryDb.Users.FirstOrDefault(u => u.Id == id)!);

        //}

        //public async Task<User?> GetUserAsync(string username)
        //{
        //    var user = await GetByNameOrDefaultAsync(username);
        //    if (user == null)
        //    {
        //        throw new OwnValidationException($"{username} not found.");
        //    }
        //    return user;
        //}
        //public Task<User?> GetByNameOrDefaultAsync(string userName)
        //{
        //    var user = _inMemoryDb.Users.FirstOrDefault(user => user.UserName == userName);
        //    return Task.FromResult(user);
        //}

        //public async Task UpdateAsync(UpdateUser updateUser)
        //{
        //    var requestedUserExist = await GetByIdOrDefaultAsync(updateUser.Id);
        //    if (requestedUserExist == null)
        //    {
        //        throw new OwnValidationException($" user with id = {updateUser.Id} doesn't exists");
        //    }

        //    updateUser.Validate();
        //        UserTransform.TransformFromModelToRepositoryModel(updateUser,requestedUserExist);



        //}
        //public async Task DeleteAsync(int id)
        //{
        //    var userToDelete = await GetByIdOrDefaultAsync(id);
        //    if (userToDelete == null)
        //    {
        //        throw new OwnValidationException($" user with Id = {id} doesn't exists");
        //    }

        //    _inMemoryDb.Users.Remove(userToDelete);

        //}
        //public Task SaveAsync(User user)
        //{
        //    return Task.CompletedTask;
        //}


    }
}
