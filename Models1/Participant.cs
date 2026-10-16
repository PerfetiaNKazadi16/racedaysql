using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{

    [Table("Participant")]
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string? ParticipantName { get; set; }
        public int UserId { get; set; }
        public Users users { get; set; } = null!;
        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
        public ICollection<Result> Results { get; set; } = new List<Result>();

    }
}
