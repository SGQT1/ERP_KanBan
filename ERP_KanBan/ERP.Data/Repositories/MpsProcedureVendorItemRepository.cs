using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedureVendorItemRepository : Bases.BaseRepository<MpsProcedureVendorItem>
    {
        public MpsProcedureVendorItemRepository(DbContext db) : base(db)
        {
        }
    }
}
