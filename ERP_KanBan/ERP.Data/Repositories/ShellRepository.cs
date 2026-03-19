using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class ShellRepository : BaseRepository<Shell>
    {
        public ShellRepository(DbContext db) : base(db)
        {
        }
    }
}