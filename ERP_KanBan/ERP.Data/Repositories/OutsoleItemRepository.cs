using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OutsoleItemRepository : BaseRepository<OutsoleItem>
    {
        public OutsoleItemRepository(DbContext db) : base(db)
        {
        }
    }
}