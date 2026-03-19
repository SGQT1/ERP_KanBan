using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class POItemVendorETDRepository : BaseRepository<POItemVendorETD>
    {
        public POItemVendorETDRepository(DbContext db) : base(db)
        {
        }
    }
}
