using System;

namespace ERP.Models.Options
{
    public class AuthorizationOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expires { get; set; }
        public string Secret { get; set; }
    }
}