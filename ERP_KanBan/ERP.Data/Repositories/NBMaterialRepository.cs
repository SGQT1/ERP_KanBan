using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBMaterialRepository : Bases.BaseRepository<NBMaterial>
    {
        public NBMaterialRepository(DbContext db) : base(db) { }
    }
}