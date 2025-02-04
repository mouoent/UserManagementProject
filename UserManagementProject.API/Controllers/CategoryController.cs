using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManagementProject.Application.Features.Book.Queries;
using UserManagementProject.Application.Features.Category.Commands;
using UserManagementProject.Application.Features.Category.Commands.Create;
using UserManagementProject.Application.Features.Category.Commands.Delete;
using UserManagementProject.Application.Features.Category.Queries;

namespace UserManagementProject.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/get-all-categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());

        return Ok(result);
    }

    [HttpGet("/get-category/{id}")]
    public async Task<IActionResult> GetCategoryDetails(int id)
    {
        var result = await _mediator.Send(new GetCategoryDetailsQuery() { Id = id });

        return Ok(result);
    }

    [HttpPost("/create-category")]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("delete-category/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand() { Id = id });

        return Ok($"Category with ID {id} succesfully deleted!");
    }
}
