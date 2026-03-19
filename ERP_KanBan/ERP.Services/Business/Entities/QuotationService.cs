using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Models;
using Diamond.DataSource.Extensions;

using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class QuotationService : BusinessService
    {
        private Services.Entities.QuotationService Quotation { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.CompanyService Company { get; }
        private Services.Entities.OutsoleService Outsole { get; }
        private Services.Entities.LastService Last { get; }
        private Services.Business.Entities.TypeService Type { get; }
        public QuotationService(
            Services.Entities.QuotationService quotationService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.CompanyService companyService,
            Services.Entities.OutsoleService outsoleService,
            Services.Entities.LastService lastService,
            Services.Business.Entities.TypeService typeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            this.Quotation = quotationService;
            this.CodeItem = codeItemService;
            this.Company = companyService;
            this.Outsole = outsoleService;
            this.Last = lastService;
            this.Type = typeService;
        }
        public IQueryable<Models.Views.Quotation> Get()
        {
            return Query();
        }
        public IQueryable<Models.Views.Quotation> Get(Expression<Func<Models.Views.Quotation, bool>> predicate)
        {
            var quotaions = Query().Where(predicate).ToList();
            return TypeMap(quotaions);
        }
        public IQueryable<Models.Views.Quotation> Get(string predicate)
        {
            var quotaions = Query().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate).ToList();
            return TypeMap(quotaions);
        }

        private IQueryable<Models.Views.Quotation> TypeMap(List<Models.Views.Quotation> quotations)
        {
            quotations.ForEach(i =>
            {
                i.ProductType = Type.GetProductType().Where(t => t.Id == i.ProductTypeId).Select(t => t.NameEn).Max();
                i.ShipmentType = Type.GetShipmentType().Where(t => t.Id == i.ShipmentTypeId).Select(t => t.NameEn).Max();
            });
            return quotations.AsQueryable();
        }
        private IQueryable<Models.Views.Quotation> Query()
        {
            var quotation = (
                from q in Quotation.Get()
                join c in Company.Get() on new { CompanyId = q.CompanyId } equals new { CompanyId = (int?)c.Id } into cGrp
                from c in cGrp.DefaultIfEmpty()
                join b in CodeItem.Get().Where(i => i.CodeType == "25") on new { LocaleId = q.LocaleId, BrandCodeId = q.BrandCodeId } equals new { LocaleId = b.LocaleId, BrandCodeId = b.Id } into bGrp
                from b in bGrp.DefaultIfEmpty()
                join s in CodeItem.Get().Where(i => i.CodeType == "35") on new { LocaleId = q.LocaleId, SizeCountryId = q.SizeCountryCodeId } equals new { LocaleId = s.LocaleId, SizeCountryId = (decimal?)s.Id } into sGrp
                from s in sGrp.DefaultIfEmpty()
                join d in CodeItem.Get() on new { LocaleId = q.LocaleId, DollarCodeId = q.DollarCodeId } equals new { LocaleId = d.LocaleId, DollarCodeId = d.Id } into dGrp
                from d in dGrp.DefaultIfEmpty()
                select new Models.Views.Quotation
                {
                    Id = q.Id,
                    LocaleId = q.LocaleId,
                    CompanyId = q.CompanyId,
                    ArticleNo = q.ArticleNo,
                    StyleNo = q.StyleNo,
                    ShoeName = q.ShoeName,
                    ProductTypeId = q.ProductType,
                    ShipmentTypeId = q.PortType,
                    LastId = q.LastId,
                    BrandCodeId = q.BrandCodeId,
                    ExchangeRate = q.ExchangeRate,
                    DollarCodeId = q.DollarCodeId,
                    Confirmed = q.Confirmed,
                    QuoteDate = q.QuoteDate,
                    EffectiveDate = q.EffectiveDate,
                    SizeCountryCodeId = q.SizeCountryCodeId,
                    DisplaySizeBeginning = q.SizeBeginning.ToString() + (q.SizeBeginningSuffix == null ? "" : q.SizeBeginningSuffix),
                    SizeBeginning = q.SizeBeginning,
                    SizeBeginningSuffix = q.SizeBeginningSuffix,
                    SizeBeginningInner = q.SizeBeginningInner,
                    DisplaySizeEndding = q.SizeEndding.ToString() + (q.SizeEnddingSuffix == null ? " " : q.SizeEnddingSuffix),
                    SizeEndding = q.SizeEndding,
                    SizeEnddingSuffix = q.SizeEnddingSuffix,
                    SizeEnddingInner = q.SizeEnddingInner,
                    FactoryPriceIntel = q.FactoryPriceIntel,
                    InvoicePriceIntel = q.InvoicePriceIntel,
                    FactoryPriceSub = q.FactoryPriceSub,
                    InvoicePriceSub = q.InvoicePriceSub,
                    Remark = q.Remark,
                    ModifyUserName = q.ModifyUserName,
                    LastUpdateTime = q.LastUpdateTime,

                    ReferFileURL = q.ReferFileURL,
                    ProductClass = q.ProductClass,
                    TargetOutput = q.TargetOutput,
                    StopOrder = q.StopOrder,
                    IsLimitedQty = q.IsLimitedQty, //MOQ Type
                    LimitedQty = q.LimitedQty, //MOQ
                    CLB = q.CLB,
                    IsForSeason = q.IsForSeason,
                    Season = q.Season,
                    CBSId = q.CBSId,
                    ToolFundIntel = q.ToolFundIntel,
                    ToolFundSub = q.ToolFundSub,
                    MidsolePrice = q.MidsolePrice,
                    OutsolePrice = q.OutsolePrice,
                    ToolingOtherPrice = q.ToolingOtherPrice,
                    ToolingTotalPrice = q.ToolingTotalPrice,
                    LastNo = q.LastNo,
                    MidsoleNo = q.MidsoleNo,
                    OutsoleId = q.OutsoleId,
                    OutsoleNo = q.OutsoleNo,

                    CompanyNo = c.CompanyNo,
                    Brand = b.NameTW,
                    // RefLast = l.LastNo,
                    // RefOutsole = o.OutsoleNo,
                    ArticleSize = s.NameTW,
                    Dollar = d.NameTW,
                    // ProductType = Type.GetProductType().Where(i => i.Id == q.ProductType).Select(i => i.NameTw).Max(),
                    // ShipmentType = Type.GetShipmentType().Where(i => i.Id == q.PortType).Select(i => i.NameTw).Max(),
                }
            );
            return quotation;
        }
        public Models.Views.Quotation Create(Models.Views.Quotation item)
        {
            var _item = Quotation.Create(Build(item));
            return Get(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Quotation Update(Models.Views.Quotation item)
        {
            var _item = Quotation.Update(Build(item));
            return Get(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Quotation item)
        {
            Quotation.Remove(Build(item));
        }
        private Models.Entities.Quotation Build(Models.Views.Quotation item)
        {

            return new Models.Entities.Quotation()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CompanyId = item.CompanyId,
                ArticleNo = item.ArticleNo,
                StyleNo = item.StyleNo,
                ShoeName = item.ShoeName,
                ProductType = item.ProductTypeId,
                PortType = item.ShipmentTypeId,
                // LastId = item.LastId,
                BrandCodeId = item.BrandCodeId,
                ExchangeRate = item.ExchangeRate,
                DollarCodeId = item.DollarCodeId,
                Confirmed = item.Confirmed,
                QuoteDate = item.QuoteDate,
                EffectiveDate = item.EffectiveDate,
                SizeCountryCodeId = item.SizeCountryCodeId,
                SizeBeginning = item.SizeBeginning,
                SizeBeginningSuffix = (item.SizeBeginningSuffix == null || item.SizeBeginningSuffix.Trim().Length == 0) ? item.SizeBeginningSuffix : item.SizeBeginningSuffix.Trim(),
                SizeBeginningInner = (item.SizeBeginningSuffix == null || item.SizeBeginningSuffix.Trim().Length == 0) ? (item.SizeBeginning * 1000) : (item.SizeBeginning * 1),

                SizeEndding = item.SizeEndding,
                SizeEnddingSuffix = (item.SizeEnddingSuffix == null || item.SizeEnddingSuffix.Trim().Length == 0) ? item.SizeEnddingSuffix : item.SizeEnddingSuffix.Trim(),
                SizeEnddingInner = (item.SizeEnddingSuffix == null || item.SizeEnddingSuffix.Trim().Length == 0) ? (item.SizeEndding * 1000) : (item.SizeEndding * 1),

                FactoryPriceIntel = item.FactoryPriceIntel,
                InvoicePriceIntel = item.InvoicePriceIntel,
                FactoryPriceSub = item.FactoryPriceSub,
                InvoicePriceSub = item.InvoicePriceSub,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

                ReferFileURL = item.ReferFileURL,
                ProductClass = item.ProductClass,
                TargetOutput = item.TargetOutput,
                StopOrder = item.StopOrder,
                IsLimitedQty = item.IsLimitedQty, //MOQ Type
                LimitedQty = item.LimitedQty, //MOQ
                CLB = item.CLB,
                IsForSeason = item.IsForSeason,
                Season = item.Season,
                CBSId = item.CBSId,
                ToolFundIntel = item.ToolFundIntel,
                ToolFundSub = item.ToolFundSub,
                MidsoleNo = item.MidsoleNo,
                // OutsoleId = item.OutsoleId,
                MidsolePrice = item.MidsolePrice,
                OutsolePrice = item.OutsolePrice,
                ToolingOtherPrice = item.ToolingOtherPrice,
                ToolingTotalPrice = item.ToolingTotalPrice,
                LastNo = item.LastNo,
                OutsoleNo = item.OutsoleNo,
                Dollar = CodeItem.Get().Where(i => i.Id == item.DollarCodeId && i.LocaleId == item.LocaleId && i.CodeType == "02").Select(i => i.NameTW).Max()
            };
        }

        public void CreateRange(List<Models.Views.Quotation> items)
        {
            Quotation.CreateRange(BuildRange(items));
        }
        public void RemoveRange(List<Models.Views.Quotation> items)
        {
            var localeId = items.Max(i => i.LocaleId);
            var ids = items.Select(i => i.Id);
            Quotation.RemoveRange(i => ids.Contains(i.Id) && i.LocaleId == localeId);
        }
        private IEnumerable<Models.Entities.Quotation> BuildRange(List<Models.Views.Quotation> items)
        {
            return items.Select(item => new Models.Entities.Quotation()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                
                ArticleNo = item.ArticleNo,
                StyleNo = item.StyleNo,
                ShoeName = item.ShoeName,

                // LastId = item.LastId,

                ExchangeRate = item.ExchangeRate,
                Confirmed = item.Confirmed,
                QuoteDate = item.QuoteDate,
                EffectiveDate = item.EffectiveDate,

                SizeBeginning = item.SizeBeginning,
                SizeBeginningSuffix = (item.SizeBeginningSuffix == null || item.SizeBeginningSuffix.Trim().Length == 0) ? item.SizeBeginningSuffix : item.SizeBeginningSuffix.Trim(),
                SizeBeginningInner = (item.SizeBeginningSuffix == null || item.SizeBeginningSuffix.Trim().Length == 0) ? (item.SizeBeginning * 1000) : (item.SizeBeginning * 1),

                SizeEndding = item.SizeEndding,
                SizeEnddingSuffix = (item.SizeEnddingSuffix == null || item.SizeEnddingSuffix.Trim().Length == 0) ? item.SizeEnddingSuffix : item.SizeEnddingSuffix.Trim(),
                SizeEnddingInner = (item.SizeEnddingSuffix == null || item.SizeEnddingSuffix.Trim().Length == 0) ? (item.SizeEndding * 1000) : (item.SizeEndding * 1),

                FactoryPriceIntel = item.FactoryPriceIntel,
                InvoicePriceIntel = item.InvoicePriceIntel,
                FactoryPriceSub = item.FactoryPriceSub,
                InvoicePriceSub = item.InvoicePriceSub,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,

                ReferFileURL = item.ReferFileURL,
                ProductClass = item.ProductClass,
                TargetOutput = item.TargetOutput,
                StopOrder = item.StopOrder,
                IsLimitedQty = item.IsLimitedQty, //MOQ Type
                LimitedQty = item.LimitedQty, //MOQ
                CLB = item.CLB,
                IsForSeason = item.IsForSeason,
                Season = item.Season,
                CBSId = item.CBSId,
                ToolFundIntel = item.ToolFundIntel,
                ToolFundSub = item.ToolFundSub,
                MidsoleNo = item.MidsoleNo,
                MidsolePrice = item.MidsolePrice,
                OutsolePrice = item.OutsolePrice,
                ToolingOtherPrice = item.ToolingOtherPrice,
                ToolingTotalPrice = item.ToolingTotalPrice,
                LastNo = item.LastNo,
                OutsoleNo = item.OutsoleNo,
                Dollar = item.Dollar,
                DollarCodeId = CodeItem.Get().Where(i => i.NameTW == item.Dollar && i.LocaleId == item.LocaleId && i.CodeType == "02").Select(i => i.Id).Max(),
                BrandCodeId = CodeItem.Get().Where(i => i.NameTW == item.Brand && i.LocaleId == item.LocaleId && i.CodeType == "25").Select(i => i.Id).Max(),
                SizeCountryCodeId = CodeItem.Get().Where(i => i.NameTW == item.ArticleSize && i.LocaleId == item.LocaleId && i.CodeType == "35").Select(i => i.Id).Max(),
                CompanyId = (int?)Company.Get().Where(i => i.CompanyNo == item.CompanyNo).Select(i => i.Id).Max(),
                ProductType = (int)Type.GetProductType().Where(t => t.NameEn == item.ProductType).Select(t => t.Id).Max(),
                PortType = Type.GetShipmentType().Where(t => t.NameEn == item.ShipmentType).Select(t => t.Id).Max(),
                // DollarCodeId = 0,
                // BrandCodeId = 0,
                // SizeCountryCodeId = 0,
                // CompanyId = 0,
                // ProductType = 0,
                // PortType = 0,
            });
        }
    }
}