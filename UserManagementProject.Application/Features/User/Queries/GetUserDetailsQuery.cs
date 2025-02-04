using MediatR;
using System.ComponentModel.DataAnnotations;
using UserManagementProject.Application.Features.User.DTOs;

namespace UserManagementProject.Application.Features.User.Queries;

public record GetUserDetailsQuery : IRequest<UserDetailsDto>
{
    public int Id { get; set; }
}
