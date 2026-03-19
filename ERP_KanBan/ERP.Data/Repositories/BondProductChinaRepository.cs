using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class BondProductChinaRepository : BaseRepository<BondProductChina>
    {
        public BondProductChinaRepository(DbContext db) : base(db) { }
    }
}
