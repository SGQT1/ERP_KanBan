using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBOrdersItemRepository : Bases.BaseRepository<NBOrdersItem>
    {
        public NBOrdersItemRepository(DbContext db) : base(db) { }
    }
}