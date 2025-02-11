using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class DomainTaskController : ControllerBase
    {
        private readonly IDomainTaskRepository _repository;
        private readonly IDomainTaskService _service;
        public DomainTaskController(IDomainTaskRepository repository, IDomainTaskService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<DomainTask>> GetTask([FromRoute(Name ="id")] int taskId)
        {
            var task = await _repository.GetByIdOrDefaultAsync(taskId);
            if (task is not null)
            {
                return Ok(task);
            }
            return NotFound();
        }

        [HttpGet]

        public async Task<ActionResult<List<DomainTask>>> GetTasks()
        {
            var tasks = await _repository.ListAsync();
            return Ok(tasks);
        }

        [HttpPut]

        public async Task<ActionResult<DomainTask>> UpdateTask(UpdateTaskCommand command)
        {
          await _service.UpdateAsync(command);
            return Ok();
        }

    }
}
