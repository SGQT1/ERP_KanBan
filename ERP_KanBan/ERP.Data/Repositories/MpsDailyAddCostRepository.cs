using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyAddCostRepository : Bases.BaseRepository<MpsDailyAddCost>
    {
        public MpsDailyAddCostRepository(DbContext db) : base(db)
        {
        }
    }
}
