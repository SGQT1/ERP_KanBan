using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsDailyLangRepository : Bases.BaseRepository<MpsDailyLang>
    {
        public MpsDailyLangRepository(DbContext db) : base(db)
        {
        }
    }
}
