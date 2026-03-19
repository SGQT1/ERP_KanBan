using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcessRepository : Bases.BaseRepository<MpsProcess>
    {
        public MpsProcessRepository(DbContext db) : base(db)
        {
        }
    }
}
