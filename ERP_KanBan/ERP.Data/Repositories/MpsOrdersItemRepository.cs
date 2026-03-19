using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsOrdersItemRepository : Bases.BaseRepository<MpsOrdersItem>
    {
        public MpsOrdersItemRepository(DbContext db) : base(db)
        {
        }
    }
}
