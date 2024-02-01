using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;

namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class ProjectRepositoryMocks
    {
        internal readonly static List<Project> Projects =
        [
            Project.Create("Project 1", "Description 1", null, null, "GitRepository 1", DateTime.Now.AddDays(1), "FullName 1").Value,
            Project.Create("Project 2", "Description 2", null, null, null, DateTime.Now.AddDays(2), "FullName 2").Value
        ];

        public static IProjectRepository GetProjectRepository()
        {
            var mockProjectRepository = Substitute.For<IProjectRepository>();

            mockProjectRepository.GetAllAsync().Returns(Result<IReadOnlyList<Project>>.Success(Projects));
            mockProjectRepository.FindByIdAsync(Projects[0].ProjectId)
                .Returns(Result<Project>.Success(Projects[0]));
            mockProjectRepository.FindByIdAsync(Projects[1].ProjectId)
                .Returns(Result<Project>.Success(Projects[1]));

            mockProjectRepository.FindByIdAsync(Arg.Is<Guid>(id => id != Projects[0].ProjectId && id != Projects[1].ProjectId))
                .Returns(Result<Project>.Failure("Not found"));

            return mockProjectRepository;
        }
    }
}
