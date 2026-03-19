using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MRPItemUsageRepository : Bases.BaseRepository<MRPItemUsage>
    {
        public MRPItemUsageRepository(DbContext db) : base(db)
        {
        }
    }
}
