namespace Ergo.App.ViewModels;

public class UpdateCommentDto
{
    public Guid CommentId { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string LastModifiedBy { get; set; } = default!;
    public DateTime LastModifiedDate { get; set; }
    public string CommentText { get; set; } = default!;
    public Guid TaskId { get; set; }
}