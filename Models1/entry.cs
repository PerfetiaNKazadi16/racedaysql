using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{
    [Table("entry")]
    public class Entry
    {
        [Key]
        [Column("EntryID")]
        public int EntryId { get; set; }

        [Required]
        [Column("ParticipantID")]
        public int ParticipantId { get; set; }

        [Required]
        [Column("EventID")]
        public int EventId { get; set; }

         
        [Required]
        public int CategoryId { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public Participant Participant { get; set; } = null!;
        public Events Event { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
