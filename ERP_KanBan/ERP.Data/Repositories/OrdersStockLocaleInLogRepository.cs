using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersStockLocaleInLogRepository : Bases.BaseRepository<OrdersStockLocaleInLog>
    {
        public OrdersStockLocaleInLogRepository(DbContext db) : base(db) { }
    }
}