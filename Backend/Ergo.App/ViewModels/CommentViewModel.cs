using Ergo.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
namespace Ergo.App.ViewModels
{
    public class CommentViewModel
    {

       
        public Guid CommentId { get; set; }
        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, ErrorMessage = "Created by must be 50 characters or less.")]
        public string CreatedBy { get; set; } = default!;

        [Required(ErrorMessage = "Created date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Last modified by is required.")]
        [StringLength(50, ErrorMessage = "Last modified by must be 50 characters or less.")]
        public string LastModifiedBy { get; set; } = default!;

        [Required(ErrorMessage = "Last modified date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        [StringLength(500, ErrorMessage = "Comment text must be 500 characters or less.")]
        public string CommentText { get; set; } = default!;

        [Required(ErrorMessage = "Task ID is required.")]
        public Guid TaskId { get; set; }

        public bool CanEdit { get; set; }

    }
}
