using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrgUnitRepository : Bases.BaseRepository<OrgUnit>
    {
        public OrgUnitRepository(DbContext db) : base(db) { }
    }
}