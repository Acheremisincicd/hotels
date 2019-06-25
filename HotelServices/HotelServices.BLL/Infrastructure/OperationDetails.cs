using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Infrastructure
{
    /// <summary>
    /// Class for keeping an information about the success of the operation
    /// </summary>
    public class OperationDetails
    {
        /// <summary>
        /// Constructor with 3 parameters
        /// </summary>
        /// <param name="succedeed"></param>
        /// <param name="message"></param>
        /// <param name="prop"></param>
        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
    }
}
