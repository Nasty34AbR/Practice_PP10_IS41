using System.ComponentModel.DataAnnotations;

namespace VodokanalWeb.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Требуется текущий пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string ?OldPassword { get; set; }

        [Required(ErrorMessage = "Требуется новый пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string ?NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ?ConfirmPassword { get; set; }
    }
}
