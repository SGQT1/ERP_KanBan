using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class LastRepository : BaseRepository<Last>
    {
        public LastRepository(DbContext db) : base(db)
        {
        }
    }
}