using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Web.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        // Внешние ключи
        public int BookId { get; set; }
        public int ReaderId { get; set; }

        // Навигационные свойства
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }

        // Вычисляемое свойство
        public bool IsOverdue => !ReturnDate.HasValue && DueDate < DateTime.UtcNow;
        public bool IsReturned => ReturnDate.HasValue;
    }
}
