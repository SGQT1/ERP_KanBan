using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyPrintLogRepository : Bases.BaseRepository<MpsDailyPrintLog>
    {
        public MpsDailyPrintLogRepository(DbContext db) : base(db)
        {
        }
    }
}
