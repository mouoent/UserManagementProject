namespace UserManagementProject.Application.Features.Book.DTOs;

public record BookDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
}
