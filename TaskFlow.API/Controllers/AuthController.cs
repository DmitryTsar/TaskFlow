using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commands;
using TaskFlow.Application.DTOs;
namespace TaskFlow.API.Controllers{
    [ApiController]    
    [Route("api/[controller]")]    
    public class AuthController : ControllerBase    
    {        
        private readonly IMediator _mediator;        
        public AuthController(IMediator mediator)        
        {            
            _mediator = mediator;        
        }        
        [HttpPost("register")]        
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserCommand command)        
        {            
            var result = await _mediator.Send(command);            
            return Ok(result);        
        }        
        [HttpPost("login")]        
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginUserCommand command)        
        {            
            var result = await _mediator.Send(command);            
            return Ok(result);        
        }    
    }
}
