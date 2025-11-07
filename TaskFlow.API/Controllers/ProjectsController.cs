using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.ProjectQueries;
using TaskFlow.Domain.Interfaces;
namespace TaskFlow.API.Controllers{    
    [ApiController]    
    [Route("api/[controller]")]    
    public class ProjectsController : ControllerBase    
    {        
        private readonly IMediator _mediator;        
        private readonly IProjectRepository _projectRepository;        
        public ProjectsController(IMediator mediator, 
            IProjectRepository projectRepository)        
        {            
            _mediator = mediator;            
            _projectRepository = projectRepository;        
        }        
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()        
        {            
            var projects = await _mediator.Send(new GetAllProjectsQuery());            
            return Ok(projects);        
        }        
        [HttpGet("{id}")]        
        public async Task<ActionResult<ProjectWithTasksDto>> GetById(Guid id)        
        {            
            var project = await _mediator.Send(new GetProjectByIdQuery(id));            
            if (project == null) return NotFound();            
            return Ok(project);        
        }        
        [HttpPost]        
        public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectCommand command)        
        {            
            var result = await _mediator.Send(command);            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);        
        }        
        [HttpPut("{id}")]        
        public async Task<ActionResult<ProjectDto>> Update(Guid id, [FromBody] UpdateProjectCommand command)        
        {            
            if (id != command.Id) return BadRequest();            
            var result = await _mediator.Send(command);            
            return Ok(result);        
        }        
        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(Guid id)        
        {            
            var result = await _mediator.Send(new DeleteProjectCommand { Id = id });            
            if (!result) return NotFound();            
            return NoContent();        
        }        
        [HttpGet("{id}/tasks")]        
        public async Task<ActionResult<IEnumerable<TaskForProjectDto>>> GetTasks(Guid id)        
        {            
            var tasks = await _mediator.Send(new GetProjectTasksCommand { ProjectId = id });            
            return Ok(tasks);        
        }        
        [HttpGet("{id}/tasks/status/{status}")]        
        public async Task<ActionResult<IEnumerable<TaskForProjectDto>>> GetTasksByStatus(Guid id, TaskFlow.Domain.Enums.TaskStatus status)        
        {            
            var tasks = await _mediator.Send(new GetProjectTasksByStatusCommand            
            {                
                ProjectId = id,                
                Status = (Domain.Enums.TaskStatus)status            
            });            
            return Ok(tasks);        
        }    
    }
}
