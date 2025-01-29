// User Model

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmkveldAPI.Models
{public class User
{
    [Key]
    public int UserId { get; set; }

    [Required] public required string UserName { get; set; }
    [Required] public required string Email { get; set; }
    [Required] public required string PasswordHash { get; set; }

    public List<Filmkveld>? MovieNights { get; set; }
}

}