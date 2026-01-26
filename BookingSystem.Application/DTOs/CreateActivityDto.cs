using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Application.DTOs
{
    public class CreateActivityDto
    {
        [Required(ErrorMessage = "Името е задолжително")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Македонското име е задолжително")]
        [StringLength(100)]
        public string NameMk { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описот е задолжителен")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Иконата е задолжителна")]
        public string Icon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цената е задолжителна")]
        [Range(0, 10000, ErrorMessage = "Цената мора да биде помеѓу 0 и 10000")]
        public decimal PricePerHour { get; set; }

        [Required(ErrorMessage = "Максималниот број играчи е задолжителен")]
        [Range(1, 20, ErrorMessage = "Максималниот број играчи мора да биде помеѓу 1 и 20")]
        public int MaxPlayers { get; set; }

        public bool IsActive { get; set; } = true;
    }
}