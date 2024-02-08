namespace Ergo.Application.Features.Badges.Queries.GetBadgesForUser
{
    public class BadgeDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
    }
}