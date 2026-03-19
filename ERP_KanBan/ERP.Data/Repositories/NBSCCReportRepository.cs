using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBSCCReportRepository : Bases.BaseRepository<NBSCCReport>
    {
        public NBSCCReportRepository(DbContext db) : base(db) { }
    }
}