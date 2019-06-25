using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object for booking
    /// </summary>
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Sum { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public DateTime Time { get; set; }

        public int? RoomId { get; set; }
        public RoomDTO Room { get; set; }

        public bool IsPaid { get; set; }

        public int? ApplicationId { get; set; }
    }
}
