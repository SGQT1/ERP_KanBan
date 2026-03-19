using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class TokenGroup
    {
        public string Token {get;set;}
        public User User {get;set;}
        public List<decimal> Brands {get;set;}
    }
}
