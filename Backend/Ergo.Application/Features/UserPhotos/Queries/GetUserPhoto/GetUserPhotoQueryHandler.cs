using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.UserPhotos.Queries.GetUserPhoto
{
    public class GetUserPhotoQueryHandler : IRequestHandler<GetUserPhotoQuery, GetUserPhotoQueryResponse>
    {
        private readonly IUserPhotoRepository userPhotoRepository;
        public GetUserPhotoQueryHandler(IUserPhotoRepository userPhotoRepository)
        {
            this.userPhotoRepository = userPhotoRepository;
        }
        public async Task<GetUserPhotoQueryResponse> Handle(GetUserPhotoQuery request, CancellationToken cancellationToken)
        {
            var userPhoto = await userPhotoRepository.GetUserPhotoByUserIdAsync(request.UserId);
            if (!userPhoto.IsSuccess)
            {
                return new GetUserPhotoQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { userPhoto.Error }
                };
            }
            return new GetUserPhotoQueryResponse
            {
                Success = true,
                UserPhotoUrl = userPhoto.Value
            };

        }

    }
}
