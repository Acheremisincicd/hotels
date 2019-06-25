using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Entities
{
    /// <summary>
    /// Model of messages whose objects will be stored in the database
    /// </summary>
    public class Message
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }

        public string MessageText { get; set; }
        public DateTime DateTime { get; set; }
    }
}
