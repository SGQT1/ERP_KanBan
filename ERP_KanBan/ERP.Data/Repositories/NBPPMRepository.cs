using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBPPMRepository : Bases.BaseRepository<NBPPM>
    {
        public NBPPMRepository(DbContext db) : base(db) { }
    }
}