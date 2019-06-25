using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for rooms
    /// </summary>
    public class RoomViewModel
    {
        [Display(Name = "Номер комнаты")]
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Класс")]
        public string Class { get; set; }
        [Required]
        [Display(Name = "Количество мест")]
        public int Seats { get; set; }
        [Required]
        [Display(Name = "Статус")]
        public string Status { get; set; }
        [Required]
        [Display(Name = "Стоимость за сутки")]
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public ICollection<BookingViewModel> Bookings { get; set; }

        public RoomViewModel()
        {
            Bookings = new List<BookingViewModel>();
        }
    }
}