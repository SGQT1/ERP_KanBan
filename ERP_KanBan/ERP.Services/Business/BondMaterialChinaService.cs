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
    public class BondMaterialChinaService : BusinessService
    {
        private ERP.Services.Business.Entities.BondMaterialChinaService BondMaterialChina { get; set; }
        public BondMaterialChinaService(
            ERP.Services.Business.Entities.BondMaterialChinaService bondMaterialChinaService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            BondMaterialChina = bondMaterialChinaService;
        }

        public ERP.Models.Views.BondMaterialChina Get(int id)
        {   
            return BondMaterialChina.Get().Where(i => i.Id == id).FirstOrDefault();
        }
        public ERP.Models.Views.BondMaterialChina GetBondMaterialChinaName(string name)
        {   
            return BondMaterialChina.Get().Where(i => i.BondMaterialName == name).FirstOrDefault();
        }
        public ERP.Models.Views.BondMaterialChina Save(BondMaterialChina item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = BondMaterialChina.Get().Where(i => i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = BondMaterialChina.Update(item);
                }
                else
                {
                    item = BondMaterialChina.Create(item);
                }

                UnitOfWork.Commit();
                return Get((int)item.Id);
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(BondMaterialChina item)
        {   
            UnitOfWork.BeginTransaction();
            try
            { 
                BondMaterialChina.Remove(item);
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
