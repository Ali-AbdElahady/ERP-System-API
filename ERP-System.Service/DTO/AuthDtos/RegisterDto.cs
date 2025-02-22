using System.ComponentModel.DataAnnotations;

namespace ERP_System.Service.DTO.AuthDtos
{
    public class RegisterDto : UpdateUserDto
    {
        
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$",
            ErrorMessage = "Password must contains 1 Uppercase, 1 Lowercase, 1 Digit, 1 Spaecial Character")]
        public string Password { get; set; }
    }
}
