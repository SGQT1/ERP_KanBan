using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OutsoleTCRepository : BaseRepository<OutsoleTC>
    {
        public OutsoleTCRepository(DbContext db) : base(db)
        {
        }
    }
}