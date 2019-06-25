using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for reviews
    /// </summary>
    public class ReviewViewModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime PublDate { get; set; }
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "Текст отзыва")]
        public string Text { get; set; }
    }
}