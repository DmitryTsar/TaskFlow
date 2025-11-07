using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries;
namespace TaskFlow.API.Controllers{    
    [ApiController]    
    [Route("api/[controller]")]    
    public class AttachmentsController : ControllerBase    
    {        private readonly IMediator _mediator;
        public AttachmentsController(IMediator mediator)        
        {            
            _mediator = mediator;        
        }        
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAll()        
        {            
            var result = await _mediator.Send(new GetAllAttachmentsQuery());            
            return Ok(result);        
        }        
        [HttpGet("{id}")]        
        public async Task<ActionResult<AttachmentDto>> GetById(Guid id)        
        {            
            var result = await _mediator.Send(new GetAttachmentByIdQuery { Id = id });            
            if (result == null) return NotFound();            
            return Ok(result);        
        }        
        [HttpGet("task/{taskId}")]        
        public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetByTask(Guid taskId)        
        {            
            var result = await _mediator.Send(new GetAttachmentsByTaskIdQuery { TaskId = taskId });            
            return Ok(result);        
        }        
        [HttpPost]        
        public async Task<ActionResult<AttachmentDto>> Create([FromBody] CreateAttachmentCommand command)        
        {            
            var result = await _mediator.Send(command);            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);        
        }        
        [HttpPut("{id}")]        
        public async Task<ActionResult<AttachmentDto>> Update(Guid id, [FromBody] UpdateAttachmentCommand command)        
        {            
            if (id != command.Id) return BadRequest();            
            var result = await _mediator.Send(command);            
            if (result == null) return NotFound();            
            return Ok(result);        
        }        
        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(Guid id)        
        {            
            var result = await _mediator.Send(new DeleteAttachmentCommand { Id = id });            
            if (!result) return NotFound();            
            return NoContent();        
        }    
    }
}
