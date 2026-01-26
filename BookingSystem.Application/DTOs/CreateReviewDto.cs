using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Application.DTOs
{
    public class CreateReviewDto
    {
        [Required]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Рејтингот е задолжителен")]
        [Range(1, 5, ErrorMessage = "Рејтингот мора да биде од 1 до 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Коментарот е задолжителен")]
        [StringLength(500, ErrorMessage = "Коментарот може да биде до 500 карактери")]
        public string Comment { get; set; } = string.Empty;
    }
}