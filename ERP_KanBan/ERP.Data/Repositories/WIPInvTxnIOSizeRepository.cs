using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class WIPInvTxnIOSizeRepository : BaseRepository<WIPInvTxnIOSize>
    {
        public WIPInvTxnIOSizeRepository(DbContext db) : base(db)
        {
        }
    }
}