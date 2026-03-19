using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyRepository : Bases.BaseRepository<MpsDaily>
    {
        public MpsDailyRepository(DbContext db) : base(db)
        {
        }
    }
}
