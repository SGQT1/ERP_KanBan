using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedureGroupItemRepository : Bases.BaseRepository<MpsProcedureGroupItem>
    {
        public MpsProcedureGroupItemRepository(DbContext db) : base(db)
        {
        }
    }
}
