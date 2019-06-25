using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for bookings
    /// </summary>
    public class BookingViewModel
    {
        [Display(Name = "№")]
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Дата въезда")]
        public DateTime CheckIn { get; set; }
        [Required]
        [Display(Name = "Дата выезда")]
        public DateTime CheckOut { get; set; }
        [Display(Name = "Сумма заказа")]
        public decimal Sum { get; set; }
        [Display(Name = "Пользователь")]
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Время бронирования")]
        public DateTime Time { get; set; }

        [Display(Name = "Номер комнаты")]
        public int? RoomId { get; set; }
        public RoomViewModel Room { get; set; }

        [Display(Name = "Оплачено")]
        public bool IsPaid { get; set; }

        public int? ApplicationId { get; set; }       
    }
}