// Vote Model
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmkveldAPI.Models
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        
        [ForeignKey("Filmkveld")]
        public int FilmkveldId { get; set; }
        public Filmkveld? MovieNight { get; set; }
        
        public string MovieTitle { get; set; }
    }
}