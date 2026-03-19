using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsProcessOrgRepository : Bases.BaseRepository<MpsProcessOrg>
    {
        public MpsProcessOrgRepository(DbContext db) : base(db)
        {
        }
    }
}
