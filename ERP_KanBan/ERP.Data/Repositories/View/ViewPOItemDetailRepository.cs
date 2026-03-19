using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ERP.Data.Repositories
{
    public class ViewPOItemDetailRepository : BaseRepository<VIEW_POITEM_DETAIL>
    {
        public ViewPOItemDetailRepository(DbContext db) : base(db) { }
    }
}
