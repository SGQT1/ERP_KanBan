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
    public class BondMRPService : BusinessService
    {

        private ERP.Services.Business.Entities.BondMRPService BondMRP { get; set; }
        private ERP.Services.Business.Entities.BondMRPItemService BondMRPItem { get; set; }

        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Entities.MRPItemOrdersService MRPItemOrders { get; set; }
        private ERP.Services.Entities.BondMaterialChinaContrastService BondMaterialChinaContrast { get; set; }
        private ERP.Services.Entities.BondProductChinaContrastService BondProductChinaContrast { get; set; }


        private ERP.Services.Entities.APMonthItemService APMonthItem { get; set; }
        private ERP.Services.Entities.APMonthService APMonth { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }

        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.CodeItemService CodeItem { get; set; }

        public BondMRPService(
            ERP.Services.Business.Entities.BondMRPService bondMRPService,
            ERP.Services.Business.Entities.BondMRPItemService bondMRPItemService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MRPItemOrdersService mrpItemOrdersService,
            ERP.Services.Entities.BondMaterialChinaContrastService bondMaterialChinaContrastService,
            ERP.Services.Entities.BondProductChinaContrastService bondProductChinaContrastService,
            ERP.Services.Entities.APMonthItemService apMonthItemService,
            ERP.Services.Entities.APMonthService apMonthService,
            ERP.Services.Entities.VendorService vendorService,

            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            BondMRP = bondMRPService;
            BondMRPItem = bondMRPItemService;

            Orders = ordersService;
            Material = materialService;
            MRPItem = mrpItemService;
            MRPItemOrders = mrpItemOrdersService;
            BondMaterialChinaContrast = bondMaterialChinaContrastService;
            BondProductChinaContrast = bondProductChinaContrastService;

            APMonthItem = apMonthItemService;
            APMonth = apMonthService;
            Vendor = vendorService;

            POItem = poItemService;
            PO = poService;
            CodeItem = codeItemService;
        }

        public IQueryable<Models.Views.BondMRP> Get()
        {
            return BondMRP.GetWithItem();
        }
        public ERP.Models.Views.BondMRP Get(int id, int localeId)
        {
            return BondMRP.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public void Save(List<BondMRP> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                UnitOfWork.BeginTransaction();
                if (items.Count() > 0)
                {
                    var addItems = items.Where(i => i.Id == 0 || i.Id == null).ToList();
                    var updateItems = items.Where(i => i.Id > 0).ToList();

                    BondMRP.CreateRange(addItems);
                    BondMRP.UpdateRange(updateItems);
                    UnitOfWork.Commit();
                }
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(List<decimal> ids, int localeId)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                BondMRP.RemoveRange(i => ids.Contains(i.Id) && i.LocaleId == localeId);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public ERP.Models.Views.BondMRPGroup GetGroup(int id, int localeId)
        {
            var group = new ERP.Models.Views.BondMRPGroup { };

            group.BondMRP = Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();

            if (group.BondMRP != null)
            {
                group.BondMRPItem = BondMRPItem.Get().Where(i => i.OrderNo == group.BondMRP.OrderNo && i.LocaleId == group.BondMRP.LocaleId).OrderBy(i => i.SeqNo).ToList();
            }

            return group;
        }
        public ERP.Models.Views.BondMRPGroup GetGroupByOrderNo(string orderNo, int localeId)
        {
            var group = new ERP.Models.Views.BondMRPGroup { };

            group.BondMRP = Get().Where(i => i.OrderNo == orderNo && i.LocaleId == localeId).FirstOrDefault();

            if (group.BondMRP != null)
            {
                group.BondMRPItem = BondMRPItem.Get().Where(i => i.OrderNo == group.BondMRP.OrderNo && i.LocaleId == group.BondMRP.LocaleId).OrderBy(i => i.SeqNo).ToList();
            }

            return group;
        }
        public ERP.Models.Views.BondMRPGroup SaveGroup(ERP.Models.Views.BondMRPGroup group)
        {
            var bondMRP = group.BondMRP;
            var bondMRPItem = group.BondMRPItem.ToList();

            if (bondMRP != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Vendor
                    {
                        var _bondMRP = BondMRP.Get().Where(i => i.LocaleId == bondMRP.LocaleId && i.Id == bondMRP.Id).FirstOrDefault();

                        if (_bondMRP != null)
                        {
                            bondMRP.Id = _bondMRP.Id;
                            bondMRP.LocaleId = _bondMRP.LocaleId;
                            bondMRP = BondMRP.Update(bondMRP);
                        }
                        else
                        {
                            bondMRP = BondMRP.Create(bondMRP);
                        }
                    }

                    //Vendor Item
                    {
                        if (bondMRP.Id != 0)
                        {
                            var _bondMRPItem = group.BondMRPItem.OrderBy(i => i.SeqNo).ToList();

                            BondMRPItem.RemoveRange(i => i.OrderNo == bondMRP.OrderNo && i.LocaleId == bondMRP.LocaleId);
                            BondMRPItem.CreateRange(group.BondMRPItem);
                        }
                    }
                    UnitOfWork.Commit();
                    return GetGroup((int)bondMRP.Id, (int)bondMRP.LocaleId);

                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }

            return group;
            // var bondMRP = BondMRP.Get().Where(i => i.Id == group.BondMRP.Id && i.LocaleId == group.BondMRP.LocaleId).FirstOrDefault();

            // if(bondMRP == null) {
            //     bondMRP = BondMRP.Create(group.BondMRP);
            // } else {
            //     group.BondMRP.Id = bondMRP.Id;
            //     group.BondMRP.LocaleId = bondMRP.LocaleId;

            //     var _bondMRP = BondMRP.Update(group.BondMRP);
            // }


            // if (bondMRP != null && group.BondMRPItem.Any())
            // {

            //     group.BondMRP.Id = bondMRP.Id;
            //     group.BondMRP.LocaleId = bondMRP.LocaleId;

            //     var _bondMRP = BondMRP.Update(group.BondMRP);
            //     var _bondMRPItem = group.BondMRPItem.OrderBy(i => i.PartNo).ToList();

            //     var seqNo = 0;
            //     _bondMRPItem.ForEach(i =>
            //     {
            //         seqNo += 1;
            //         i.SeqNo = seqNo;
            //     });

            //     BondMRPItem.RemoveRange(i => i.OrderNo == _bondMRP.OrderNo && i.LocaleId == _bondMRP.LocaleId);
            //     BondMRPItem.CreateRange(group.BondMRPItem);

            //     group = GetGroup((int)group.BondMRP.Id, (int)group.BondMRP.LocaleId);
            // }
            // else
            // {
            //     var _bondMRP = BondMRP.Create(group.BondMRP);

            // }

            // return group;
        }
        public ERP.Models.Views.BondMRPGroup RemoveGroup(ERP.Models.Views.BondMRPGroup group)
        {
            if (group.BondMRP != null && group.BondMRPItem.Any())
            {
                BondMRPItem.RemoveRange(i => i.OrderNo == group.BondMRP.OrderNo && i.LocaleId == group.BondMRP.LocaleId);

                group = GetGroup((int)group.BondMRP.Id, (int)group.BondMRP.LocaleId);
            }

            return group;
        }

        public List<ERP.Models.Views.BondMRPItem> BuildMRPItem(string orderNo, int localeId)
        {
            var items = new List<ERP.Models.Views.BondMRPItem>();
            {
                // MRPItem
                var mrpItem = (
                    from o in Orders.Get()
                    join m in MRPItem.Get() on new { OrdersId = o.Id, LocaleI = o.LocaleId } equals new { OrdersId = m.OrdersId, LocaleI = m.LocaleId }
                    join _m in Material.Get() on new { MaterialId = m.MaterialId, LocaleI = m.LocaleId } equals new { MaterialId = _m.Id, LocaleI = _m.LocaleId }
                    join b in BondMaterialChinaContrast.Get() on new { MaterialName = m.MaterialNameTw } equals new { MaterialName = b.MaterialName } into bGRP
                    from b in bGRP.DefaultIfEmpty()
                    where o.OrderNo == orderNo && o.LocaleId == localeId
                    group new { o, m, b }
                    by new { m.PartNo, m.PartNameTw, m.ParentMaterialId, m.MaterialNameTw, m.UnitNameTw, m.MaterialId, m.LocaleId, m.OrdersId, _m.SemiGoods, _m.TextureCodeId, b.WeightEachUnit, b.BondMaterialName } into mrpGRP
                    select new
                    {
                        PartNo = mrpGRP.Key.PartNo,
                        PartNameTw = mrpGRP.Key.PartNameTw,
                        ParentMaterialId = mrpGRP.Key.ParentMaterialId,
                        MaterialNameTw = mrpGRP.Key.MaterialNameTw,
                        UnitNameTw = mrpGRP.Key.UnitNameTw,
                        MaterialId = mrpGRP.Key.MaterialId,
                        LocaleId = mrpGRP.Key.LocaleId,
                        OrdersId = mrpGRP.Key.OrdersId,
                        WeightEachUnit = mrpGRP.Key.WeightEachUnit,
                        BondMaterialName = mrpGRP.Key.BondMaterialName,
                        IsAdh = mrpGRP.Key.SemiGoods,
                        IsSub = mrpGRP.Key.TextureCodeId,
                        SubUsage = mrpGRP.Sum(g => g.m.Total),
                    }
                )
                .ToList();

                // MRPOrderItem
                var mrpItemOrders = (
                    from o in Orders.Get()
                    join m in MRPItemOrders.Get() on new { OrdersId = o.Id, LocaleI = o.LocaleId } equals new { OrdersId = m.OrdersId, LocaleI = m.LocaleId }
                    join _m in Material.Get() on new { MaterialId = m.MaterialId, LocaleI = m.LocaleId } equals new { MaterialId = _m.Id, LocaleI = _m.LocaleId }
                    join b in BondMaterialChinaContrast.Get() on new { MaterialName = m.MaterialNameTw } equals new { MaterialName = b.MaterialName } into bGRP
                    from b in bGRP.DefaultIfEmpty()
                    where o.OrderNo == orderNo && o.LocaleId == localeId
                    group new { o, m, b }
                    by new { m.PartNo, m.PartNameTw, m.ParentMaterialId, m.MaterialNameTw, m.UnitNameTw, m.MaterialId, m.LocaleId, m.OrdersId, _m.SemiGoods, _m.TextureCodeId, b.WeightEachUnit, b.BondMaterialName } into mrpGRP
                    select new
                    {
                        PartNo = mrpGRP.Key.PartNo,
                        PartNameTw = mrpGRP.Key.PartNameTw,
                        ParentMaterialId = mrpGRP.Key.ParentMaterialId,
                        MaterialNameTw = mrpGRP.Key.MaterialNameTw,
                        UnitNameTw = mrpGRP.Key.UnitNameTw,
                        MaterialId = mrpGRP.Key.MaterialId,
                        LocaleId = mrpGRP.Key.LocaleId,
                        OrdersId = mrpGRP.Key.OrdersId,
                        WeightEachUnit = mrpGRP.Key.WeightEachUnit,
                        BondMaterialName = mrpGRP.Key.BondMaterialName,
                        IsAdh = mrpGRP.Key.SemiGoods,
                        IsSub = mrpGRP.Key.TextureCodeId,
                        SubUsage = mrpGRP.Sum(g => g.m.Total),
                    }
                )
                .ToList();

                var mrpItems = mrpItem.Union(mrpItemOrders);
                var bomItems = mrpItems.GroupBy(i => new { i.PartNo, i.PartNameTw, i.ParentMaterialId, i.MaterialNameTw, i.UnitNameTw, i.MaterialId, i.LocaleId, i.OrdersId, i.IsAdh, i.IsSub, i.WeightEachUnit, i.BondMaterialName })
                                    .Select(i => new
                                    {
                                        PartNo = i.Key.PartNo,
                                        PartNameTw = i.Key.PartNameTw,
                                        ParentMaterialId = i.Key.ParentMaterialId,
                                        MaterialNameTw = i.Key.MaterialNameTw,
                                        UnitNameTw = i.Key.UnitNameTw,
                                        MaterialId = i.Key.MaterialId,
                                        LocaleId = i.Key.LocaleId,
                                        OrdersId = i.Key.OrdersId,
                                        WeightEachUnit = i.Key.WeightEachUnit,
                                        BondMaterialName = i.Key.BondMaterialName,
                                        IsAdh = i.Key.IsAdh,
                                        IsSub = i.Key.IsSub,
                                        Total = i.Sum(g => g.SubUsage),
                                    })
                                    .ToList();

                var materials = bomItems.Select(i => i.MaterialNameTw).Distinct();

                var poItems = (
                    from poi in POItem.Get()
                    join po in PO.Get() on new { POId = poi.POId, LocaleId = poi.LocaleId } equals new { POId = po.Id, LocaleId = po.LocaleId }
                    join m in Material.Get() on new { MId = poi.MaterialId, LocaleId = poi.LocaleId } equals new { MId = m.Id, LocaleId = m.LocaleId }
                    join a in APMonthItem.Get() on new { POItemId = (decimal?)poi.Id, LocaleId = poi.LocaleId } equals new { POItemId = a.POItemId, LocaleId = a.LocaleId } into aGRP
                    from a in aGRP.DefaultIfEmpty()
                    join v in Vendor.Get() on new { VendorId = po.VendorId, LocaleId = po.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                    where poi.OrderNo == orderNo && poi.Status != 2
                    select new
                    {
                        Vendor = v.ShortNameTw,
                        Material = m.MaterialName,
                        PurCurrency = CodeItem.Get().Where(c => c.Id == poi.DollarCodeId && c.LocaleId == poi.LocaleId && c.CodeType == "02").Select(c => c.NameTW).FirstOrDefault(),
                        ExCurrency = CodeItem.Get().Where(c => c.Id == v.DollarCodeId && c.LocaleId == v.LocaleId && c.CodeType == "02").Select(c => c.NameTW).FirstOrDefault(),
                        PayCurrency = a.DollarCodeName,
                        PODate = po.PODate,
                    }
                ).ToList();

                items = bomItems.Select(i => new ERP.Models.Views.BondMRPItem
                {
                    Id = 0,
                    LocaleId = localeId,
                    OrderNo = orderNo,
                    IsAdh = (int)i.IsAdh,
                    IsSub = i.ParentMaterialId > 0 ? 1 : 0,
                    PartNo = i.PartNo,
                    PartNameTw = i.PartNameTw,
                    MaterialNameTw = i.MaterialNameTw,
                    UnitNameTw = i.UnitNameTw,
                    Total = i.Total,
                    WeightEachUnit = i.WeightEachUnit,
                    // Weight = i.Weight,
                    BondMaterialName = i.BondMaterialName,
                    // VendorShortNameTw = i.VendorShortNameTw,
                    // SeqNo = i.SeqNo,
                    // ModifyUserName = i.ModifyUserName,
                    // LastUpdateTime = i.LastUpdateTime,
                    // PurDollarNameTw = i.PurDollarNameTw,
                    // PayDollarNameTw = i.PayDollarNameTw,
                    // ExDollarNameTw = i.ExDollarNameTw,
                    ParentBy = i.ParentMaterialId == 0 ? $"{i.MaterialId}" : $"{i.ParentMaterialId}{i.MaterialId}{i.OrdersId}",
                })
                .OrderBy(i => i.PartNo)
                .ThenBy(i => i.ParentBy)
                .ThenBy(i => i.MaterialNameTw)
                .ToList();

                var _seqNo = 0;
                items.ForEach(i =>
                {
                    _seqNo += 1;
                    i.SeqNo = _seqNo;

                    var poItem = poItems.Where(p => p.Material == i.MaterialNameTw).OrderByDescending(p => p.PODate).FirstOrDefault();
                    if (poItem != null)
                    {
                        i.Weight = i.WeightEachUnit == null ? 0 : i.WeightEachUnit * i.Total;
                        i.VendorShortNameTw = poItem.Vendor;
                        i.PayDollarNameTw = poItem.PayCurrency == null ? poItem.PurCurrency : poItem.PayCurrency;
                        i.ExDollarNameTw = poItem.ExCurrency;
                        i.PurDollarNameTw = poItem.PurCurrency == null ? poItem.PayCurrency : poItem.PurCurrency;
                    }
                });
            }

            return items;
        }

        public ERP.Models.Views.BondMRPGroup BuildGroup(string orderNo, int localeId, int type)
        {
            var group = GetGroupByOrderNo(orderNo, localeId);
            var items = this.BuildMRPItem(orderNo, localeId);

            if (group.BondMRP.Id > 0)
            {
                var _items = group.BondMRPItem.ToList();

                // 選部分更新，但當原始的item是空值時，直接帶新的值。
                if (type == 1)
                {
                    items.ForEach(i =>
                    {
                        var item = _items.Where(o => o.PartNo == i.PartNo && o.MaterialNameTw == i.MaterialNameTw).FirstOrDefault();
                        if (item != null)
                        {
                            // i.VendorShortNameTw = item.VendorShortNameTw;
                            // i.PayDollarNameTw = item.PayDollarNameTw;
                            // i.ExDollarNameTw = item.ExDollarNameTw;
                            // i.PurDollarNameTw = item.PurDollarNameTw;
                            i.Weight = item.Weight;
                            i.WeightEachUnit = item.WeightEachUnit;
                        }
                    });
                }
                group.BondMRPItem = items; // 匯入全新

                return group;
            }
            else
            {
                var order = (
                    from o in Orders.Get().Where(i => i.Status != 3)
                    join b in BondProductChinaContrast.Get() on new { StyleNo = o.StyleNo, LocaleId = o.LocaleId } equals new { StyleNo = b.StyleNo, LocaleId = b.LocaleId } into bGRP
                    from b in bGRP.DefaultIfEmpty()
                    where o.OrderNo == orderNo && o.LocaleId == localeId
                    select new ERP.Models.Views.BondMRP
                    {
                        Id = 0,
                        LocaleId = (decimal?)b.LocaleId ?? localeId,
                        IsClose = 0,
                        OrderNo = o.OrderNo,
                        BOMLocaleId = b.LocaleId,
                        StyleNo = o.StyleNo,
                        BondProductName = b.BondProductName ?? "",
                    }
                )
                .FirstOrDefault();

                if (order != null)
                {
                    group.BondMRP = order;
                    group.BondMRPItem = items;
                }

                return group;
            }
        }
        public ERP.Models.Views.BondMRPGroup BuildGroup1(string orderNo, int localeId, int type)
        {
            var group = GetGroupByOrderNo(orderNo, localeId);
            var items = this.BuildMRPItem(orderNo, localeId);

            if (group.BondMRP.Id > 0)
            {
                var _items = group.BondMRPItem.ToList();

                if (!_items.Any()) { _items = items; }

                // 選部分更新，但當原始的item是空值時，直接帶新的值。
                if (type == 1 && _items.Any())
                {
                    _items.ForEach(i =>
                    {
                        var item = items.Where(o => o.PartNo == i.PartNo && o.MaterialNameTw == i.MaterialNameTw).FirstOrDefault();
                        if (item != null)
                        {
                            i.VendorShortNameTw = item.VendorShortNameTw;
                            i.PayDollarNameTw = item.PayDollarNameTw;
                            i.ExDollarNameTw = item.ExDollarNameTw;
                            i.PurDollarNameTw = item.PurDollarNameTw;
                        }
                    });
                }
                group.BondMRPItem = items; // 匯入全新

                return group;
            }
            else
            {
                var order = (
                    from o in Orders.Get().Where(i => i.Status != 3)
                    join b in BondProductChinaContrast.Get() on new { StyleNo = o.StyleNo, LocaleId = o.LocaleId } equals new { StyleNo = b.StyleNo, LocaleId = b.LocaleId } into bGRP
                    from b in bGRP.DefaultIfEmpty()
                    where o.OrderNo == orderNo && o.LocaleId == localeId
                    select new ERP.Models.Views.BondMRP
                    {
                        Id = 0,
                        LocaleId = (decimal?)b.LocaleId ?? localeId,
                        IsClose = 0,
                        OrderNo = o.OrderNo,
                        BOMLocaleId = b.LocaleId,
                        StyleNo = o.StyleNo,
                        BondProductName = b.BondProductName ?? "",
                    }
                )
                .FirstOrDefault();

                if (order != null)
                {
                    group.BondMRP = order;
                    group.BondMRPItem = items;
                }

                return group;
            }
        }

    }
}
