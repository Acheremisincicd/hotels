using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Entities
{
    /// <summary>
    /// Model of booking application whose objects will be stored in the database
    /// </summary>
    public class BookingApplication
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
