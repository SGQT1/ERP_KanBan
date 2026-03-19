using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsStyleItemUsageRepository : Bases.BaseRepository<MpsStyleItemUsage>
    {
        public MpsStyleItemUsageRepository(DbContext db) : base(db)
        {
        }
    }
}
