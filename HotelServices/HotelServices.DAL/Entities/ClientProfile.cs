using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.DAL.Entities
{
    /// <summary>
    /// Auxiliary user data which does not play any role in authentication
    /// </summary>
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string ClientName { get; set; }
        public string Address { get; set; }
        public decimal СashAccount { get; set; }
 
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
