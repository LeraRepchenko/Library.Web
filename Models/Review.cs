using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Web.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int BookId { get; set; }
        public string ReaderId { get; set; }  // ← string

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }
    }
}