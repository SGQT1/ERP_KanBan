using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBPOItemRepository : Bases.BaseRepository<NBPOItem>
    {
        public NBPOItemRepository(DbContext db) : base(db) { }
    }
}