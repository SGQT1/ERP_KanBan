using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedureQuotRepository : Bases.BaseRepository<MpsProcedureQuot>
    {
        public MpsProcedureQuotRepository(DbContext db) : base(db)
        {
        }
    }
}
