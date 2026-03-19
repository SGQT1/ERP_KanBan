using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class CompanyRepository : Bases.BaseRepository<Company>
    {
        public CompanyRepository(DbContext db) : base(db)
        {
        }
    }
}