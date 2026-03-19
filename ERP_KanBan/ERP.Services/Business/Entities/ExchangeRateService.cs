using System;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ExchangeRateService : BusinessService
    {
        private Services.Entities.ExchangeRateService ExchangeRate { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        public ExchangeRateService(
            Services.Entities.ExchangeRateService exchangeRateService, 
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.ExchangeRate = exchangeRateService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.ExchangeRate> Get()
        {
            return ExchangeRate.Get().Select(i => new Models.Views.ExchangeRate
            {
                Id = i.Id,
                ExchDate = i.ExchDate,
                CodeId = i.CodeId,
                CurrencyTw = i.NameTw,
                CurrencyEn = i.NameEn,
                BankingRate = i.BankingRate,
                CustomRate = i.CustomRate,
                TransCodeId = i.TransCodeId,
                TransCurrencyTw = i.TransNameTw,
                TransCurrencyEn = i.TransNameEn,
                ReversedBankingRate = i.ReversedBankingRate,
                ReversedCustomRate = i.ReversedCustomRate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public Models.Views.ExchangeRate Create(Models.Views.ExchangeRate item)
        {
            var _item = ExchangeRate.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public Models.Views.ExchangeRate Update(Models.Views.ExchangeRate item)
        {
            var _item = ExchangeRate.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.ExchangeRate item)
        {
            ExchangeRate.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.ExchangeRate Build(Models.Views.ExchangeRate item)
        {
            // for TDM LocaleId
            var currency = CodeItem.Get().Where(i => i.LocaleId == 6 && i.CodeType == "02").ToList();
            return new Models.Entities.ExchangeRate()
            {
                Id = item.Id,
                ExchDate = item.ExchDate,
                CodeId = item.CodeId,
                NameTw = currency.Where(i => i.Id == item.CodeId).FirstOrDefault().NameTW,
                NameEn = currency.Where(i => i.Id == item.CodeId).FirstOrDefault().NameEng,
                BankingRate = item.BankingRate,
                CustomRate = item.CustomRate,
                TransCodeId = item.TransCodeId,
                TransNameTw = currency.Where(i => i.Id == item.TransCodeId).FirstOrDefault().NameTW,
                TransNameEn = currency.Where(i => i.Id == item.TransCodeId).FirstOrDefault().NameEng,
                ReversedBankingRate = 1/item.BankingRate,
                ReversedCustomRate = 1/item.CustomRate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
        
    }
}