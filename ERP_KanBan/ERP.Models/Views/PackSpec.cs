using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class PackSpec
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal BrandCodeId { get; set; }
        public string Brand { get; set; }
        public int Type { get; set; }
        public string Spec { get; set; }
        public string L { get; set; }
        public string W { get; set; }
        public string H { get; set; }      
        public decimal TextureCodeId { get; set; }  
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string RefBrand { get; set; }    
    }
}