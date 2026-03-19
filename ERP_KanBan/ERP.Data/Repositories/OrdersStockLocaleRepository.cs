using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersStockLocaleRepository : Bases.BaseRepository<OrdersStockLocale>
    {
        public OrdersStockLocaleRepository(DbContext db) : base(db) { }
    }
}