using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Application.DTOs
{
    public class CreateReviewDto
    {
        public int? ActivityId { get; set; } // Nullable за playroom reviews

        public int? AppointmentId { get; set; } // NEW - за activity reviews

        [Required(ErrorMessage = "Типот на рецензија е задолжителен")]
        public string ReviewType { get; set; } = "Activity"; // "Activity" or "Playroom"

        [Required(ErrorMessage = "Рејтингот е задолжителен")]
        [Range(1, 5, ErrorMessage = "Рејтингот мора да биде од 1 до 5")]
        public int Rating { get; set; }

        
        [StringLength(500, ErrorMessage = "Коментарот може да биде до 500 карактери")]
        public string Comment { get; set; }
    }
}