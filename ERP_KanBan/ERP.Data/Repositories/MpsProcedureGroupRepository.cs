using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedureGroupRepository : Bases.BaseRepository<MpsProcedureGroup>
    {
        public MpsProcedureGroupRepository(DbContext db) : base(db)
        {
        }
    }
}
