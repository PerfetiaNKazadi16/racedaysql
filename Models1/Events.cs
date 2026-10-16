using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{

    [Table("Events")]
    public class Events
    {
        [Key]
       [Column ("EventsId")]
        public int EventsId { get; set; }

        [Required, MaxLength(50)]
        public string? EventsName { get; set; }

        [Required, Column(TypeName = "date")]
        public DateTime EventDate { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        [Required, MaxLength(100)]
        public string? Location { get; set; }
        [Required]
        public EventType EventType { get; set; }
        [Required]
        public int OrganiserId { get; set; }
        [Required]
        public Organiser Organiser { get; set; }
        public ICollection<Category> Categories { get; set; }=new List<Category>();
        public ICollection<route> routes { get; set; } =new List<route>();
        public ICollection<Entry> entries { get; set; }= new List<Entry>();
        public ICollection<Result> Results { get; set; } = new List<Result>();

    }
}
