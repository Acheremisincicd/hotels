using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Entities
{
    /// <summary>
    /// Model of reviews whose objects will be stored in the database
    /// </summary>
    public class Review
    {
        public int Id { get; set; }
        public DateTime PublDate {get;set;}
        public string UserEmail { get; set; }
        public string Text { get; set; }
    }
}
