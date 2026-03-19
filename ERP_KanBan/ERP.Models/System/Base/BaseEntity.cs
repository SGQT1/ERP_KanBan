using System;
using System.Collections.Generic;

namespace ERP.Models.System.Base
{
    public class BaseEntity
    {
        public ModelState ModelState { get; set; }
        // private ModelState _ModelState;
        // public ModelState ModelState 
        // { 
        //     get { return _ModelState == null ? new ModelState() { isValid = false, Code = String.Empty} : _ModelState; }
        //     set { _ModelState = value; }
        // }
    }
}
