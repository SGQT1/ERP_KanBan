using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.System;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

// using Z.EntityFramework.Plus;

namespace ERP.Services.Business
{
    public class NBPPMService : BusinessService
    {
        private ERP.Services.Business.Entities.NBPPMService NBPPM { get; set; }

        public NBPPMService(
            ERP.Services.Business.Entities.NBPPMService nbPPMService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            NBPPM = nbPPMService;
        }
        public IQueryable<ERP.Models.Views.NBPPM> Get()
        {
            return NBPPM.Get();
        }
        public List<ERP.Models.Views.NBPPM> Save(List<ERP.Models.Views.NBPPM> items)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                // check MaterialId, MaterialCode, ColorKey
                var materials = items.Where(i => i.Id == 0).ToList();

                // step2.insert NBMaterial
                NBPPM.CreateRange(materials);

                UnitOfWork.Commit();
                return items;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}