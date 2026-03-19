using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyMaterialRepository : Bases.BaseRepository<MpsDailyMaterial>
    {
        public MpsDailyMaterialRepository(DbContext db) : base(db)
        {
        }
    }
}
