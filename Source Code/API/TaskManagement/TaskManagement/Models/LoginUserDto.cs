using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class LoginUserDto
    {

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [Unicode(false)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(15, ErrorMessage = "Password cannot exceed 15 characters.")]
        [Unicode(false)]
        public string UserPassword { get; set; } = null!;

    }
}
