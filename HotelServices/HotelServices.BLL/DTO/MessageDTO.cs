using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object for message
    /// </summary>
    public class MessageDTO
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }

        public string MessageText { get; set; }
        public DateTime DateTime { get; set; }
    }
}
