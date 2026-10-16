using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{
    [Table("Result")]
    public class Result
    {
        [Key]
        public int ResultId { get; set; }

        [Required]
        public int ParticipantId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required, Column(TypeName = "time")]
        public TimeSpan FinishingTime { get; set; }

        [Required]
        public int Position { get; set; }

        public Participant Participant { get; set; } = null!;
        public Events Event { get; set; } = null!;
    }
}
