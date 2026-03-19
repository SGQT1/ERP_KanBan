using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ERP.Data.Repositories
{
    public class ViewOrdersItemSizeRunRepository : BaseRepository<VIEW_ORDERSITEM_SIZERUN>
    {
        public ViewOrdersItemSizeRunRepository(DbContext db) : base(db) { }
    }
}
