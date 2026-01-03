using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Application.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Корисничкото име е задолжително")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email адресата е задолжителна")]
        [EmailAddress(ErrorMessage = "Невалидна email адреса")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Лозинката е задолжителна")]
        [MinLength(6, ErrorMessage = "Лозинката мора да има најмалку 6 карактери")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потврдата на лозинката е задолжителна")]
        [Compare("Password", ErrorMessage = "Лозинките не се совпаѓаат")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Името е задолжително")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Презимето е задолжително")]
        public string LastName { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Корисничкото име е задолжително")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Лозинката е задолжителна")]
        public string Password { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}