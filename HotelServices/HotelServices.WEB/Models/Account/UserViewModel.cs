using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelServices.WEB.Models
{
    /// <summary>
    /// View Model for user
    /// </summary>
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public decimal СashAccount { get; set; }
    }
}