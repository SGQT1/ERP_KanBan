using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ERP.Data.Repositories
{
    public class RepPCLRepository : BaseRepository<REP_PCL>
    {
        public RepPCLRepository(DbContext db) : base(db) { }
    }
}
