using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Queries;
using TaskFlow.Application.Queries.TaskQueries;
namespace TaskFlow.API.Controllers{    
    [ApiController]    
    [Route("api/[controller]")]    
    public class TaskHistoryController : ControllerBase    
    {        
        private readonly IMediator _mediator;        
        public TaskHistoryController(IMediator mediator)        
        {            
            _mediator = mediator;        
        }        
        [HttpGet("{taskId}")]        
        public async Task<IActionResult> GetHistoryByTaskId(Guid taskId)        
        {            
            var result = await _mediator.Send(new GetTaskHistoryByTaskIdQuery(taskId));           
            return Ok(result);        
        }    
    }
}
