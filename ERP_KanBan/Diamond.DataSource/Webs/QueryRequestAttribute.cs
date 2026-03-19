using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diamond.DataSource.Webs
{
    public class QueryRequestAttribute : ModelBinderAttribute
    {
        public QueryRequestAttribute()
        {
            BinderType = typeof(QueryRequestBinder);
        }
    }
}
