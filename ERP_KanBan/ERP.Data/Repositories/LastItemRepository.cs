using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class LastItemRepository : BaseRepository<LastItem>
    {
        public LastItemRepository(DbContext db) : base(db)
        {
        }
    }
}