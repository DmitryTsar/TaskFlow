using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.TaskCommand;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.TaskQueries;
using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;
namespace TaskFlow.API.Controllers{ 
    [ApiController]    
    [Route("api/[controller]")]    
    public class TasksController : ControllerBase
    {

        private readonly IMediator _mediator; 
        public TasksController(IMediator mediator)
        { 
            _mediator = mediator;
        }

        
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
        {
            var tasks = await _mediator.Send(new GetAllTasksQuery());
            return Ok(tasks);            
            // Здесь можно сделать отдельный запрос GetAllTasksQuery, если нужно            
            //return Ok(await _mediator.Send(new GetTasksByProjectQuery { ProjectId = Guid.Empty }));
        }
            
        
        [HttpGet("{id}")]        
        public async Task<ActionResult<TaskDto>> GetById(Guid id)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery { Id = id });            
            return Ok(task);        
        }        
        
        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskCommand command)
        { 
            var task = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);        
        }        
        
        [HttpPut("{id}")]        
        public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] UpdateTaskCommand command)        
        {
            if (id != command.Id) return BadRequest();            
            var updatedTask = await _mediator.Send(command);
            return Ok(updatedTask);        
        }        
        
        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTaskCommand { Id = id });
            if (!result) return NotFound();            return NoContent();        
        }        
        
        [HttpGet("project/{projectId}")]        
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetByProject(Guid projectId)
        {
            var tasks = await _mediator.Send(new GetTasksByProjectQuery { ProjectId = projectId });            
            return Ok(tasks);        
        }        
        
        [HttpGet("project/{projectId}/status/{status}")]        
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetByStatus(Guid projectId, TaskStatus status)
        {
            var tasks = await _mediator.Send(new GetTasksByStatusQuery            
            {                ProjectId = projectId,                
                Status = status            });            
            return Ok(tasks);        
        }    
    }
}
