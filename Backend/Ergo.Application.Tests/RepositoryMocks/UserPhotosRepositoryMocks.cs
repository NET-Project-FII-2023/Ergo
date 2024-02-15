using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class UserPhotosRepositoryMocks
    {
        internal readonly static List<UserPhoto> UserPhotos =
        [
            UserPhoto.Create("photo", "d1906196-01f7-4335-88b9-89f9672bb4ce").Value,
            UserPhoto.Create("photo", Guid.NewGuid().ToString()).Value
        ];
        public static IUserPhotoRepository GetUserPhotoRepository()
        {
            var mockUserPhotoRepository = Substitute.For<IUserPhotoRepository>();

            mockUserPhotoRepository.GetAllAsync().Returns(Result<IReadOnlyList<UserPhoto>>.Success(UserPhotos));
            mockUserPhotoRepository.FindByIdAsync(UserPhotos[0].UserPhotoId)
                .Returns(Result<UserPhoto>.Success(UserPhotos[0]));
            mockUserPhotoRepository.FindByIdAsync(UserPhotos[1].UserPhotoId)
                .Returns(Result<UserPhoto>.Success(UserPhotos[1]));

            mockUserPhotoRepository.FindByIdAsync(Arg.Is<Guid>(id => id != UserPhotos[0].UserPhotoId && id != UserPhotos[1].UserPhotoId))
                .Returns(Result<UserPhoto>.Failure("Not found"));

            return mockUserPhotoRepository;
        }
    }
}
