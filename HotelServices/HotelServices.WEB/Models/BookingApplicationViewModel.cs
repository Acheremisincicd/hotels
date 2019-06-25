using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for booking applications
    /// </summary>
    public class BookingApplicationViewModel
    {
        [Display(Name = "№")]
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Количество мест")]
        public int Seats { get; set; }
        [Required]
        [Display(Name = "Класс")]
        public string Class { get; set; }
        [Required]
        [Display(Name = "Дата въезда")]
        public DateTime CheckIn { get; set; }
        [Required]
        [Display(Name = "Дата выезда")]
        public DateTime CheckOut { get; set; }
        [Display(Name = "Пользователь")]
        public string UserEmail { get; set; }
        public string UserId { get; set; }
    }
}