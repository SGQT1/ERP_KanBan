using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business.Entities
{
    public class NBMaterialService : BusinessService
    {
        private Services.Entities.NBMaterialService NBMaterial { get; }
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.StyleItemService StyleItem { get; }
        private Services.Entities.MaterialService Material { get; }
        private Services.Entities.MRPItemService MRPItem { get; }

        public NBMaterialService(
            Services.Entities.NBMaterialService nbMaterialService,
            Services.Entities.OrdersService ordersService,
            Services.Entities.StyleItemService styleItemService,
            Services.Entities.MaterialService materialService,
            Services.Entities.MRPItemService mrpItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            NBMaterial = nbMaterialService;
            Orders = ordersService;
            StyleItem = styleItemService;
            Material = materialService;
            MRPItem = mrpItemService;
        }
        public IQueryable<Models.Views.NBMaterial> Get()
        {
            return NBMaterial.Get().Select(i => new Models.Views.NBMaterial
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                MaterialId = i.MaterialId,
                MaterialCode = i.MaterialCode,
                CommodityType = i.CommodityType,
                Description = i.Description,
                UOM = i.UOM,
                ColorKey = i.ColorKey,
                NBColorName = i.NBColorName,
                ColorFamily = i.ColorFamily,
                VendorName = i.VendorName,
                VendorCode = i.VendorCode,
                // MaterialName = i.MaterialName,
                // MaterialNameEng = i.MaterialNameEng,
            });
        }

        public IQueryable<Models.Views.NBMaterial> GetWithOrders(string predicate)
        {
            var result = (
                from o in Orders.Get()
                    // join si in StyleItem.Get() on new { StyleId = o.StyleId, LocaleId = o.LocaleId } equals new { StyleId = si.StyleId, LocaleId = si.LocaleId }
                join si in MRPItem.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = si.OrdersId, LocaleId = si.LocaleId }
                join m in Material.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId }
                join n in NBMaterial.Get() on new { MaterialId = si.MaterialId, LocaleId = si.LocaleId } equals new { MaterialId = n.MaterialId, LocaleId = n.LocaleId } into nGRP
                from n in nGRP.DefaultIfEmpty()
                select new
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    CSD = o.CSD,
                    BrandCodeId = o.BarcodeCodeId,
                    Brand = o.Brand,
                    StyleNo = o.StyleNo,
                    OrderNo = o.OrderNo,
                    MaterialId = m.Id,
                    MaterialName = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,

                    MaterialCode = (string?)n.MaterialCode,
                    ColorKey = (string?)n.ColorKey,
                    Description = (string?)n.Description,
                    VendorCode = (string?)n.VendorCode,
                    VendorName = (string?)n.VendorName,
                    UOM = (string?)n.UOM,
                    CommodityType = (string?)n.CommodityType,
                    NBColorName = (string?)n.NBColorName,
                    ColorFamily = (string?)n.ColorFamily,
                    ModifyUserName = (string?)n.ModifyUserName,
                    LastUpdateTime = (DateTime?)n.LastUpdateTime,
                    NBMaterialId = (decimal?)n.Id,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.NBMaterial
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                MaterialId = i.MaterialId,
                MaterialCode = i.MaterialCode,
                CommodityType = i.CommodityType,
                Description = i.Description,
                UOM = i.UOM,
                ColorKey = i.ColorKey,
                NBColorName = i.NBColorName,
                ColorFamily = i.ColorFamily,
                VendorName = i.VendorName,
                VendorCode = i.VendorCode,
                MaterialName = i.MaterialName,
                MaterialNameEng = i.MaterialNameEng,
            })
            .Distinct();
            return result;
        }
        public void CreateRange(IEnumerable<Models.Views.NBMaterial> items)
        {
            NBMaterial.CreateRange(StockOutBuildRange(items));
        }
        public void RemoveRange(List<decimal> items, decimal localeId)
        {
            NBMaterial.RemoveRange(i => items.Contains(i.MaterialId) && i.LocaleId == localeId);
        }
        public void ExecuteSqlCommand()
        {
            string sql = @"
                            UPDATE a
                            SET
                                a.CommodityType = b.CommodityType,
                                a.Description = b.Description,
                                a.UOM = b.UOM,
                                a.NBColorName = b.NBColorName,
                                a.ColorFamily = b.ColorFamily,
                                a.VendorName = b.VendorName,
                                a.VendorCode = b.VendorCode
                            FROM NBMaterial AS a
                            JOIN NBPPM AS b ON a.MaterialCode = b.MaterialCode AND a.ColorKey = b.ColorKey
                        ";

            NBMaterial.ExecuteSqlCommand(sql);
        }
        private IEnumerable<Models.Entities.NBMaterial> StockOutBuildRange(IEnumerable<Models.Views.NBMaterial> items)
        {
            return items.Select(item => new Models.Entities.NBMaterial
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime ?? DateTime.Now,
                MaterialId = item.MaterialId,
                MaterialCode = item.MaterialCode,
                CommodityType = item.CommodityType,
                Description = item.Description,
                UOM = item.UOM,
                ColorKey = item.ColorKey,
                NBColorName = item.NBColorName,
                ColorFamily = item.ColorFamily,
                VendorName = item.VendorName,
                VendorCode = item.VendorCode,
            });
        }

    }
}