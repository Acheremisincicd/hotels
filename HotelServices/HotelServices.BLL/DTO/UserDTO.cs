using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.DTO
{
    /// <summary>
    /// Data Transfer Object for user
    /// </summary>
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public decimal СashAccount { get; set; }
    }
}
