using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Models;

public partial class User
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("user_password")]
    [StringLength(15)]
    [Unicode(false)]
    public string UserPassword { get; set; } = null!;

    [Column("full_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [InverseProperty("AssignedPerson")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [InverseProperty("User")]
    public virtual ICollection<TasksHistory> TasksHistories { get; set; } = new List<TasksHistory>();

}
