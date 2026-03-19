using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class BOMService : BusinessService
    {
        private ERP.Services.Business.Entities.BOMRepOrdersService MRPOrders { get; set; }
        private ERP.Services.Business.Entities.BOMRepPCLService MRPPCL { get; set; }
        private ERP.Services.Business.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersService MRPItemOrders { get; set; }

        private ERP.Services.Business.Entities.BOMRepPCLBKService MRPPCLBK { get; set; }
        private ERP.Services.Business.Entities.MRPItemBKService MRPItemBK { get; set; }
        private ERP.Services.Business.Entities.MRPItemOrdersBKService MRPItemOrdersBK { get; set; }

        public BOMService(
            ERP.Services.Business.Entities.BOMRepOrdersService mrpOrders,
            ERP.Services.Business.Entities.BOMRepPCLService mrpPCL,
            ERP.Services.Business.Entities.MRPItemService mrpItem,
            ERP.Services.Business.Entities.MRPItemOrdersService mRPItemOrders,

            ERP.Services.Business.Entities.BOMRepPCLBKService mrpPCLBK,
            ERP.Services.Business.Entities.MRPItemBKService mrpItemBK,
            ERP.Services.Business.Entities.MRPItemOrdersBKService mRPItemOrdersBK,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MRPOrders = mrpOrders;
            MRPPCL = mrpPCL;
            MRPItem = mrpItem;
            MRPItemOrders = mRPItemOrders;

            MRPPCLBK = mrpPCLBK;
            MRPItemBK = mrpItemBK;
            MRPItemOrdersBK = mRPItemOrdersBK;
        }

        public ERP.Models.Views.BOMGroup Get(int id, int localeId)
        {
            var bomOrders = MRPOrders.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();

            if (bomOrders != null)
            {
                var bomPCL = MRPPCL.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId).ToList();
                var bomPCLBK = MRPPCLBK.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId).ToList();

                if (bomPCL.Count() > 0)
                {
                    var mrpItems = MRPItem.Get()
                        .Where(i => i.OrdersId == id && i.LocaleId == localeId && (i.ParentMaterialId == null || i.ParentMaterialId == 0))
                        .Select(i => new ERP.Models.Views.View.BOMItem
                        {
                            Id = i.Id,
                            LocaleId = i.LocaleId,
                            OrdersId = i.OrdersId,
                            PartId = i.PartId,
                            PartNo = i.PartNo,
                            PartNameTw = i.PartNameTw,
                            PartNameEn = i.PartNameEn,
                            MaterialId = i.MaterialId,
                            MaterialNameTw = i.MaterialNameTw,
                            MaterialNameEn = i.MaterialNameEn,
                            UnitCodeId = i.UnitCodeId,
                            UnitNameTw = i.UnitNameTw,
                            UnitNameEn = i.UnitNameEn,
                            UnitTotal = i.UnitTotal,
                            Total = i.Total,
                            SizeDivision = i.SizeDivision,
                            SizeDivisionDescTw = i.SizeDivisionDescTw,
                            SizeDivisionDescEn = i.SizeDivisionDescEn,
                            Version = i.StyleVersion,
                            ParentMaterialId = i.ParentMaterialId,
                            ModifyUserName = i.ModifyUserName,
                            LastUpdateTime = i.LastUpdateTime,
                            SemiGoods = i.SemiGoods,
                        }).ToList();
                    var mrpItemOrders = MRPItemOrders.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId)
                        .Select(i => new ERP.Models.Views.View.BOMItem
                        {
                            Id = i.Id,
                            LocaleId = i.LocaleId,
                            OrdersId = i.OrdersId,
                            PartId = i.PartId,
                            PartNo = i.PartNo,
                            PartNameTw = i.PartNameTw,
                            PartNameEn = i.PartNameEn,
                            MaterialId = i.MaterialId,
                            MaterialNameTw = i.MaterialNameTw,
                            MaterialNameEn = i.MaterialNameEn,
                            UnitCodeId = i.UnitCodeId,
                            UnitNameTw = i.UnitNameTw,
                            UnitNameEn = i.UnitNameEn,
                            UnitTotal = i.UnitTotal,
                            Total = i.Total,
                            SizeDivision = i.SizeDivision,
                            SizeDivisionDescTw = i.SizeDivisionDescTw,
                            SizeDivisionDescEn = i.SizeDivisionDescEn,
                            Version = i.OrderVersion,
                            ParentMaterialId = i.ParentMaterialId,
                            ModifyUserName = i.ModifyUserName,
                            LastUpdateTime = i.LastUpdateTime,
                            SemiGoods = i.SemiGoods,
                        }).ToList();

                    var serialNo = 1;
                    var bomItems = mrpItems.Union(mrpItemOrders).OrderBy(i => i.PartNo).ToList();
                    bomItems.ForEach(i =>
                    {
                        i.SerialNo = serialNo;
                        serialNo++;
                    });
                    return new ERP.Models.Views.BOMGroup
                    {
                        BOMOrders = bomOrders,
                        BOMPCL = bomPCL,
                        BOMItems = bomItems
                    };
                }

                if (bomPCLBK.Count() > 0)
                {
                    var mrpItemsBK = MRPItemBK.Get()
                        .Where(i => i.OrdersId == id && i.LocaleId == localeId && (i.ParentMaterialId == null || i.ParentMaterialId == 0))
                        .Select(i => new ERP.Models.Views.View.BOMItem
                        {
                            Id = i.Id,
                            LocaleId = i.LocaleId,
                            OrdersId = i.OrdersId,
                            PartId = i.PartId,
                            PartNo = i.PartNo,
                            PartNameTw = i.PartNameTw,
                            PartNameEn = i.PartNameEn,
                            MaterialId = i.MaterialId,
                            MaterialNameTw = i.MaterialNameTw,
                            MaterialNameEn = i.MaterialNameEn,
                            UnitCodeId = i.UnitCodeId,
                            UnitNameTw = i.UnitNameTw,
                            UnitNameEn = i.UnitNameEn,
                            UnitTotal = i.UnitTotal,
                            Total = i.Total,
                            SizeDivision = i.SizeDivision,
                            SizeDivisionDescTw = i.SizeDivisionDescTw,
                            SizeDivisionDescEn = i.SizeDivisionDescEn,
                            Version = i.StyleVersion,
                            ParentMaterialId = i.ParentMaterialId,
                            ModifyUserName = i.ModifyUserName,
                            LastUpdateTime = i.LastUpdateTime,
                            SemiGoods = i.SemiGoods,
                        }).ToList();
                    var mrpItemOrders = MRPItemOrders.Get().Where(i => i.OrdersId == id && i.LocaleId == localeId)
                        .Select(i => new ERP.Models.Views.View.BOMItem
                        {
                            Id = i.Id,
                            LocaleId = i.LocaleId,
                            OrdersId = i.OrdersId,
                            PartId = i.PartId,
                            PartNo = i.PartNo,
                            PartNameTw = i.PartNameTw,
                            PartNameEn = i.PartNameEn,
                            MaterialId = i.MaterialId,
                            MaterialNameTw = i.MaterialNameTw,
                            MaterialNameEn = i.MaterialNameEn,
                            UnitCodeId = i.UnitCodeId,
                            UnitNameTw = i.UnitNameTw,
                            UnitNameEn = i.UnitNameEn,
                            UnitTotal = i.UnitTotal,
                            Total = i.Total,
                            SizeDivision = i.SizeDivision,
                            SizeDivisionDescTw = i.SizeDivisionDescTw,
                            SizeDivisionDescEn = i.SizeDivisionDescEn,
                            Version = i.OrderVersion,
                            ParentMaterialId = i.ParentMaterialId,
                            ModifyUserName = i.ModifyUserName,
                            LastUpdateTime = i.LastUpdateTime,
                            SemiGoods = i.SemiGoods,
                        }).ToList();

                    var serialNo = 1;
                    var bomItemsBK = mrpItemsBK.Union(mrpItemOrders).OrderBy(i => i.PartNo).ToList();
                    bomItemsBK.ForEach(i =>
                    {
                        i.SerialNo = serialNo;
                        serialNo++;
                    });
                    return new ERP.Models.Views.BOMGroup
                    {
                        BOMOrders = bomOrders,
                        BOMPCL = bomPCLBK,
                        BOMItems = bomItemsBK
                    };
                }
            }

            return new ERP.Models.Views.BOMGroup { };

        }
    }
}