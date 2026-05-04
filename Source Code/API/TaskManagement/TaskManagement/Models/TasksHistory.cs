using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Models;

[Table("TasksHistory")]
public partial class TasksHistory
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("task_id")]
    public int TaskId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("action")]
    [StringLength(100)]
    [Unicode(false)]
    public string Action { get; set; } = null!;

    [Column("date", TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TasksHistories")]
    public virtual Task Task { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TasksHistories")]
    public virtual User User { get; set; } = null!;

}
