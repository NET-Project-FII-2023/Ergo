using Ergo.Domain.Entities.Enums;
namespace Ergo.App.ViewModels
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; } = default!;

        public DateTime LastModifiedDate { get; set; }
        public string CommentText { get; set; } = default!;
        public Guid TaskId { get; set; }

       
    }
}
