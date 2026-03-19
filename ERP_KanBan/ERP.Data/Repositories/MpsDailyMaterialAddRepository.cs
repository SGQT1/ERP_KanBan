using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyMaterialAddRepository : Bases.BaseRepository<MpsDailyMaterialAdd>
    {
        public MpsDailyMaterialAddRepository(DbContext db) : base(db)
        {
        }
    }
}
