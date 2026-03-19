using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class QuotationRepository : BaseRepository<Quotation>
    {
        public QuotationRepository(DbContext db) : base(db)
        {
        }
    }
}