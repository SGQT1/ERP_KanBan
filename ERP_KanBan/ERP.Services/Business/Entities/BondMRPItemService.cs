using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BondMRPItemService : BusinessService
    {
        private Services.Entities.BondMRPService BondMRP { get; }
        private Services.Entities.BondMRPItemService BondMRPItem { get; }

        public BondMRPItemService(
            Services.Entities.BondMRPService bondMRPService,
            Services.Entities.BondMRPItemService bondMRPItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            BondMRP = bondMRPService;
            BondMRPItem = bondMRPItemService;
        }
        public IQueryable<Models.Views.BondMRPItem> Get()
        {
            return BondMRPItem.Get().Select(i => new Models.Views.BondMRPItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                OrderNo = i.OrderNo,
                IsAdh = i.IsAdh,
                IsSub = i.IsSub,
                PartNo = i.PartNo,
                PartNameTw = i.PartNameTw,
                MaterialNameTw = i.MaterialNameTw,
                UnitNameTw = i.UnitNameTw,
                Total = i.Total,
                WeightEachUnit = i.WeightEachUnit,
                Weight = i.Weight,
                BondMaterialName = i.BondMaterialName,
                VendorShortNameTw = i.VendorShortNameTw,
                SeqNo = i.SeqNo,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                PurDollarNameTw = i.PurDollarNameTw,
                PayDollarNameTw = i.PayDollarNameTw,
                ExDollarNameTw = i.ExDollarNameTw,
            });
        }
        public void CreateRange(IEnumerable<Models.Views.BondMRPItem> items)
        {
            BondMRPItem.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.BondMRPItem> items)
        {
            BondMRPItem.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.BondMRPItem, bool>> predicate)
        {
            BondMRPItem.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.BondMRPItem> BuildRange(IEnumerable<Models.Views.BondMRPItem> items)
        {
            return items.Select(item => new Models.Entities.BondMRPItem()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                OrderNo = item.OrderNo,
                IsAdh = item.IsAdh,
                IsSub = item.IsSub,
                PartNo = item.PartNo,
                PartNameTw = item.PartNameTw,
                MaterialNameTw = item.MaterialNameTw,
                UnitNameTw = item.UnitNameTw,
                Total = item.Total,
                WeightEachUnit = item.WeightEachUnit,
                Weight = item.Weight,
                BondMaterialName = item.BondMaterialName,
                VendorShortNameTw = item.VendorShortNameTw,
                SeqNo = item.SeqNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PurDollarNameTw = item.PurDollarNameTw,
                PayDollarNameTw = item.PayDollarNameTw,
                ExDollarNameTw = item.ExDollarNameTw,
            });
        }
    }
}