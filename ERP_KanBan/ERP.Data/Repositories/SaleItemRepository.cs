using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class SaleItemRepository : BaseRepository<SaleItem>
    {
        public SaleItemRepository(DbContext db) : base(db) { }
    }
}
