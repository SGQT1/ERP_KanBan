using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class MPSOutsourceService : SearchService
    {
        ERP.Services.Business.Entities.MPSProcedurePOService MPSProcedurePO;

        public MPSOutsourceService(
            ERP.Services.Business.Entities.MPSProcedurePOService mpsProcedurePOService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedurePO = mpsProcedurePOService;
        }

        public IQueryable<Models.Views.MPSProcedurePO> GetMPSOutsource(string predicate)
        {
            return MPSProcedurePO.Get()
                   .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                   .ToList()
                   .AsQueryable();
        }

    }
}