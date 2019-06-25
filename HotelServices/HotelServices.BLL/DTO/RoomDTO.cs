using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object for room
    /// </summary>
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public int Seats { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public ICollection<BookingDTO> Bookings { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RoomDTO()
        {
            Bookings = new List<BookingDTO>();
        }
    }
}
