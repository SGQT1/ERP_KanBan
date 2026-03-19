using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedureRepository : Bases.BaseRepository<MpsProcedure>
    {
        public MpsProcedureRepository(DbContext db) : base(db)
        {
        }
    }
}
