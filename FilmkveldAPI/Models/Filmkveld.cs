// Filmkveld Model
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmkveldAPI.Models
{
    public class Filmkveld
{
    [Key]
    public int FilmkveldId { get; set; }

    [Required] public required string Title { get; set; }
    [Required] public required string Genre { get; set; }
    [Required] public required string Location { get; set; }
    
    [Required]
    public DateTime EventDate { get; set; }

    public int MaxParticipants { get; set; }
    
    [ForeignKey("User")] 
    public int OwnerId { get; set; }
    public User? Owner { get; set; }

    public List<Vote>? Votes { get; set; }
}
}