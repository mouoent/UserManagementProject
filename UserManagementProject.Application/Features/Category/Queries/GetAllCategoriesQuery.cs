using MediatR;
using UserManagementProject.Application.Features.Category.DTOs;

namespace UserManagementProject.Application.Features.Category.Queries;

public record GetAllCategoriesQuery : IRequest<List<CategoryDto>>
{
}
