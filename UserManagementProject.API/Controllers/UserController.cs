using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagementProject.Application.Features.User.Commands.Create;
using UserManagementProject.Application.Features.User.Commands.Update;
using UserManagementProject.Application.Features.User.Queries;

namespace UserManagementProject.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/get-all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());  

        return Ok(result);
    }

    [HttpGet("/get-user/{id}")]
    public async Task<IActionResult> GetAllUsers(int id)
    {
        var result = await _mediator.Send(new GetUserDetailsQuery() { Id = id });

        return Ok(result);
    }

    [HttpPost("/create-user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPatch("/update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        await _mediator.Send(command);

        return Ok($"User with ID {command.Id} succesfully updated!");
    }
}
