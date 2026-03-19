using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class VendorRepository : BaseRepository<Vendor>
    {
        public VendorRepository(DbContext db) : base(db)
        {
        }
    }
}