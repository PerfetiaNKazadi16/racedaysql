using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{

    [Table("Organiser")]
    public class Organiser
    {
        public int OrganiserId { get; set; }
        public int OrganiserNumber { get;set; }
        public int UserId { get; set; }
        public Users Users { get; set; } = null!;
        public ICollection<Events> Event { get; set; } = new List<Events>();

    }
}
