using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Core.DTOs
{
    public class RequestTokenDTO
    {
        public string Token { get; set; }
        public string result { get; set; }
        public string errors { get; set; }
        public int status { get; set; }
        public string tokenType { get; set; }
        public string expiresIn { get; set; }

    }

    public class RequestToken
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
