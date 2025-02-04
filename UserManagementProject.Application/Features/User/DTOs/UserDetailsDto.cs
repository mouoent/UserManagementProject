namespace UserManagementProject.Application.Features.User.DTOs;

public record UserDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<int> RoleIds { get; set; }
}
