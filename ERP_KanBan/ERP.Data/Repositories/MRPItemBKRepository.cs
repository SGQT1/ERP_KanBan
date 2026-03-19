using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MRPItemBKRepository : Bases.BaseRepository<MRPItemBK>
    {
        public MRPItemBKRepository(DbContext db) : base(db)
        {
        }
    }
}
