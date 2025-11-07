using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries;
namespace TaskFlow.API.Controllers{
    [ApiController]    
    [Route("api/[controller]")]    
    public class CommentsController : ControllerBase    
    {
        private readonly IMediator _mediator;        
        public CommentsController(IMediator mediator)        
        {
            _mediator = mediator;        
        }                
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll()        
        {
            var result = await _mediator.Send(new GetAllCommentsQuery());            
            return Ok(result);        
        }                
        [HttpGet("{id}")]        
        public async Task<ActionResult<CommentDto>> GetById(Guid id)        
        {
            var result = await _mediator.Send(new GetCommentByIdQuery { Id = id });            
            if (result == null) return NotFound();            
            return Ok(result);        
        }               
        [HttpGet("task/{taskId}")]        
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetByTaskId(Guid taskId)
        {
            var result = await _mediator.Send(new GetCommentsByTaskIdQuery { TaskId = taskId });            
            return Ok(result);        
        }                 
        [HttpPost]        
        public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentCommand command)
        {
            var result = await _mediator.Send(command);            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);        
        }                
        [HttpPut("{id}")]        
        public async Task<ActionResult<CommentDto>> Update(Guid id, [FromBody] UpdateCommentCommand command)
        {
            if (id != command.Id) return BadRequest();            
            var result = await _mediator.Send(command);            
            if (result == null) return NotFound();            
            return Ok(result);        
        }                
        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(Guid id)        
        {            
            var result = await _mediator.Send(new DeleteCommentCommand { Id = id });            
            if (!result) return NotFound();            
            return NoContent();        
        }    
    }
}
