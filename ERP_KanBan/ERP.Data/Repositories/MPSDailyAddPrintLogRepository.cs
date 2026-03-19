using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MPSDailyAddPrintLogRepository : Bases.BaseRepository<MpsDailyAddPrintLog>
    {
        public MPSDailyAddPrintLogRepository(DbContext db) : base(db)
        {
        }
    }
}
