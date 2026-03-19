using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP.Services.Bases
{
    public abstract class ViewService<T> where T : class
    {
        protected virtual ViewRepository<T> Repository { get; }

        public ViewService(ViewRepository<T> repository)
        {
            Repository = repository;
        }
    }
}