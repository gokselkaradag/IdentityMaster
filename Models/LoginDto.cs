using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Kullanıcı Adınızı Giriniz")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre Hatalı Tekrar Giriniz")]
        public string Password { get; set; }
    }
}
