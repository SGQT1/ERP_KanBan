using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OutsoleTCService : BusinessService
    {
        private Services.Entities.OutsoleTCService OutsoleTC { get; }
        private Services.Entities.CompanyService Company { get; }

        public OutsoleTCService(
            Services.Entities.OutsoleTCService outsoleTCService,
            Services.Entities.CompanyService companyService,
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.OutsoleTC = outsoleTCService;
            this.Company = companyService;
        }
        public IQueryable<Models.Views.OutsoleTC> Get()
        {   
            return OutsoleTC.Get().Select(item => new Models.Views.OutsoleTC
            {
                Id = item.Id,
                OutsoleNo = item.OutsoleNo,
                TCType = item.TCType,
                CompanyId = item.CompanyId,
                Company = Company.Get().Where(i => i.Id == item.CompanyId).Select(i => i.CompanyNo).Max(),
                Date = item.Date,
                VendorShortNameTw = item.VendorShortNameTw,
                OrderNo = item.OrderNo,
                Qty = item.Qty,
                DollarName = item.DollarName,
                TC = item.TC,
                SubTC = item.SubTC,
                LocaleId = item.LocaleId,
                IsClose = item.IsClose == 1 ? true : false,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                Confirmer = item.Confirmer,
                ConfirmDate = item.ConfirmDate,
            });
        }

        public Models.Views.OutsoleTC Create(Models.Views.OutsoleTC item)
        {
            var _item = OutsoleTC.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.OutsoleTC Update(Models.Views.OutsoleTC item)
        {
            var _item = OutsoleTC.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.OutsoleTC item)
        {
            OutsoleTC.Remove(Build(item));
        }

        public void UpdateConfirm(int localeId, string confirmer, IEnumerable<decimal> confirmIds, IEnumerable<decimal> unConfirmIds)
        {
            // update shipiing Id = 0 ,remove shipping Id, umcloseed Only
            OutsoleTC.UpdateRange(
                i => confirmIds.Contains(i.Id) && i.LocaleId == localeId && i.IsClose == 0,
                // u => new Models.Entities.OutsoleTC { IsClose = 1, Confirmer = confirmer, ConfirmDate = DateTime.Now }
                u => u.SetProperty(p => p.IsClose, v => 1).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );

            // updat shipping id = paymentId where Id in ShipmentId, closed only
            OutsoleTC.UpdateRange(
                i => unConfirmIds.Contains(i.Id) && i.LocaleId == localeId && i.IsClose == 1,
                // u => new Models.Entities.OutsoleTC { IsClose = 0, Confirmer = confirmer, ConfirmDate = DateTime.Now }
                u => u.SetProperty(p => p.IsClose, v => 0).SetProperty(p => p.Confirmer, v => confirmer).SetProperty(p => p.ConfirmDate, v => DateTime.Now)
            );
        }

        private Models.Entities.OutsoleTC Build(Models.Views.OutsoleTC item)
        {
            return new Models.Entities.OutsoleTC()
            {
                Id = item.Id,
                OutsoleNo = item.OutsoleNo,
                TCType = item.TCType,
                CompanyId = item.CompanyId,
                Date = item.Date,
                VendorShortNameTw = item.VendorShortNameTw,
                OrderNo = item.OrderNo,
                Qty = item.Qty,
                DollarName = item.DollarName,
                TC = item.TC,
                SubTC = item.SubTC,
                LocaleId = item.LocaleId,
                IsClose = item.IsClose == true ? 1 : 0,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                Confirmer = item.Confirmer,
                ConfirmDate = item.ConfirmDate,
            };
        }
    }
}