using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackLabelEditionService : BusinessService
    {
        private Services.Entities.OrdersPLService OrdersPL { get; }
        private Services.Entities.CTNOrdersService CTNOrders { get; }
        public PackLabelEditionService(
            Services.Entities.OrdersPLService ordersPLService,
            Services.Entities.CTNOrdersService ctnOrdersService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            OrdersPL = ordersPLService;
            CTNOrders = ctnOrdersService;
        }
        public IEnumerable<Models.Views.PackLabelEdition> Get(int packLabelId, string orderNo, int localeId)
        {
            return (
                from pl in OrdersPL.Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId)
                join e in CTNOrders.Get().Where(i => i.CTNLabelId == packLabelId && i.LocaleId == localeId) on new { pl.LocaleId, pl.OrderNo, pl.Edition } equals new { e.LocaleId, e.OrderNo, e.Edition } into eGrp
                from e in eGrp.DefaultIfEmpty()
                select new Models.Views.PackLabelEdition
                {
                    Id = e == null ? 0 : e.Id,
                    LocaleId = pl.LocaleId,
                    PackLabelId = packLabelId,
                    OrderNo = pl.OrderNo,
                    Edition = pl.Edition,
                    IsEdition = e == null ? false : true
                }
            ).ToList();
        }

        public void CreateRange(IEnumerable<Models.Views.PackLabelEdition> packLabelEdtions)
        {
            CTNOrders.CreateRange(BuildRange(packLabelEdtions));
        }
        public void RemoveRange(int packLabelId, int localeId)
        {
            CTNOrders.RemoveRange(i => i.CTNLabelId == packLabelId && i.LocaleId == localeId);
        }

        public IEnumerable<Models.Entities.CTNLabelItem> BuildRange(IEnumerable<Models.Views.PackLabelItem> packLabelItems)
        {
            return packLabelItems.Select(item => new Models.Entities.CTNLabelItem
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CTNLabelId = item.CTNLabelId,
                GroupBy = item.GroupBy,
                PackingType = item.PackingType,
                LabelCode = item.LabelCode,
                MinRefDisplaySize = item.MinRefDisplaySize,
                MaxRefDisplaySize = item.MaxRefDisplaySize,
                SubPackingQty = item.SubPackingQty,
                SubNetWeight = item.SubNetWeight,
                SubGrossWeight = item.SubGrossWeight,
                SubMEAS = item.SubMEAS,
                SubCBM = item.SubCBM,
                SeqNo = item.SeqNo,
                SubLabelCode = item.SubLabelCode,
                DeptNo = item.DeptNo,
            });
        }

        public IEnumerable<Models.Entities.CTNOrders> BuildRange(IEnumerable<Models.Views.PackLabelEdition> packLabelEdtions)
        {
            return packLabelEdtions.Select(item => new Models.Entities.CTNOrders
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CTNLabelId = item.PackLabelId,
                OrderNo = item.OrderNo,
                Edition = item.Edition,
            });
        }
    }
}