using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Api.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Project>> GetProject([FromRoute(Name ="id")] int projectId)
        {
           var project = await _projectRepository.GetByIdOrDefaultAsync(projectId);
            if (project is not null)
            {
                return Ok(project);
            }
            return NotFound();

        }

        [HttpGet]

        public async Task<ActionResult<List<Project>>> GetProjects()
        {
            var projects = await _projectRepository.ListAsync();
            return Ok(projects);
        }
    }
}
