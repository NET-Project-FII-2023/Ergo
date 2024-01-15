using Ergo.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ergo.App.ViewModels
{
    public class ProjectViewModel
    {
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        public string ProjectName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description must be 500 characters or less.")]
        public string? Description { get; set; }

        public string? FullName { get; set; }

        public string? GitRepository { get; set; }

        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        public bool IsEditing { get; set; }

        public ProjectState State { get; set; }

        public string ModifiedBy { get; set; }
    }
}
