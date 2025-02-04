using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagementProject.Application.Features.Book.Commands.Create;
using UserManagementProject.Application.Features.Book.Queries;

namespace UserManagementProject.API.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/get-all-books")]
    public async Task<IActionResult> GetAllBooks()
    {
        var result = await _mediator.Send(new GetAllBooksQuery());

        return Ok(result);
    }

    [HttpGet("/get-book/{id}")]
    public async Task<IActionResult> GetBookDetails(int id)
    {
        var result = await _mediator.Send(new GetBookDetailsQuery() { Id = id });

        return Ok(result);
    }

    [HttpPost("/create-book")]
    public async Task<IActionResult> CreateBook(CreateBookCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }
}
