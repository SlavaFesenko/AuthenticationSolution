using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class Constants
    {
        public const string Issuer = Audience;
        public const string Audience = "https://localhost:44343/";
        public const string Secret = "loooooooooooooooong secret phrase";
    }
}
