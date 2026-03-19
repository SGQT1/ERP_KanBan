using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class KnifeItemRepository : Bases.BaseRepository<KnifeItem>
    {
        public KnifeItemRepository(DbContext db) : base(db)
        {
        }
    }
}