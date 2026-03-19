using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MaterialRepository : Bases.BaseRepository<Material>
    {
        public MaterialRepository(DbContext db) : base(db)
        {
        }
    }
}