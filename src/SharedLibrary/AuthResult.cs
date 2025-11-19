using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Uid { get; set; } 
        public string Token { get; set; }
    }
}
