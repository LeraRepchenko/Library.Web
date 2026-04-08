using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Web.Models
{
    public enum NotificationStatus
    {
        Unread = 0,
        Read = 1
    }

    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;

        public string ReaderId { get; set; }  // ← string
        public int? BookId { get; set; }
        public int? LoanId { get; set; }

        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [ForeignKey("LoanId")]
        public Loan? Loan { get; set; }
    }
}