using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyLangAddRepository : Bases.BaseRepository<MpsDailyLangAdd>
    {
        public MpsDailyLangAddRepository(DbContext db) : base(db)
        {
        }
    }
}
