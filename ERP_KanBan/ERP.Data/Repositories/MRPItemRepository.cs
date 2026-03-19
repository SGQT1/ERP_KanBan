using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MRPItemRepository : Bases.BaseRepository<MRPItem>
    {
        public MRPItemRepository(DbContext db) : base(db)
        {
        }
    }
}
