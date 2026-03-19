using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class WIPInvTxnRepository : BaseRepository<WIPInvTxn>
    {
        public WIPInvTxnRepository(DbContext db) : base(db)
        {
        }
    }
}