using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for messages
    /// </summary>
    public class MessageViewModel
    {
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public int ApplicationId { get; set; }

        [Required]
        [Display(Name = "Номер бронирования")]
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Текст сообщения")]
        public string MessageText { get; set; }

        [Required]
        [Display(Name = "Время сообщения")]
        public DateTime DateTime { get; set; }
    }
}