using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class PORepository : BaseRepository<PO>
    {
        public PORepository(DbContext db) : base(db)
        {
        }
    }
}
