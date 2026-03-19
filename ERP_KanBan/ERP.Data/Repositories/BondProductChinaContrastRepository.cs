using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class BondProductChinaContrastRepository : BaseRepository<BondProductChinaContrast>
    {
        public BondProductChinaContrastRepository(DbContext db) : base(db) { }
    }
}
