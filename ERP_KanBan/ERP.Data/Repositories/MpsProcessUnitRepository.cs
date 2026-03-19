using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcessUnitRepository : Bases.BaseRepository<MpsProcessUnit>
    {
        public MpsProcessUnitRepository(DbContext db) : base(db)
        {
        }
    }
}
