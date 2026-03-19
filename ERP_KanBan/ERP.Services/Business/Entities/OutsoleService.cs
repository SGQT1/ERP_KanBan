using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OutsoleService : BusinessService
    {
        private Services.Entities.OutsoleService Outsole { get; }
        private Services.Entities.CustomerService Customer { get; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public OutsoleService(
            Services.Entities.OutsoleService outsoleService,
            Services.Entities.CustomerService customerService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Outsole = outsoleService;
            Customer = customerService;
            CodeItem = codeItemService;
        }
        public IQueryable<Models.Views.Outsole> Get()
        {
            return Outsole.Get().Select(item => new Models.Views.Outsole
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
                TCQty = item.TCQty ?? 0,
                TC = item.TC ?? 0,
                TCTTL = item.TCTTL ?? 0,
                MDQty = item.MDQty ?? 0,
                MDTC = item.MDTC ?? 0,
                MDTCTTL = item.MDTCTTL ?? 0,
                EVAQty = item.EVAQty ?? 0,
                EVATC = item.EVATC ?? 0,
                EVATCTTL = item.EVATCTTL ?? 0,
                CBDTC = item.CBDTC ?? 0,
                CBDMDTC = item.CBDMDTC ?? 0,
                CBDEVATC = item.CBDEVATC ?? 0,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }


        public IQueryable<Models.Views.Outsole> GetCopyOutsole(string predicate, int localeId)
        {
            var notIn = Outsole.Get().Where(i => i.LocaleId == localeId).Select(i => i.OutsoleNo).Distinct();
            var vendor = Outsole.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Where(i => !notIn.Contains(i.OutsoleNo))
                .Select(i => new Models.Views.Outsole
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    MoneyCodeId = i.MoneyCodeId,
                    VendorId = i.VendorId,
                    OutsoleNo = i.OutsoleNo,
                    OutsoleDesc = i.OtherDesc,
                    MDVendorId = i.MDVendorId,
                    MDNo = i.MDNo,
                    MDDesc = i.MDDesc,
                    EVANo = i.EVANo,
                    EVAVendorId = i.EVAVendorId,
                    EVADesc = i.EVADesc,
                    TCQty = i.TCQty ?? 0,
                    TC = i.TC ?? 0,
                    TCTTL = i.TCTTL ?? 0,
                    MDQty = i.MDQty ?? 0,
                    MDTC = i.MDTC ?? 0,
                    MDTCTTL = i.MDTCTTL ?? 0,
                    EVAQty = i.EVAQty ?? 0,
                    EVATC = i.EVATC ?? 0,
                    EVATCTTL = i.EVATCTTL ?? 0,
                    CBDTC = i.CBDTC ?? 0,
                    CBDMDTC = i.CBDMDTC ?? 0,
                    CBDEVATC = i.CBDEVATC ?? 0,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    OwnerCustomer = Customer.Get().Where(c => c.LocaleId == i.LocaleId && c.Id == i.OwnerCustomerId).Max(c => c.ChineseName),
                    MoneyCode = CodeItem.Get().Where(c => c.LocaleId == i.LocaleId && c.Id == i.MoneyCodeId && c.CodeType == "02").Max(c => c.NameTW),
                })
                .ToList()
                .AsQueryable();
            return vendor;
        }

        public Models.Views.Outsole Create(Models.Views.Outsole item)
        {
            var _item = Outsole.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Outsole Update(Models.Views.Outsole item)
        {
            var _item = Outsole.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Outsole item)
        {
            Outsole.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Outsole Build(Models.Views.Outsole item)
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