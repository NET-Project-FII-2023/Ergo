using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class UserVotedBadges
    {
        private UserVotedBadges(Guid voterId,Guid votedId, string type)
        {
            UserVotedBadgesId = Guid.NewGuid();
            VoterId = voterId;
            VotedId = votedId;
            Type = type;
        }
        public Guid UserVotedBadgesId { get; set; }
        public Guid VoterId { get; set; }
        public Guid VotedId { get; set; }
        public string Type { get; set; }
        private UserVotedBadges()
        {

        }
        public static Result<UserVotedBadges> Create(Guid voterId,Guid votedId, string type)
        {
            if (voterId == Guid.Empty)
            {
                return Result<UserVotedBadges>.Failure(Constants.VoterIdRequired);
            }
            if (votedId == Guid.Empty)
            {
                return Result<UserVotedBadges>.Failure(Constants.VotedIdRequired);
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                return Result<UserVotedBadges>.Failure(Constants.BadgeTypeRequired);
            }

            return Result<UserVotedBadges>.Success(new UserVotedBadges(voterId, votedId, type));
            
        }

    }
}
