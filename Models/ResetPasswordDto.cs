using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Models
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Token Eşleşmiyor")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Şifre Alanı Gereklidir")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mail Alanı Gereklidir")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Tekrar Alanı Gereklidir")]
        [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
