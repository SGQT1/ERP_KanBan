using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MRPItemOrdersBKService : BusinessService
    {
        private Services.Entities.MRPItemOrdersBKService MRPItemOrdersBK { get; }
        private Services.Entities.MaterialService Material { get; }

        public MRPItemOrdersBKService(
            Services.Entities.MRPItemOrdersBKService mrpItemOrdersBKService,
            Services.Entities.MaterialService materialService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.MRPItemOrdersBK = mrpItemOrdersBKService;
            this.Material = materialService;
        }
        public IQueryable<Models.Views.MRPItemOrders> Get()
        {
            var mrpItemOrders = (
                from mis in MRPItemOrdersBK.Get()
                join m in Material.Get() on new { MaterialId = mis.MaterialId, LocaleId = mis.LocaleId} equals new { MaterialId = m.Id, LocaleId = m.LocaleId} into mGrp
                from m in mGrp.DefaultIfEmpty()
                select new Models.Views.MRPItemOrders
                {
                    Id = mis.Id,
                    LocaleId = mis.LocaleId,
                    OrdersId = mis.OrdersId,
                    PartId = mis.PartId,
                    PartNo = mis.PartNo,
                    PartNameTw = mis.PartNameTw,
                    PartNameEn = mis.PartNameEn,
                    MaterialId = mis.MaterialId,
                    MaterialNameTw = m == null ? mis.MaterialNameTw + "(Material Loss)" : mis.MaterialNameTw,
                    MaterialNameEn = m == null ? mis.MaterialNameEn + "(Material Loss)" :mis.MaterialNameEn,
                    UnitCodeId = mis.UnitCodeId,
                    UnitNameTw = mis.UnitNameTw,
                    UnitNameEn = mis.UnitNameEn,
                    UnitTotal = mis.UnitTotal,
                    Total = mis.Total,
                    SizeDivision = mis.SizeDivision,
                    SizeDivisionDescTw = mis.SizeDivisionDescTw,
                    SizeDivisionDescEn = mis.SizeDivisionDescEn,
                    OrderVersion = mis.OrderVersion,
                    ParentMaterialId = mis.ParentMaterialId,
                    ModifyUserName = mis.ModifyUserName,
                    LastUpdateTime = mis.LastUpdateTime,
                    SemiGoods = m == null ? 0 : m.SemiGoods,
                    CategoryCodeId = m.CategoryCodeId
                } 
            );
            return mrpItemOrders;
        }
        public void CreateRange(IEnumerable<Models.Views.MRPItemOrders> ordersItems)
        {
            MRPItemOrdersBK.CreateRange(BuildRange(ordersItems));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MRPItemOrdersBK, bool>> predicate)
        {
            MRPItemOrdersBK.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MRPItemOrdersBK> BuildRange(IEnumerable<Models.Views.MRPItemOrders> items)
        {
            return items.Select(item => new ERP.Models.Entities.MRPItemOrdersBK
            {
                LocaleId = item.LocaleId,
                OrdersId = item.OrdersId,
                PartId = item.PartId,
                PartNo = item.PartNo,
                PartNameTw = item.PartNameTw,
                PartNameEn = item.PartNameEn,
                MaterialId = item.MaterialId,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                UnitCodeId = item.UnitCodeId,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                UnitTotal = item.UnitTotal,
                Total = item.Total,
                SizeDivision = item.SizeDivision,
                SizeDivisionDescTw = item.SizeDivisionDescTw,
                SizeDivisionDescEn = item.SizeDivisionDescEn,
                OrderVersion = item.OrderVersion,
                ParentMaterialId = item.ParentMaterialId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
            });
        }
    }
}