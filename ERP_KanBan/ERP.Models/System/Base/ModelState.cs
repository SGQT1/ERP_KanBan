using System;
using System.Collections.Generic;

namespace ERP.Models.System
{
    public class ModelState
    {
        // private bool? _valid;
        // private string _code;
        // public bool isValid { get{ return _valid ?? false; } set {_valid = value ;} }
        // public string Code {  get{ return _code ?? String.Empty;} set {_code = value ;}}

        public bool isValid { get;set; }
        public string Code {  get;set; }
    }
}
