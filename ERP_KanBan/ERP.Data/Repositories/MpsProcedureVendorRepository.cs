using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcedureVendorRepository : Bases.BaseRepository<MpsProcedureVendor>
    {
        public MpsProcedureVendorRepository(DbContext db) : base(db)
        {
        }
    }
}
