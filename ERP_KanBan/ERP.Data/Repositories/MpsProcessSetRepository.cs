using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcessSetRepository : Bases.BaseRepository<MpsProcessSet>
    {
        public MpsProcessSetRepository(DbContext db) : base(db)
        {
        }
    }
}
