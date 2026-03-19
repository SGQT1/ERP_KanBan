using ERP.Data.DbContexts;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace ERP.Data.Repositories
{
    public class ViewBatchProductionCostRepository : BaseRepository<VIEW_BATCHPRODUCTIONCOST>
    {
        public ViewBatchProductionCostRepository(DbContext db) : base(db) { }
    }
}
