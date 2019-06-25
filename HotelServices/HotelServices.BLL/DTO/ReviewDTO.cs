using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object for review
    /// </summary>
    public class ReviewDTO
    {
        public int Id { get; set; }
        public DateTime PublDate { get; set; }
        public string UserEmail { get; set; }
        public string Text { get; set; }
    }
}
