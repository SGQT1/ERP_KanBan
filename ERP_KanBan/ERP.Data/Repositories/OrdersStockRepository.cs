using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Models.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class OrdersStockRepository : BaseRepository<OrdersStock>
    {
        public OrdersStockRepository(DbContext db) : base(db) { }
    }
}
