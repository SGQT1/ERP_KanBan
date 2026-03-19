using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class KnifeRepository : Bases.BaseRepository<Knife>
    {
        public KnifeRepository(DbContext db) : base(db)
        {
        }
    }
}