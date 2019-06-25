using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.BLL.Infrastructure
{
    /// Class for keeping an information about the exception
    public class ValidationException: Exception
    {
        public string Property { get; protected set; }
        /// <summary>
        /// Constructor with 2 parameters, which keep property where was an exception and message of it's exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="prop"></param>
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
