using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название жанра обязательно")]
        [StringLength(50, ErrorMessage = "Название жанра не может превышать 50 символов")]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Навигационное свойство
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}