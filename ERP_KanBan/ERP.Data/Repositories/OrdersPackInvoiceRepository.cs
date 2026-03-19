using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersPackInvoiceRepository : Bases.BaseRepository<OrdersPackInvoice>
    {
        public OrdersPackInvoiceRepository(DbContext db) : base(db) { }
    }
}