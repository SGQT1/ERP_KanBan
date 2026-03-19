using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class WIPInvTxnIORepository : BaseRepository<WIPInvTxnIO>
    {
        public WIPInvTxnIORepository(DbContext db) : base(db)
        {
        }
    }
}