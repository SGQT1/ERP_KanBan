using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class NBReportUpperSCCRepository : Bases.BaseRepository<NBReportUpperSCC>
    {
        public NBReportUpperSCCRepository(DbContext db) : base(db) { }
    }
}