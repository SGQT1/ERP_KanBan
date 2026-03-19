using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ERP.Data.Repositories
{
    public class ViewBatchProductionCostStandardRepository : BaseRepository<VIEW_BATCHPRODUCTIONCOSTStandard>
    {
        public ViewBatchProductionCostStandardRepository(DbContext db) : base(db) { }
    }
}
