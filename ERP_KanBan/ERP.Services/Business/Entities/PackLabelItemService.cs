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
    public class PackLabelItemService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.CTNLabelService CTNLabel { get; }
        private Services.Entities.CTNLabelItemService CTNLabelItem { get; }
        private Services.Entities.CTNLabelStockInService CTNLabelStockIn { get; }
        private Services.Entities.CTNLabelStockOutService CTNLabelStockOut { get; }

        private Services.Entities.OrdersPLService OrdersPL { get; }
        private Services.Entities.OrdersPackService OrdersPack { get; }
        public PackLabelItemService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.CTNLabelService ctnLabelService,
            Services.Entities.CTNLabelItemService ctnLabelItemService,
            Services.Entities.CTNLabelStockInService ctnLabelStockInService,
            Services.Entities.CTNLabelStockOutService ctnLabelStockOutService,

            Services.Entities.OrdersPLService ordersPLService,
            Services.Entities.OrdersPackService ordersPackService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            Orders = ordersService;
            CTNLabel = ctnLabelService;
            CTNLabelItem = ctnLabelItemService;
            CTNLabelStockIn = ctnLabelStockInService;
            CTNLabelStockOut = ctnLabelStockOutService;

            OrdersPL = ordersPLService;
            OrdersPack = ordersPackService;
        }

        public IQueryable<Models.Views.PackLabelItem> Get()
        {
            var items = (
                from h in CTNLabel.Get()
                join o in Orders.Get() on new { OrderNo = h.OrderNo } equals new { OrderNo = o.OrderNo }
                join i in CTNLabelItem.Get() on new { CTNLabelId = h.Id, LocaleId = h.LocaleId } equals new { CTNLabelId = i.CTNLabelId, LocaleId = i.LocaleId }
                join si in CTNLabelStockIn.Get() on new { LocaleId = i.LocaleId, LabelCode = i.LabelCode } equals new { LocaleId = si.LocaleId, LabelCode = si.LabelCode } into siGRP
                from si in siGRP.DefaultIfEmpty()
                join so in CTNLabelStockOut.Get() on new { LocaleId = i.LocaleId, LabelCode = i.LabelCode } equals new { LocaleId = so.LocaleId, LabelCode = so.LabelCode } into soGRP
                from so in soGRP.DefaultIfEmpty()
                select new Models.Views.PackLabelItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    CTNLabelId = i.CTNLabelId,
                    GroupBy = i.GroupBy,
                    PackingType = i.PackingType,
                    LabelCode = i.LabelCode,
                    MinRefDisplaySize = i.MinRefDisplaySize,
                    MaxRefDisplaySize = i.MaxRefDisplaySize,
                    SubPackingQty = i.SubPackingQty,
                    SubNetWeight = i.SubNetWeight,
                    SubGrossWeight = i.SubGrossWeight,
                    SubMEAS = i.SubMEAS,
                    SubCBM = i.SubCBM,
                    SeqNo = i.SeqNo,
                    SubLabelCode = i.SubLabelCode,
                    DeptNo = i.DeptNo,

                    OrderNo = o.OrderNo,
                    StyleNo = o.StyleNo,
                    ShoeName = o.ShoeName,
                    Edition = "",
                    IsPrint = h.IsPrint,
                    PackingQty = h.PackingQty,
                    OrderId = o.Id,
                    CompanyId = o.CompanyId,
                    BrandCodeId = o.BrandCodeId,

                    ExFactoryDate = h.ExFactoryDate,
                    
                    HasStockIn = si != null ? true : false,
                    StockInGrossWeight = si.SubGrossWeight,
                    StockInTime = si.StockInTime,

                    HasStockOut = so != null ? true : false,
                    StockOutGrossWeight = so.SubGrossWeight,
                    StockOutTime = so.StockOutTime,
                    LastLabelCode = i.LabelCode,
                }
            );
            return items.OrderBy(i => i.SeqNo);
        }

        public IQueryable<Models.Views.PackLabelItem> GetEntity()
        {
            var items = (
                from i in CTNLabelItem.Get()
                select new Models.Views.PackLabelItem
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    CTNLabelId = i.CTNLabelId,
                    GroupBy = i.GroupBy,
                    PackingType = i.PackingType,
                    LabelCode = i.LabelCode,
                    MinRefDisplaySize = i.MinRefDisplaySize,
                    MaxRefDisplaySize = i.MaxRefDisplaySize,
                    SubPackingQty = i.SubPackingQty,
                    SubNetWeight = i.SubNetWeight,
                    SubGrossWeight = i.SubGrossWeight,
                    SubMEAS = i.SubMEAS,
                    SubCBM = i.SubCBM,
                    SeqNo = i.SeqNo,
                    SubLabelCode = i.SubLabelCode,
                    DeptNo = i.DeptNo,
                }
            );
            return items;
        }
                
        public IEnumerable<Models.Views.PackLabelItem> GetWithStock(IEnumerable<Models.Views.PackLabelItem> packLabelItems, int localeId)
        {
            var codes = packLabelItems.Select(i => i.LabelCode);
            var si = CTNLabelStockIn.Get().Where(i => i.LocaleId == localeId && codes.Contains(i.LabelCode)).Select(i => new { LabelCode = i.LabelCode }).ToList();
            var so = CTNLabelStockOut.Get().Where(i => i.LocaleId == localeId && codes.Contains(i.LabelCode)).Select(i => new { LabelCode = i.LabelCode }).ToList();

            packLabelItems.ToList().ForEach(i =>
            {
                i.HasStockIn = si.Where(s => s.LabelCode == i.LabelCode).Any();
                i.HasStockOut = so.Where(s => s.LabelCode == i.LabelCode).Any();
            });
            return packLabelItems;
        }
        public IEnumerable<Models.Views.PackLabelItem> GetPreBarcode(int ctnFrom, IEnumerable<Models.Views.PackLabelEdition> packLabelEdition)
        {
            var barcodes = new List<Models.Views.PackLabelItem>();
            
            if (packLabelEdition.Where(i => i.IsEdition == true).Any())
            {
                var orderNo = packLabelEdition.Where(i => i.IsEdition == true).Max(i => i.OrderNo);
                var localeId = packLabelEdition.Where(i => i.IsEdition == true).Max(i => i.LocaleId);
                var editions = packLabelEdition.Where(i => i.IsEdition == true).Select(i => i.Edition).Distinct().ToArray();

                var order = Orders.Get().Where(o => o.OrderNo == orderNo).Select(o => new { Id = o.Id, LocaleId = o.LocaleId , OrderNo = o.OrderNo}).FirstOrDefault();
                if (order != null)
                {
                    var ops = (
                        from ol in OrdersPL.Get()
                        join op in OrdersPack.Get() on new { RefOrdersId = (decimal)ol.RefOrdersId, LocaleId = ol.LocaleId, Edition = ol.Edition } equals new { RefOrdersId = op.RefOrdersId, LocaleId = op.LocaleId, Edition = op.Edition }
                        //where ol.RefOrdersId == order.Id && ol.LocaleId == order.LocaleId
                        where ol.OrderNo == order.OrderNo
                        select new
                        {
                            Edition = op.Edition,
                            GroupBy = op.GroupBy,
                            CTNS = op.CTNS,
                            PairOfCTN = op.PairOfCTN,
                            PackingType = ol.PackingType,
                            LocaleId = op.LocaleId,

                            NWOfCTN = op.NWOfCTN,
                            GWOfCTN = op.GWOfCTN,
                            MEAS = op.MEAS,
                            CBM = op.CBM,
                            ItemInnerSize = op.ItemInnerSize,
                            RefDisplaySize = op.RefDisplaySize,
                            CNoFrom = ol.CNoFrom,
                        }
                    )
                    .Where(i => editions.Contains(i.Edition))
                    .ToList();
                    
                    var packingType = ops.Select(i => i.PackingType).Distinct().FirstOrDefault();
                    ctnFrom = ops.Select(i => i.CNoFrom).Distinct().FirstOrDefault(); 
                    
                    if(packingType == 1) {
                        //混裝
                        var editionGroupBY = ops.GroupBy(i => new { i.Edition, i.GroupBy, i.CTNS, i.PackingType, i.NWOfCTN, i.GWOfCTN, i.MEAS })
                        .Select(i => new
                        {
                            Edition = i.Key.Edition,
                            GroupBy = i.Key.GroupBy,
                            CTNS = i.Key.CTNS,
                            PairOfCTN = i.Sum(op => op.PairOfCTN),
                            PackingType = i.Key.PackingType
                        })
                        .OrderBy(i => i.Edition).ThenBy(i => i.GroupBy).ToList();

                        var seqNo = ctnFrom;
                        editionGroupBY.ForEach(g =>
                        {
                            var op = ops.Where(o => o.Edition == g.Edition && o.GroupBy == g.GroupBy).FirstOrDefault();
                            var MinRefDisplaySize = ops.Where(o => o.Edition == g.Edition && o.GroupBy == g.GroupBy).OrderBy(i => i.ItemInnerSize).Select(i => i.RefDisplaySize).FirstOrDefault();
                            var MaxRefDisplaySize = ops.Where(o => o.Edition == g.Edition && o.GroupBy == g.GroupBy).OrderByDescending(i => i.ItemInnerSize).Select(i => i.RefDisplaySize).FirstOrDefault();
                            for (var i = 0; i < g.CTNS; i++)
                            {
                                var barcode = new Models.Views.PackLabelItem
                                {
                                    LocaleId = op.LocaleId,
                                    GroupBy = op.GroupBy,
                                    SeqNo = seqNo,
                                    SubNetWeight = op.NWOfCTN,
                                    SubGrossWeight = op.GWOfCTN,
                                    SubMEAS = op.MEAS,
                                    SubCBM = op.CBM/g.CTNS,
                                    OrderNo = orderNo,
                                    MinRefDisplaySize = MinRefDisplaySize,
                                    MaxRefDisplaySize = MaxRefDisplaySize,
                                    SubPackingQty = g.PairOfCTN,
                                    PackingType = g.PackingType,
                                    LabelCode = (g.PackingType + 1) + "-" + op.GroupBy + "-" + g.PairOfCTN + "-" + seqNo.ToString("0000") + "-"
                                };
                                barcodes.Add(barcode);
                                seqNo += 1;
                            }
                        });
                    } 
                    
                    if(packingType == 0) {
                        // 單裝
                        var seqNo = ctnFrom;
                        ops.ForEach(g =>
                        {
                            for (var i = 0; i < g.CTNS; i++)
                            {
                                var barcode = new Models.Views.PackLabelItem
                                {
                                    LocaleId = g.LocaleId,
                                    GroupBy = g.GroupBy,
                                    SeqNo = seqNo,
                                    SubNetWeight = g.NWOfCTN,
                                    SubGrossWeight = g.GWOfCTN,
                                    SubMEAS = g.MEAS,
                                    SubCBM = g.CBM/g.CTNS,
                                    OrderNo = orderNo,
                                    MinRefDisplaySize = g.RefDisplaySize,
                                    MaxRefDisplaySize = g.RefDisplaySize,
                                    SubPackingQty = g.PairOfCTN,
                                    PackingType = g.PackingType,
                                    LabelCode = (g.PackingType + 1) + "-" + g.GroupBy + "-" + g.PairOfCTN + "-" + seqNo.ToString("0000") + "-"
                                };
                                barcodes.Add(barcode);
                                seqNo += 1;
                            }
                        });
                    }

                }
            }
            return barcodes.OrderBy(i => i.SeqNo);
        }

        public void CreateRange(IEnumerable<Models.Views.PackLabelItem> packLabelItems)
        {
            CTNLabelItem.CreateRange(BuildRange(packLabelItems));
        }
        public void RemoveRange(List<string> labelCodes, int packLabelId, int localeId)
        {
            CTNLabelStockIn.RemoveRange(i => labelCodes.Contains(i.LabelCode) && i.LocaleId == localeId);
            CTNLabelStockOut.RemoveRange(i => labelCodes.Contains(i.LabelCode) && i.LocaleId == localeId);            
            CTNLabelItem.RemoveRange(i => labelCodes.Contains(i.LabelCode) && i.CTNLabelId == packLabelId && i.LocaleId == localeId);
        }

        // 與普通的CTNLableItem的差別是CTNLabelItem全部刪掉在重新新增
        public void CustomerRemoveRange(List<string> labelCodes, int packLabelId, int localeId)
        {      
            CTNLabelStockIn.RemoveRange(i => labelCodes.Contains(i.LabelCode) && i.LocaleId == localeId);
            CTNLabelStockOut.RemoveRange(i => labelCodes.Contains(i.LabelCode) && i.LocaleId == localeId); 
            CTNLabelItem.RemoveRange(i => i.CTNLabelId == packLabelId && i.LocaleId == localeId);
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
    
        public Models.Views.PackLabelItem Create(Models.Views.PackLabelItem packLabelItems)
        {
            var _item = CTNLabelItem.Create(Build(packLabelItems));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Update (Models.Views.PackLabelItem packLabelItems, string labelCode, int localeId)
        {
            var newLabelCode = packLabelItems.LabelCode;

            CTNLabelStockIn.UpdateRange(
                i => i.LabelCode == labelCode && i.LocaleId == localeId,
                // i => new Models.Entities.CTNLabelStockIn { LabelCode = newLabelCode }
                u => u.SetProperty(p => p.LabelCode, v => newLabelCode)
            );
            CTNLabelStockOut.UpdateRange(
                i => i.LabelCode == labelCode && i.LocaleId == localeId,
                // i => new Models.Entities.CTNLabelStockout { LabelCode = newLabelCode }
                u => u.SetProperty(p => p.LabelCode, v => newLabelCode)
            );

            var _item = CTNLabelItem.Update(Build(packLabelItems));

            Console.WriteLine(_item.LabelCode);
        }
        public Models.Entities.CTNLabelItem Build(Models.Views.PackLabelItem item)
        {
            return new Models.Entities.CTNLabelItem
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
            };
        }
    }
}