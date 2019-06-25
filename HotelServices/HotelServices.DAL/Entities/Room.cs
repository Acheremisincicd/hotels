using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Entities
{
    /// <summary>
    /// Model of room whose objects will be stored in the database
    /// </summary>
    public class Room
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public int Seats { get; set; }        
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Room()
        {
            Bookings = new List<Booking>();
        }
    }
}
