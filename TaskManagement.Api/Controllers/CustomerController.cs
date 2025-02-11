using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomeRepository _repository;
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomeRepository repository, ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _repository = repository;
            _customerService = customerService;
            _logger = logger;


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer([FromRoute(Name = "id")]int customerId)
        {
            _logger.LogInformation("searching customer with id : {customerId}", customerId);
           
            var customer = await _repository.GetByIdOrDefaultAsync(customerId);
            if (customer is not null)
            {
                _logger.LogDebug("customer found : {@Customer}", customer);
                return Ok(customer);
            }
            _logger.LogWarning("Customer not found");
            return NotFound();
        }

        

        [HttpGet]
        public async Task<List<Customer>> GetCustomers()
        {
            var users = await _repository.ListAsync();
            return users;

        }

        [HttpGet("AdminUsers")]
        public async Task<List<Customer>> GetAdmins()
        {
            var users = await _repository.ListAsync();
            return users.Where(x => x.Role == Domain.Models.Enums.Role.Admin).ToList();

        }

        [HttpPost]

        public async Task<ActionResult<Customer>> RegisterCustomer([FromBody] RegisterCustomerCommand command)
        {

            var id = await _customerService.ExecuteAsync(command);
            return CreatedAtAction(nameof(GetCustomer), new { id }, new { id });


        }
    }
}
