using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using Moq;
namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class ProjectRepositoryMocks
    {
        internal readonly static List<Project> Projects =
        [
            Project.Create("Project 1", "Description 1", "GitRepository 1", DateTime.Now.AddDays(1), "FullName 1").Value,
            Project.Create("Project 2", "Description 2", null, DateTime.Now.AddDays(2), "FullName 2").Value
        ];

        public static Mock<IProjectRepository> GetProjectRepository()
        {
            var mockProjectRepository = new Mock<IProjectRepository>();

            mockProjectRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Result<IReadOnlyList<Project>>.Success(Projects));
            mockProjectRepository.Setup(repo => repo.FindByIdAsync(Projects[0].ProjectId))
                .ReturnsAsync(Result<Project>.Success(Projects[0]));
            mockProjectRepository.Setup(repo => repo.FindByIdAsync(Projects[1].ProjectId))
                .ReturnsAsync(Result<Project>.Success(Projects[1]));

            mockProjectRepository.Setup(repo => repo.FindByIdAsync(It.Is<Guid>(id => id != Projects[0].ProjectId && id != Projects[1].ProjectId)))
                .ReturnsAsync(Result<Project>.Failure("Not found"));

            return mockProjectRepository;
        }
    }
}
