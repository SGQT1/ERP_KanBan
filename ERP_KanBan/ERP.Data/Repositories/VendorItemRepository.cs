using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class VendorItemRepository : BaseRepository<VendorItem>
    {
        public VendorItemRepository(DbContext db) : base(db)
        {
        }
    }
}