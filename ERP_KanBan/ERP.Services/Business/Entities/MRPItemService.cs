using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MRPItemService : BusinessService
    {
        private Services.Entities.MRPItemService MRPItem { get; }
        private Services.Entities.MaterialService Material { get; }

        public MRPItemService(
            Services.Entities.MRPItemService mrpItemService, 
            Services.Entities.MaterialService materialService,
            UnitOfWork unitOfWork):base(unitOfWork)
        {
            MRPItem = mrpItemService;
            Material = materialService;
        }
        public IQueryable<Models.Views.MRPItem> Get()
        {
            var mrpItemOrders = (
                from mi in MRPItem.Get()
                // join m in Material.Get() on new { MaterialId = mi.MaterialId, LocaleId = mi.LocaleId} equals new { MaterialId = m.Id, LocaleId = m.LocaleId} into mGrp
                // from m in mGrp.DefaultIfEmpty()
                join m in Material.Get() on new { MaterialId = mi.MaterialId, LocaleId = mi.LocaleId} equals new { MaterialId = m.Id, LocaleId = m.LocaleId} 
                select new Models.Views.MRPItem
                {
                    Id = mi.Id,
                    LocaleId = mi.LocaleId,
                    OrdersId = mi.OrdersId,
                    PartId = mi.PartId,
                    PartNo = mi.PartNo,
                    PartNameTw = mi.PartNameTw,
                    PartNameEn = mi.PartNameEn,
                    MaterialId = mi.MaterialId,
                    // MaterialNameTw = m == null ? mi.MaterialNameTw + "(Material Loss)" : mi.MaterialNameTw,
                    // MaterialNameEn = m == null ? mi.MaterialNameEn + "(Material Loss)" :mi.MaterialNameEn,
                    MaterialNameTw = mi.MaterialNameTw,
                    MaterialNameEn = mi.MaterialNameEn,
                    UnitCodeId = mi.UnitCodeId,
                    UnitNameTw = mi.UnitNameTw,
                    UnitNameEn = mi.UnitNameEn,
                    UnitTotal = mi.UnitTotal,
                    Total = mi.Total,
                    SizeDivision = mi.SizeDivision,
                    SizeDivisionDescTw = mi.SizeDivisionDescTw,
                    SizeDivisionDescEn = mi.SizeDivisionDescEn,
                    StyleVersion = mi.StyleVersion,
                    ParentMaterialId = mi.ParentMaterialId,
                    ModifyUserName = mi.ModifyUserName,
                    LastUpdateTime = mi.LastUpdateTime,
                    // SemiGoods = m == null ? 0 : m.SemiGoods,
                    SemiGoods = m.SemiGoods,
                    SizeUsage = mi.SizeUsage,
                    MRPVersion = mi.MRPVersion,
                    CategoryCodeId = m.CategoryCodeId
                } 
            );
            return mrpItemOrders;
        }
    }
}