using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Web.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(200, ErrorMessage = "Название не может превышать 200 символов")]
        public string Title { get; set; }

        [StringLength(20)]
        [Display(Name = "ISBN")]
        public string? Isbn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Год публикации")]
        public int? PublicationYear { get; set; }

        public int? PageCount { get; set; }

        public string? Publisher { get; set; }

        public string? Description { get; set; }

        public string? CoverImage { get; set; }

        // Внешние ключи
        public int AuthorId { get; set; }
        public int GenreId { get; set; }

        // Навигационные свойства
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
