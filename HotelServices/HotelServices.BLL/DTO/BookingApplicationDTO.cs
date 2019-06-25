using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object for booking application
    /// </summary>
    public class BookingApplicationDTO
    {
        public int Id { get; set; }
        public int Seats { get; set; }
        public string Class { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
    }
}
