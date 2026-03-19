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
    public class BondProductChinaService : BusinessService
    {
        private ERP.Services.Business.Entities.BondProductChinaService BondProductChina { get; set; }
        public BondProductChinaService(
            ERP.Services.Business.Entities.BondProductChinaService bondProductChinaService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            BondProductChina = bondProductChinaService;
        }

        public ERP.Models.Views.BondProductChina Get(int id)
        {   
            return BondProductChina.Get().Where(i => i.Id == id).FirstOrDefault();
        }
        public ERP.Models.Views.BondProductChina GetBondProductChinaName(string name)
        {   
            return BondProductChina.Get().Where(i => i.BondProductName == name).FirstOrDefault();
        }
        public ERP.Models.Views.BondProductChina Save(BondProductChina item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = BondProductChina.Get().Where(i => i.Id == item.Id).FirstOrDefault();

                if (_item != null)
                {
                    item.Id = _item.Id;
                    item = BondProductChina.Update(item);
                }
                else
                {
                    item = BondProductChina.Create(item);
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

        public void Remove(BondProductChina item)
        {   
            UnitOfWork.BeginTransaction();
            try
            { 
                BondProductChina.Remove(item);
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
