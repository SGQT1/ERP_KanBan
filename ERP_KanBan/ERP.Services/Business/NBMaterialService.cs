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
    public class NBMaterialService : BusinessService
    {
        private ERP.Services.Business.Entities.NBMaterialService NBMaterial { get; set; }

        public NBMaterialService(
            ERP.Services.Business.Entities.NBMaterialService nbMaterialService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            NBMaterial = nbMaterialService;
        }
        public IQueryable<ERP.Models.Views.NBMaterial> Get()
        {
            return NBMaterial.Get();
        }
        public List<ERP.Models.Views.NBMaterial> Save(List<ERP.Models.Views.NBMaterial> items)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                // check MaterialId, MaterialCode, ColorKey
                var materials = items.Where(i => i.MaterialId != null && i.MaterialCode != null).ToList();

                // step0.get process items.
                var localeId = materials.FirstOrDefault().LocaleId;
                var mIds = materials.Select(i => i.MaterialId).ToList();

                // step1.remove NBMaterial.
                NBMaterial.RemoveRange(mIds, localeId);

                // step2.insert NBMaterial
                NBMaterial.CreateRange(materials);

                NBMaterial.ExecuteSqlCommand();

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