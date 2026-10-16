using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{

    [Table("route")]
    public class route
    {
        [Key]
        [Column("RouteID")]
        public int RouteId { get; set; }

        [Required]
        [Column("EventID")]
        public int EventId { get; set; }

        [Required, MaxLength(100)]
        public string RouteName { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(6,2)")]
        public decimal Distance { get; set; }

        public Events Event { get; set; } = null!;
    }
}
