using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ERP.Data.Repositories
{
    public class RepPCLBKRepository : BaseRepository<REP_PCLBK>
    {
        public RepPCLBKRepository(DbContext db) : base(db) { }
    }
}
