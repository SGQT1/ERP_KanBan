using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class BondMaterialChinaContrastService : BusinessService
    {
        private ERP.Services.Business.Entities.BondMaterialChinaContrastService BondMaterialChinaContrast { get; set; }
        public BondMaterialChinaContrastService(
            ERP.Services.Business.Entities.BondMaterialChinaContrastService bondMaterialChinaContrastService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            BondMaterialChinaContrast = bondMaterialChinaContrastService;
        }

        public ERP.Models.Views.BondMaterialChinaContrast Get(int id, int localeId)
        {
            return BondMaterialChinaContrast.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public void Save(List<BondMaterialChinaContrast> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                UnitOfWork.BeginTransaction();
                if (items.Count() > 0)
                {
                    var addItems = items.Where(i => i.Id == 0 || i.Id == null).ToList();
                    var updateItems = items.Where(i => i.Id > 0).ToList();

                    BondMaterialChinaContrast.CreateRange(addItems);
                    BondMaterialChinaContrast.UpdateRange(updateItems);
                    UnitOfWork.Commit();
                }
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(List<decimal> ids, int localeId)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                BondMaterialChinaContrast.RemoveRange(i => ids.Contains(i.Id) && i.LocaleId == localeId);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
