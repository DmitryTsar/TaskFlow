using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands.UserCommand;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.UserQueries;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
namespace TaskFlow.API.Controllers{    
    [ApiController]    
    [Route("api/[controller]")]    
    public class UsersController : ControllerBase{
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;        
        public UsersController(IMediator mediator)
        {            
            _mediator = mediator;
        }        
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);        
        }        
        [HttpGet("{id}")]        
        public async Task<ActionResult<User>> GetById(Guid id)
        {            
            var user = await _mediator.Send(new GetUserByIdQuery(id));            
            if (user == null)                
                return NotFound($"Пользователь с Id={id} не найден.");            
            return Ok(user);        
        }        
        [HttpPost]        
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserCommand command)
        {            
            var created = await _mediator.Send(command);            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }        
        [HttpPut("{id}")]        
        public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UpdateUserCommand command)
        {            
            if (id != command.Id)                
                return BadRequest("ID в URL и теле запроса не совпадают.");            
            var updated = await _mediator.Send(command);            
            if (updated == null)                
                return NotFound($"Пользователь с Id={id} не найден.");            
            return Ok(updated);        
        }        
        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(Guid id)
        {            
            var deleted = await _mediator.Send(new DeleteUserCommand { Id = id });            
            if (!deleted)                
                return NotFound($"Пользователь с Id={id} не найден.");            
            return NoContent();        
        }        
        [HttpGet("{id}/projects")]        
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects(Guid id)
        {            
            var projects = await _mediator.Send(new GetUserProjectsQuery(id));
            return Ok(projects);        
        }        
        [HttpGet("{id}/tasks/created")]        
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetCreatedTasks(Guid id)
        {            
            var tasks = await _mediator.Send(new GetUserCreatedTasksQuery(id));            
            return Ok(tasks);        
        }        
        [HttpGet("{id}/tasks/assigned")]        
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAssignedTasks(Guid id)
        {            
            var tasks = await _mediator.Send(new GetUserAssignedTasksQuery(id));
            return Ok(tasks);        
        }    
    }
}