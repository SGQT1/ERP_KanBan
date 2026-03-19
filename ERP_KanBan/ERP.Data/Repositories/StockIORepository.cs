using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class StockIORepository : Bases.BaseRepository<StockIO>
    {
        public StockIORepository(DbContext db) : base(db)
        {
        }
    }
}