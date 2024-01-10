using Ergo.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ergo.App.ViewModels
{
    public class TaskViewModel
    {
        public Guid TaskItemId { get; set; }
        [Required(ErrorMessage = "Task name is required.")]
        [StringLength(100, ErrorMessage = "Task name must be 100 characters or less.")]
        public string? TaskName { get; set; }

        [StringLength(500, ErrorMessage = "Description must be 500 characters or less.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        public string? AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }

        public string? CreatedBy { get; set; }

        [Required(ErrorMessage = "Project ID is required.")]
        public Guid ProjectId { get; set; }

        public TaskState? State { get; set; } = TaskState.ToDo;

        public bool DescriptionEditMode { get; set; }

        public bool IsTitleEditMode { get; set; }
    }
}