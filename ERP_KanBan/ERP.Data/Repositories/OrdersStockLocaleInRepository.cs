using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersStockLocaleInRepository : Bases.BaseRepository<OrdersStockLocaleIn>
    {
        public OrdersStockLocaleInRepository(DbContext db) : base(db) { }
    }
}