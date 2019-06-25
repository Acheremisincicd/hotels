using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Entities
{
    /// <summary>
    /// Model of booking whose objects will be stored in the database
    /// </summary>
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Sum { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public bool IsPaid { get; set; }
        public DateTime Time { get; set; }

        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public int? ApplicationId { get; set; }
    }
}
