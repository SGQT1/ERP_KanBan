using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class PMStockOutRepository : BaseRepository<PMStockOut>
    {
        public PMStockOutRepository(DbContext db) : base(db)
        {
        }
    }
}
