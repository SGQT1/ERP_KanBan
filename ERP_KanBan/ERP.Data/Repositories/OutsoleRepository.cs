using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OutsoleRepository : BaseRepository<Outsole>
    {
        public OutsoleRepository(DbContext db) : base(db)
        {
        }
    }
}