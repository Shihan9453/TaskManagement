using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class AddTaskDto
    {

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        [Unicode(false)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        [Unicode(false)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(11, ErrorMessage = "Status cannot exceed 11 characters.")]
        [Unicode(false)]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "Priority level is required.")]
        [StringLength(6, ErrorMessage = "Priority level cannot exceed 6 characters.")]
        [Unicode(false)]
        public string PriorityLevel { get; set; } = null!;

        [Required(ErrorMessage = "Assigned person is required.")]
        public int AssignedPersonId { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [ValidDate(ErrorMessage = "Invalid due date. A valid date is expected.")]
        public DateTime DueDate { get; set; }

    }
}
