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
            if (type == "Innovator" || type == "Quality-Master" || type == "Problem-Solver" || type == "Team-Player")
            {
                Active = true;
            }
            else
            {
                Active = false;

            }
        }
        public Guid BadgeId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public Badge()
        {

        }
        public static Result<Badge> Create(string name, int count, Guid userId, string type)
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

            return Result<Badge>.Success(new Badge(name, count, userId, type));
        }
        public Result<Badge> UpdateCount(int count)
        {
            if (count < 0)
            {
                return Result<Badge>.Failure(Constants.BadgeCountRequired);
            }
            Count = count;
            switch (Type)
            {
                case "TasksDone":
                    if (count >= 100)
                    {
                        Active = true;
                    }
                    break;
                case "CommentsMade":
                    if (count >= 50)
                    {
                        Active = true;
                    }
                    break;
                case "ProjectsMade":
                    if (count >= 10)
                    {
                        Active = true;
                    }
                    break;
                case "HoursWorked":
                    if (count >= 100)
                    {
                        Active = true;
                    }
                    break;
            }

            return Result<Badge>.Success(this);
        }



    }
}
