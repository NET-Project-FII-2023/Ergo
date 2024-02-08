using MediatR;
namespace Ergo.Application.Features.Users.Queries.Search
{
    public class SearchUsersQuery : IRequest<SearchUsersQueryResponse>
    {
        public string SearchValue { get; set; }
    }
}
