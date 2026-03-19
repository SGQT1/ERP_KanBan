using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedurePOSizeRepository : Bases.BaseRepository<MpsProcedurePOSize>
    {
        public MpsProcedurePOSizeRepository(DbContext db) : base(db)
        {
        }
    }
}
