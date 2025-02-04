using Moq;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Tests.UnitTests;
public abstract class TestBase
{
    protected readonly Mock<IUserRepository> _userRepositoryMock;
    protected readonly Mock<IRoleRepository> _roleRepositoryMock;
    protected readonly Mock<IBookRepository> _bookRepositoryMock;
    protected readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    protected TestBase()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _bookRepositoryMock = new Mock<IBookRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
    }
}
