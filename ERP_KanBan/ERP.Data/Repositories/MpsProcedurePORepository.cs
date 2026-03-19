using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedurePORepository : Bases.BaseRepository<MpsProcedurePO>
    {
        public MpsProcedurePORepository(DbContext db) : base(db)
        {
        }
    }
}
