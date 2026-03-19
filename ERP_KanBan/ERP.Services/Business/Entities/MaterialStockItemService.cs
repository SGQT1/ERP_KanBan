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
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MaterialStockItemService : BusinessService
    {
        private Services.Entities.MaterialStockService MaterialStock { get; }
        private Services.Entities.StockIOService StockIO { get; }
        private Services.Entities.CodeItemService CodeItem { get; }
        private Services.Entities.StockIOService _StockIO { get; }
        private Services.Entities.MaterialService Material { get; }
        public MaterialStockItemService(
            Services.Entities.MaterialStockService materialStock,
            Services.Entities.StockIOService stockIOService,
            Services.Entities.StockIOService _stockIOService,
            Services.Entities.CodeItemService codeItemService,
            Services.Entities.MaterialService materialService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            MaterialStock = materialStock;
            StockIO = stockIOService;
            _StockIO = _stockIOService;
            CodeItem = codeItemService;
            Material = materialService;
        }
        public IQueryable<Models.Views.MaterialStockItem> Get()
        {
            return (
                from sio in StockIO.Get()
                join ms in MaterialStock.Get() on new { MaterialStockId = sio.MaterialStockId, LocaleId = sio.LocaleId } equals
                                                  new { MaterialStockId = ms.Id, LocaleId = ms.LocaleId }
                 join m in Material.Get() on new { MaterialId = sio.MaterialId, LocaleId = sio.LocaleId } equals
                                                  new { MaterialId = m.Id, LocaleId = m.LocaleId }
                select new Models.Views.MaterialStockItem
                {
                    Id = sio.Id,
                    LocaleId = sio.LocaleId,
                    IODate = sio.IODate,
                    SourceType = sio.SourceType,
                    MaterialId = sio.MaterialId,
                    WarehouseId = sio.WarehouseId,
                    OrderNo = sio.OrderNo,
                    PCLUnitCodeId = sio.PCLUnitCodeId,
                    TransRate = sio.TransRate,
                    PurUnitCodeId = sio.PurUnitCodeId,
                    PCLIOQty = sio.PCLIOQty,
                    PurIOQty = sio.PurIOQty,
                    ReceivedLogId = sio.ReceivedLogId,
                    PurUnitPrice = sio.PurUnitPrice,
                    PurDollarCodeId = sio.PurDollarCodeId,
                    BankingRate = sio.BankingRate,
                    StockDollarCodeId = sio.StockDollarCodeId,
                    Remark = sio.Remark,
                    RefNo = sio.RefNo,
                    OrgUnitId = sio.OrgUnitId,
                    OrgUnitNameTw = sio.OrgUnitNameTw,
                    OrgUnitNameEn = sio.OrgUnitNameEn,
                    MPSProcessId = sio.MPSProcessId,
                    MPSProcessNameTw = sio.MPSProcessNameTw,
                    MPSProcessNameEn = sio.MPSProcessNameEn,
                    RefUserName = sio.RefUserName,
                    MaterialStockId = sio.MaterialStockId,
                    PrePCLQty = sio.PrePCLQty,
                    PreAmount = sio.PreAmount,
                    ModifyUserName = sio.ModifyUserName,
                    LastUpdateTime = sio.LastUpdateTime,

                    MaterialName = ms.MaterialName,
                    MaterialNameEng = ms.MaterialNameEng,
                    WarehouseNo = ms.WarehouseNo,
                    PurUnit = CodeItem.Get().Where(u => u.Id == sio.PurUnitCodeId && u.LocaleId == sio.LocaleId && u.CodeType == "21").Max(u => u.NameTW),
                    PCLUnit = CodeItem.Get().Where(u => u.Id == sio.PCLUnitCodeId && u.LocaleId == sio.LocaleId && u.CodeType == "21").Max(u => u.NameTW),
                    PurCurrency = CodeItem.Get().Where(u => u.Id == sio.PurDollarCodeId && u.LocaleId == sio.LocaleId && u.CodeType == "02").Max(u => u.NameTW),
                    StockCurrency = CodeItem.Get().Where(u => u.Id == sio.StockDollarCodeId && u.LocaleId == sio.LocaleId && u.CodeType == "02").Max(u => u.NameTW),
                    StockQty = ms.PCLQty,
                    MPSQty = ms.PCLPlanQty,
                    AvgUnitPrice = ms.AvgUnitPrice,
                    SemiGoods = m.SemiGoods,
                    // ExchangeRate = ms.ExchangeRate,
                });

        }
        
        public Models.Views.MaterialStockItem Create(Models.Views.MaterialStockItem item)
        {
            var _item = StockIO.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MaterialStockItem Update(Models.Views.MaterialStockItem item)
        {
            var _item = StockIO.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MaterialStockItem item)
        {
            StockIO.Remove(Build(item));
        }

        //for update, transfer view model to entity
        private Models.Entities.StockIO Build(Models.Views.MaterialStockItem item)
        {
            return new Models.Entities.StockIO()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                IODate = item.IODate,
                SourceType = item.SourceType,
                MaterialId = item.MaterialId,
                WarehouseId = item.WarehouseId,
                OrderNo = item.OrderNo,
                PCLUnitCodeId = item.PCLUnitCodeId,
                TransRate = item.TransRate,
                PurUnitCodeId = item.PurUnitCodeId,
                PCLIOQty = item.PCLIOQty,
                PurIOQty = item.PurIOQty,
                ReceivedLogId = item.ReceivedLogId,
                PurUnitPrice = item.PurUnitPrice,
                PurDollarCodeId = item.PurDollarCodeId,
                BankingRate = item.BankingRate,
                StockDollarCodeId = item.StockDollarCodeId,
                Remark = item.Remark,
                RefNo = item.RefNo,
                OrgUnitId = item.OrgUnitId,
                OrgUnitNameTw = item.OrgUnitNameTw,
                OrgUnitNameEn = item.OrgUnitNameEn,
                MPSProcessId = item.MPSProcessId,
                MPSProcessNameTw = item.MPSProcessNameTw,
                MPSProcessNameEn = item.MPSProcessNameEn,
                RefUserName = item.RefUserName,
                MaterialStockId = item.MaterialStockId,
                PrePCLQty = item.PrePCLQty,
                PreAmount = item.PreAmount,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                SeqNo = item.SeqNo,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.MaterialStockItem> items)
        {
            StockIO.CreateRange(BuildRange(items));
        }
        public void UpdateRange(IEnumerable<Models.Views.MaterialStockItem> items)
        {
            StockIO.UpdateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.StockIO, bool>> predicate)
        {
            StockIO.RemoveRange(predicate);
        }
        private IEnumerable<Models.Entities.StockIO> BuildRange(IEnumerable<Models.Views.MaterialStockItem> items)
        {
            return items.Select(item => new Models.Entities.StockIO
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                IODate = item.IODate,
                SourceType = item.SourceType,
                MaterialId = item.MaterialId,
                WarehouseId = item.WarehouseId,
                OrderNo = item.OrderNo,
                PCLUnitCodeId = item.PCLUnitCodeId,
                TransRate = item.TransRate,
                PurUnitCodeId = item.PurUnitCodeId,
                PCLIOQty = item.PCLIOQty,
                PurIOQty = item.PurIOQty,
                ReceivedLogId = item.ReceivedLogId,
                PurUnitPrice = item.PurUnitPrice,
                PurDollarCodeId = item.PurDollarCodeId,
                BankingRate = item.BankingRate,
                StockDollarCodeId = item.StockDollarCodeId,
                Remark = item.Remark,
                RefNo = item.RefNo,
                OrgUnitId = item.OrgUnitId,
                OrgUnitNameTw = item.OrgUnitNameTw,
                OrgUnitNameEn = item.OrgUnitNameEn,
                MPSProcessId = item.MPSProcessId,
                MPSProcessNameTw = item.MPSProcessNameTw,
                MPSProcessNameEn = item.MPSProcessNameEn,
                RefUserName = item.RefUserName,
                MaterialStockId = item.MaterialStockId,
                PrePCLQty = item.PrePCLQty,
                PreAmount = item.PreAmount,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                SeqNo = item.SeqNo,
            });
        }


        public void UpdatRefNo(int id, int localeId, string refNo)
        {
            _StockIO.UpdateRange(
                i => i.Id == id && i.LocaleId == localeId,
                // u => new StockIO { RefNo = refNo }
                u => u.SetProperty(p => p.RefNo, v => refNo)
            );
        }
        public void UpdatePrice(Models.Views.ReceivedLog item)
        {
            StockIO.UpdateRange(
                i => i.ReceivedLogId == item.Id && i.LocaleId == item.LocaleId,
                // u => new StockIO { PurUnitPrice = (decimal)item.PurUnitPrice, LastUpdateTime = DateTime.Now }
                u => u.SetProperty(p => p.PurUnitPrice, v => (decimal)item.PurUnitPrice).SetProperty(p => p.LastUpdateTime, v => DateTime.Now)
            );
        }
    }
}