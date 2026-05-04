using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Models;

public partial class Task
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("status")]
    [StringLength(11)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column("priority_level")]
    [StringLength(6)]
    [Unicode(false)]
    public string PriorityLevel { get; set; } = null!;

    [Column("assigned_person_id")]
    public int AssignedPersonId { get; set; }

    [Column("due_date", TypeName = "datetime")]
    public DateTime DueDate { get; set; }

    [Column("completed_date", TypeName = "datetime")]
    public DateTime? CompletedDate { get; set; }

    [Column("remarks")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [ForeignKey("AssignedPersonId")]
    [InverseProperty("Tasks")]
    public virtual User AssignedPerson { get; set; } = null!;

    [InverseProperty("Task")]
    public virtual ICollection<TasksHistory> TasksHistories { get; set; } = new List<TasksHistory>();

}

public class ValidDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is DateTime Date)
        {
            DateTime dt;
            if (!DateTime.TryParse(Date.ToString().Trim(), out dt))
            {
                return new ValidationResult("Invalid date. A valid date is expected.");
            }

            if (Date == DateTime.MinValue)
            {
                return new ValidationResult("Invalid date. A valid date is expected.");
            }
        }

        return ValidationResult.Success;
    }
}