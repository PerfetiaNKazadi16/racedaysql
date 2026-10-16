using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(50)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public int EventId { get; set; }

        public Events Event { get; set; } = null!;
        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}
