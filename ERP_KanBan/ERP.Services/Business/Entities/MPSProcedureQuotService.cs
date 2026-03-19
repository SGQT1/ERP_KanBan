using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSProcedureQuotService : BusinessService
    {
        private ERP.Services.Entities.MpsProcedureQuotService MPSProcedureQuot { get; set; }
        private ERP.Services.Entities.MpsProcedureGroupService MPSProcedureGroup { get; set; }
        private ERP.Services.Entities.MpsProcedureVendorService MPSProcedureVendor { get; set; }

        public MPSProcedureQuotService(
            ERP.Services.Entities.MpsProcedureQuotService mpsProcedureQuotService,
            ERP.Services.Entities.MpsProcedureGroupService mpsProcedureGroupService,
            ERP.Services.Entities.MpsProcedureVendorService mpsProcedureVendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSProcedureQuot = mpsProcedureQuotService;
            MPSProcedureGroup = mpsProcedureGroupService;
            MPSProcedureVendor = mpsProcedureVendorService;
        }

        public IQueryable<Models.Views.MPSOutsourceForQuot> GetMPSOutsourceForQuot(string predicate)
        {
            var result = (
                from g in MPSProcedureGroup.Get()
                join q in MPSProcedureQuot.Get() on new { MPSProcedureGroupId = g.Id, LocaleId = g.LocaleId } equals new { MPSProcedureGroupId = q.MpsProcedureGroupId, LocaleId = q.LocaleId } into qGPR
                from q in qGPR.DefaultIfEmpty()
                select new
                {
                    Id = g.Id,
                    LocaleId = g.LocaleId,
                    StyleNo = g.StyleNo,
                    ProcedureGroup = g.GroupNameTw,
                    GroupNameLocal = g.GroupNameLocal,
                    GroupNameEn = g.GroupNameEn,
                    DollarNameTw = g.DollarNameTw,
                    UnitStandardTime = g.UnitStandardTime,
                    UnitLaborCost = g.UnitLaborCost,
                    VendorId = q.MpsProcedureVendorId,
                    HasQuot = q == null ? 0 : 1,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new Models.Views.MPSOutsourceForQuot
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                StyleNo = i.StyleNo,
                GroupNameTw = i.ProcedureGroup,
                GroupNameLocal = i.GroupNameLocal,
                GroupNameEn = i.GroupNameEn,
                DollarNameTw = i.DollarNameTw,
                UnitStandardTime = i.UnitStandardTime,
                UnitLaborCost = i.UnitLaborCost,
                HasQuot = i.HasQuot,
            })
            .Distinct();

            return result;
        }

        public IQueryable<Models.Views.MPSProcedureQuot> Get()
        {
            var result = (
                from g in MPSProcedureGroup.Get()
                join q in MPSProcedureQuot.Get() on new { MPSProcedureGroupId = g.Id, LocaleId = g.LocaleId } equals new { MPSProcedureGroupId = q.MpsProcedureGroupId, LocaleId = q.LocaleId }
                join v in MPSProcedureVendor.Get() on new { MPSProcedureVendorId = q.MpsProcedureVendorId, LocaleId = q.LocaleId } equals new { MPSProcedureVendorId = v.Id, LocaleId = v.LocaleId }
                select new Models.Views.MPSProcedureQuot
                {
                    Id = q.Id,
                    LocaleId = q.LocaleId,
                    MpsProcedureGroupId = q.MpsProcedureGroupId,
                    MpsProcedureVendorId = q.MpsProcedureVendorId,
                    StyleNo = q.StyleNo,
                    QuotDate = q.QuotDate,
                    VendorQuotNo = q.VendorQuotNo,
                    PurUnitNameTw = q.PurUnitNameTw,
                    UnitPrice = q.UnitPrice,
                    DollarNameTw = q.DollarNameTw,
                    PayCodeId = q.PayCodeId,
                    EffectiveDate = q.EffectiveDate,
                    ModifyUserName = q.ModifyUserName,
                    LastUpdateTime = q.LastUpdateTime,
                    VendorShortNameTw = q.VendorShortNameTw,
                    VendorNameLocal = v.NameLocal,
                    VendorNameTw = v.NameTw,

                    ReferenceNo = q.ReferenceNo,
                    ProcessMethod = q.ProcessMethod,
                    ContractNo = q.ContractNo,
                    // Enable = q.Enable,
                    Enable = q.Enable == 1 ? 1 : 0,
                    CustomUnitNameTw = q.CustomUnitNameTw,
                    CustomTransRate = q.CustomTransRate,
                    Confirmed = q.Confirmed,
                    MpsProcedureGroupName = g.GroupNameLocal,
                    PaymentCodeId = (decimal)v.PaymentCodeId,
                }
            );
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.MPSProcedureQuot> items)
        {
            MPSProcedureQuot.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcedureQuot, bool>> predicate)
        {
            MPSProcedureQuot.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedureQuot> BuildRange(IEnumerable<Models.Views.MPSProcedureQuot> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedureQuot
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsProcedureGroupId = item.MpsProcedureGroupId,
                MpsProcedureVendorId = item.MpsProcedureVendorId,
                StyleNo = item.StyleNo,
                QuotDate = item.QuotDate,
                VendorQuotNo = item.VendorQuotNo,
                PurUnitNameTw = item.PurUnitNameTw,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                PayCodeId = item.PayCodeId,
                EffectiveDate = item.EffectiveDate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                VendorShortNameTw = item.VendorShortNameTw,
                ReferenceNo = item.ReferenceNo,
                ProcessMethod = item.ProcessMethod,
                ContractNo = item.ContractNo,
                Enable = item.Enable,
                CustomUnitNameTw = item.CustomUnitNameTw,
                CustomTransRate = item.CustomTransRate,
                Confirmed = item.Confirmed,
            });
        }
        
        public Models.Views.MPSProcedureQuot Create(Models.Views.MPSProcedureQuot item)
        {
            var _item = MPSProcedureQuot.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSProcedureQuot Update(Models.Views.MPSProcedureQuot item)
        {
            var _item = MPSProcedureQuot.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSProcedureQuot item)
        {
            MPSProcedureQuot.Remove(Build(item));
        }
        private Models.Entities.MpsProcedureQuot Build(Models.Views.MPSProcedureQuot item)
        {
            return new Models.Entities.MpsProcedureQuot()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsProcedureGroupId = item.MpsProcedureGroupId,
                MpsProcedureVendorId = item.MpsProcedureVendorId,
                StyleNo = item.StyleNo,
                QuotDate = item.QuotDate,
                VendorQuotNo = item.VendorQuotNo,
                PurUnitNameTw = item.PurUnitNameTw,
                UnitPrice = item.UnitPrice,
                DollarNameTw = item.DollarNameTw,
                PayCodeId = item.PayCodeId,
                EffectiveDate = item.EffectiveDate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                VendorShortNameTw = item.VendorShortNameTw,
                ReferenceNo = item.ReferenceNo,
                ProcessMethod = item.ProcessMethod,
                // ContractNo = item.ContractNo,
                ContractNo = "dm_admin",
                // Enable = item.Enable,
                Enable = item.Enable == 1 ? 1 : 2,
                CustomUnitNameTw = item.CustomUnitNameTw,
                CustomTransRate = item.CustomTransRate,
                Confirmed = item.Confirmed,
            };
        }
    }
}
