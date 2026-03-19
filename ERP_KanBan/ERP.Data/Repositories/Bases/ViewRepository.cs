using ERP.Data.DbContexts;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
// using Z.EntityFramework.Plus;

namespace ERP.Data.Repositories.Bases
{
    public abstract class ViewRepository<T> where T : class
    {
        protected DbContext Db { get; }

        public ViewRepository(DbContext db)
        {
            Db = db;
        }
    }
}