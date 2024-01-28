using Ergo.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ergo.App.ViewModels
{
    public class UpdateProjectViewModel
    {
        [Required(ErrorMessage = "Project ID is required.")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(100, ErrorMessage = "Project name must be 100 characters or less.")]
        public string? ProjectName { get; set; }

        [StringLength(500, ErrorMessage = "Description must be 500 characters or less.")]
        public string? Description { get; set; }

        public string? GitRepository { get; set; }

        public string? ModifiedBy { get; set; }

        [Required(ErrorMessage = "Last Modified Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "Project state is required.")]
        public ProjectState State { get; set; }


        public bool IsEditing { get; set; }
    }
}
