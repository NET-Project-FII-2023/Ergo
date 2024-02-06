using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class Badge
    {
        private Badge(string name, int count, Guid userId, string type)
        {
            BadgeId = Guid.NewGuid();
            Name = name;
            Count = count;
            UserId = userId;
            Type = type;
        }
        public Guid BadgeId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        private Badge()
        {
            
        }
        public static Result<Badge> Create(string name,int count, Guid userId,string type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<Badge>.Failure(Constants.BadgeNameRequired);
            }
            if (count < 0)
            {
                return Result<Badge>.Failure(Constants.BadgeCountRequired);
            }
            if (userId == Guid.Empty)
            {
                return Result<Badge>.Failure(Constants.UserIdRequired);
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                return Result<Badge>.Failure(Constants.BadgeTypeRequired);
            }
            return Result<Badge>.Success(new Badge(name, count, userId,type));
        }
        public Result<Badge> UpdateCount(int count)
        {
            if (count < 0)
            {
                return Result<Badge>.Failure(Constants.BadgeCountRequired);
            }
            Count = count;
            return Result<Badge>.Success(this);
        }



    }
}
