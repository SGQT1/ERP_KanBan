using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyMaterialItemAddRepository : Bases.BaseRepository<MpsDailyMaterialItemAdd>
    {
        public MpsDailyMaterialItemAddRepository(DbContext db) : base(db)
        {
        }
    }
}
