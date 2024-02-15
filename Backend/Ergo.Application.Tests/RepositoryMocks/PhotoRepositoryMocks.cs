using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;

namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class PhotoRepositoryMocks
    {
        internal readonly static List<Photo> Photos =
            [
                Photo.Create(Guid.Parse("00000000-0000-0000-0000-000000000000"), "token").Value,
                Photo.Create(Guid.Parse("00000000-0000-0000-0000-000000000000"), "token").Value,
            ];
        public static IPhotoRepository GetPhotoRepository()
        {
            var mockPhotoRepository = Substitute.For<IPhotoRepository>();
            mockPhotoRepository.GetAllAsync().Returns(Result<IReadOnlyList<Photo>>.Success(Photos));
            mockPhotoRepository.FindByIdAsync(Photos[0].PhotoId).Returns(Result<Photo>.Success(Photos[0]));
            mockPhotoRepository.FindByIdAsync(Photos[1].PhotoId).Returns(Result<Photo>.Success(Photos[1]));
            mockPhotoRepository.FindByIdAsync(Arg.Is<Guid>(id => id != Photos[0].PhotoId && id != Photos[1].PhotoId)).Returns(Result<Photo>.Failure("Not found"));
            return mockPhotoRepository;


        }
    }
}
