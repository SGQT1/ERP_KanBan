using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class MaterialQuotRepository : Bases.BaseRepository<MaterialQuot>
    {
        public MaterialQuotRepository(DbContext db) : base(db)
        {
        }
    }
}