using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class PMStockRepository : BaseRepository<PMStock>
    {
        public PMStockRepository(DbContext db) : base(db)
        {
        }
    }
}
