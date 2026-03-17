using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Web.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Комментарий не может превышать 1000 символов")]
        public string? Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Внешние ключи
        public int BookId { get; set; }
        public int ReaderId { get; set; }

        // Навигационные свойства
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }
    }
}