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
    public class ExchangeRateService : BusinessService
    {
        private ERP.Services.Business.Entities.ExchangeRateService ExchangeRate { get; set; }
        public ExchangeRateService(
            ERP.Services.Business.Entities.ExchangeRateService exchangeRateService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ExchangeRate = exchangeRateService;
        }

        public IQueryable<ERP.Models.Views.ExchangeRate> GetExchangeRate()
        {   
            return ExchangeRate.Get();
        }
        public ERP.Models.Views.ExchangeRate CreateExchangeRate(ExchangeRate item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                item = ExchangeRate.Create(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.ExchangeRate UpdateExchangeRate(ExchangeRate item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                item = ExchangeRate.Update(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemoveExchangeRate(ExchangeRate item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                ExchangeRate.Remove(item);
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
