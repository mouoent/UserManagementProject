namespace UserManagementProject.Application.Features.Category.DTOs;

public record CategoryDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> BookIds { get; set; }
}
