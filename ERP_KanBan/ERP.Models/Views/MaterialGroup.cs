using ERP.Models.System.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views
{
    public class MaterialGroup
    {
        public Material Material { get; set; }
        public IEnumerable<SOM> SOM { get; set; }
        public bool UseFor { get; set; }
    }
}
