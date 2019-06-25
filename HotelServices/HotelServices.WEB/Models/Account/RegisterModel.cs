using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for user to register in system
    /// </summary>
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", 
            ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название города не может быть " +
            "слишком коротким")]
        [Display(Name = "Адресс")]
        public string Address { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя не может быть" +
            "слишком коротким")]
        [Display(Name = "Имя")]
        public string ClientName { get; set; }
    }
}