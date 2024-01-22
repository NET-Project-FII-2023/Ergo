namespace Ergo.App.ViewModels;

public class InboxItemViewModel
{
    public string? InboxItemId { get; set; }
    public string? UserId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsRead { get; set; }
}