using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MaterialQuotService : BusinessService
    {
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.MaterialQuotService MaterialQuot { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.VendorService Vendor { get; }
        public MaterialQuotService(
            Services.Entities.MaterialService materialService,
            Services.Entities.MaterialQuotService materialQuotService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.CompanyService companyService,
            Services.Entities.VendorService vendorService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Material = materialService;
            MaterialQuot = materialQuotService;
            CodeItem = codeItemService;
            Company = companyService;
            Vendor = vendorService;
        }
        public IQueryable<Models.Views.MaterialQuot> Get()
        {
            var result = (
                from m in Material.Get()
                join mq in MaterialQuot.Get() on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mq.MaterialId, LocaleId = mq.LocaleId }
                join v in Vendor.Get() on new { VendorId = mq.VendorId, LocaleId = mq.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.MaterialQuot
                {
                    Id = mq.Id,
                    LocaleId = mq.LocaleId,
                    Locale = Company.Get().Where(c => c.Id == mq.LocaleId).Select(i => i.CompanyNo).Max(),
                    MaterialId = mq.MaterialId,
                    MaterialName = m.MaterialName,
                    VendorId = mq.VendorId,
                    QuotDate = mq.QuotDate,
                    VendorQuotNo = mq.VendorQuotNo,
                    UnitCodeId = mq.UnitCodeId,
                    UnitCode = CodeItem.Get().Where(c => c.Id == mq.UnitCodeId && c.LocaleId == mq.LocaleId && c.CodeType == "21" ).Select(i => i.NameTW).Max(),
                    UnitPrice = mq.UnitPrice,
                    DollarCodeId = mq.DollarCodeId,
                    DollarCode = CodeItem.Get().Where(c => c.Id == mq.DollarCodeId && c.LocaleId == mq.LocaleId && c.CodeType == "02").Select(i => i.NameTW).Max(),
                    PayCodeId = mq.PayCodeId,
                    PayCode = mq.PayCodeId == 0 ? "Net 30" : mq.PayCodeId == 1 ? "Before T/T" : mq.PayCodeId == 2 ? "After T/T" : "None",
                    EffectiveDate = mq.EffectiveDate,
                    MinOrderQty = mq.MinOrderQty,
                    QuotType = mq.QuotType,
                    QuotationType = mq.QuotType == 1 ? "Production" : mq.QuotType == 2 ? "Sample" : "None",
                    ModifyUserName = mq.ModifyUserName,
                    LastUpdateTime = mq.LastUpdateTime,
                    // VendorShortNameTw = mq.VendorShortNameTw,
                    VendorShortNameTw = v.ShortNameTw,
                    ReferenceNo = mq.ReferenceNo,
                    ProcessMethod = mq.ProcessMethod,
                    ContractNo = mq.ContractNo,
                    Enable = mq.Enable == 1 ? 1 : 0,
                    CustomUnitCodeId = mq.CustomUnitCodeId,
                    CustomTransRate = mq.CustomTransRate,
                    Confirmed = mq.Confirmed,
                    CategoryNameTw = CodeItem.Get().Where(c => c.Id == m.CategoryCodeId && c.LocaleId == mq.LocaleId && c.CodeType == "11").Select(i => i.NameTW).Max(),
                });

            return result;
        }
        public IQueryable<Models.Views.MaterialQuot> GetMaterial()
        {
            return this.Get().Select(i => new Models.Views.MaterialQuot
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialId = i.MaterialId,
                MaterialName = i.MaterialName,
                VendorId = i.VendorId,
                VendorShortNameTw = i.VendorShortNameTw,
                CategoryNameTw = i.CategoryNameTw,
                QuotType = i.QuotType,
            }).Distinct();
        }
        public IQueryable<Models.Views.MaterialForQuot> GetMaterialForQuot(string predicate)
        {
            var result = (
                from m in Material.Get()
                join mq in MaterialQuot.Get() on new { MaterialId = m.Id, LocaleId = m.LocaleId } equals new { MaterialId = mq.MaterialId, LocaleId = mq.LocaleId } into mqGRP
                from mq in mqGRP.DefaultIfEmpty()
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MaterialName = m.MaterialName,
                    SamplingMethod = m.SamplingMethod,
                    CategoryCodeId = m.CategoryCodeId,
                    SemiGoods = m.SemiGoods,
                    VendorId = mq.VendorId,
                    VendorShortNameTw = mq.VendorShortNameTw,
                    TextureCodeId = m.TextureCodeId,
                    HasQuot = mq == null ? 0 : 1,
                    QuotType = mq.QuotType
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.MaterialForQuot {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MaterialName = i.MaterialName,
                SamplingMethod = i.SamplingMethod,
                CategoryCodeId = i.CategoryCodeId,
                SemiGoods = i.SemiGoods,
                TextureCodeId = i.TextureCodeId,
                HasQuot = i.HasQuot
            })
            .Distinct();

            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.MaterialQuot> items)
        {
            MaterialQuot.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MaterialQuot, bool>> predicate)
        {
            MaterialQuot.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MaterialQuot> BuildRange(IEnumerable<Models.Views.MaterialQuot> items)
        {
            return items.Select(item => new ERP.Models.Entities.MaterialQuot
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MaterialId = item.MaterialId,
                VendorId = item.VendorId,
                QuotDate = item.QuotDate,
                VendorQuotNo = item.VendorQuotNo,
                UnitCodeId = item.UnitCodeId,
                UnitPrice = item.UnitPrice,
                DollarCodeId = item.DollarCodeId,
                PayCodeId = item.PayCodeId,
                EffectiveDate = item.EffectiveDate,
                MinOrderQty = item.MinOrderQty,
                QuotType = item.QuotType,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                VendorShortNameTw = item.VendorShortNameTw,
                ReferenceNo = item.ReferenceNo,
                ProcessMethod = item.ProcessMethod,
                ContractNo = item.ContractNo,
                Enable = item.Enable,
                CustomUnitCodeId = item.CustomUnitCodeId,
                CustomTransRate = item.CustomTransRate,
                Confirmed = item.Confirmed,
            });
        }
    }
}