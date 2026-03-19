using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsStyleItemRepository : Bases.BaseRepository<MpsStyleItem>
    {
        public MpsStyleItemRepository(DbContext db) : base(db)
        {
        }
    }
}
