using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class PUMAOrdersItemRepository : Bases.BaseRepository<PUMAOrdersItem>
    {
        public PUMAOrdersItemRepository(DbContext db) : base(db) { }
    }
}