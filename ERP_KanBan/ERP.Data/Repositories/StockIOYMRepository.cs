using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class StockIOYMRepository : Bases.BaseRepository<StockIOYM>
    {
        public StockIOYMRepository(DbContext db) : base(db)
        {
        }
    }
}