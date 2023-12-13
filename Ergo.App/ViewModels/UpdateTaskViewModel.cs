using System.ComponentModel.DataAnnotations;

namespace Ergo.App.ViewModels;

public class UpdateTaskViewModel : TaskViewModel
{
    [Required(ErrorMessage = "Task ID is required.")]
    public Guid TaskId { get; set; }
}