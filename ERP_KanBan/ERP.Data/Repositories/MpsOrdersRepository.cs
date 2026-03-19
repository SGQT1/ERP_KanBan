using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsOrdersRepository : Bases.BaseRepository<MpsOrders>
    {
        public MpsOrdersRepository(DbContext db) : base(db)
        {
        }
    }
}
