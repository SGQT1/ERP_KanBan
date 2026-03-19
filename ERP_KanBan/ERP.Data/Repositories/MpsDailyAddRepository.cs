using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyAddRepository : Bases.BaseRepository<MpsDailyAdd>
    {
        public MpsDailyAddRepository(DbContext db) : base(db)
        {
        }
    }
}
