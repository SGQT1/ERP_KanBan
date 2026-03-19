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
    public class OutsoleToolingService : BusinessService
    {
        private Services.Entities.OutsoleService Outsole { get; }

        public OutsoleToolingService(
            Services.Entities.OutsoleService outsoleService, 
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.Outsole = outsoleService;
        }
        public IQueryable<Models.Views.OutsoleTooling> Get()
        {
            return Outsole.Get().Select(item => new Models.Views.OutsoleTooling
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MoneyCodeId = item.MoneyCodeId,
                VendorId = item.VendorId,
                OutsoleNo = item.OutsoleNo,
                OutsoleDesc = item.OtherDesc,
                MDVendorId = item.MDVendorId,
                MDNo = item.MDNo,
                MDDesc = item.MDDesc,
                EVANo = item.EVANo,
                EVAVendorId = item.EVAVendorId,
                EVADesc = item.EVADesc,
                TCQty = item.TCQty,
                TC = item.TC,
                TCTTL = item.TCTTL,
                MDQty = item.MDQty,
                MDTC = item.MDTC,
                MDTCTTL = item.MDTCTTL,
                EVAQty = item.EVAQty,
                EVATC = item.EVATC,
                EVATCTTL = item.EVATCTTL,
                CBDTC = item.CBDTC,
                CBDMDTC = item.CBDMDTC,
                CBDEVATC = item.CBDEVATC,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
        public Models.Views.OutsoleTooling Create(Models.Views.OutsoleTooling item)
        {
            var _item = Outsole.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.OutsoleTooling Update(Models.Views.OutsoleTooling item)
        {
            var _item = Outsole.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.OutsoleTooling item)
        {
            Outsole.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Outsole Build(Models.Views.OutsoleTooling item)
        {
            return new Models.Entities.Outsole()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MoneyCodeId = item.MoneyCodeId,
                VendorId = item.VendorId,
                OutsoleNo = item.OutsoleNo,
                OtherDesc = item.OutsoleDesc,
                MDVendorId = item.MDVendorId,
                MDNo = item.MDNo,
                MDDesc = item.MDDesc,
                EVANo = item.EVANo,
                EVAVendorId = item.EVAVendorId,
                EVADesc = item.EVADesc,
                TCQty = item.TCQty,
                TC = item.TC,
                TCTTL = item.TCTTL,
                MDQty = item.MDQty,
                MDTC = item.MDTC,
                MDTCTTL = item.MDTCTTL,
                EVAQty = item.EVAQty,
                EVATC = item.EVATC,
                EVATCTTL = item.EVATCTTL,
                CBDTC = item.CBDTC,
                CBDMDTC = item.CBDMDTC,
                CBDEVATC = item.CBDEVATC,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }
    }
}