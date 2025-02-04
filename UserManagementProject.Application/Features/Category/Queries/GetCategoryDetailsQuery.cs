using MediatR;
using UserManagementProject.Application.Features.Category.DTOs;


namespace UserManagementProject.Application.Features.Category.Queries;

public record GetCategoryDetailsQuery : IRequest<CategoryDetailsDto>
{
    public int Id { get; set; }
}
