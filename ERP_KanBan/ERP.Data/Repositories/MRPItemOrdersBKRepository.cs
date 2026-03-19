using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MRPItemOrdersBKRepository : Bases.BaseRepository<MRPItemOrdersBK>
    {
        public MRPItemOrdersBKRepository(DbContext db) : base(db)
        {
        }
    }
}
