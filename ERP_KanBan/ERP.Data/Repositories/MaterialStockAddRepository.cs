using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MaterialStockAddRepository : Bases.BaseRepository<MaterialStockAdd>
    {
        public MaterialStockAddRepository(DbContext db) : base(db)
        {
        }
    }
}