using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class ManagementService : SearchService
    {

        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }

        private ERP.Services.Business.MaterialStockService MaterialStock { get; }
        private ERP.Services.Business.Entities.MaterialStockItemService MaterialStockItem { get; }

        private ERP.Services.Entities.StockIOService StockIO { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Entities.WarehouseService Warehouse { get; set; }

        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.PurBatchItemService PurBatchItem { get; set; }
        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.POItemService POItem { get; set; }
        private ERP.Services.Entities.ReceivedLogService ReceivedLog { get; set; }
        public ManagementService(

            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.MaterialStockService materialStockService,
            ERP.Services.Business.Entities.MaterialStockItemService materialStockItemService,
            ERP.Services.Entities.StockIOService stockIOService,
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.WarehouseService warehouseService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.POItemService poItemService,
            ERP.Services.Entities.ReceivedLogService receivedLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            MaterialStock = materialStockService;
            MaterialStockItem = materialStockItemService;
            StockIO = stockIOService;
            Material = materialService;
            Warehouse = warehouseService;

            Company = companyService;
            PurBatchItem = purBatchItemService;
            PO = poService;
            POItem = poItemService;
            ReceivedLog = receivedLogService;
        }
        //業務key單查詢
        public IQueryable<Models.Views.OrdersRecord> GetOrderRecordSummary(string predicate)
        {

            var items = Orders.GetEntity().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new
            {
                Year = ((DateTime)i.KeyInDate).Year,
                Month = ((DateTime)i.KeyInDate).Month,
                CompanyNo = i.CompanyNo,
                CompanyId = i.CompanyId,
                Brand = i.Brand,
                BrandCodeId = i.BrandCodeId
            })
            .GroupBy(i => new { i.Year, i.Month, i.CompanyNo, i.CompanyId, i.Brand, i.BrandCodeId })
            .Select(i => new
            {
                Year = i.Key.Year,
                Month = i.Key.Month,
                CompanyNo = i.Key.CompanyNo,
                CompanyId = i.Key.CompanyId,
                Brand = i.Key.Brand,
                BrandCodeId = i.Key.BrandCodeId,
                Records = i.Count()
            })
            .ToList();


            var result = items.Select(i => new Models.Views.OrdersRecord
            {
                CompanyNo = i.CompanyNo,
                CompanyId = i.CompanyId,
                Brand = i.Brand,
                BrandCodeId = i.BrandCodeId,
                Records = i.Records,
                KeyInMonth = Convert.ToDecimal(i.Year.ToString("0000") + i.Month.ToString("00"))
            })
                .ToList();

            return result.AsQueryable();

        }
        //業務key單查詢-表身
        public IQueryable<Models.Views.Orders> GetOrders(string predicate)
        {
            var result = Orders.GetEntity().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate).ToList();

            return result.AsQueryable();
        }
        //批次材料首批出庫
        public IQueryable<Models.Views.MaterialStockItem> GerMaterialFistStockOut(string predicate, string[] filters)
        {
            var extendPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("LocaleId") || i.Contains("SourceType")).ToArray();
                extendPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var firtStockOutIds = StockIO.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate).GroupBy(g => g.OrderNo).Select(i => i.Min(g => g.Id));

            var result = (
                from s in StockIO.Get()
                join m in Material.Get() on new { MaterialId = s.MaterialId, LocaleId = s.LocaleId } equals new { MaterialId = m.Id, LocaleId = m.LocaleId } into mGRP
                from m in mGRP.DefaultIfEmpty()
                join w in Warehouse.Get() on new { WarehouseId = s.WarehouseId, LocaleId = s.LocaleId } equals new { WarehouseId = w.Id, LocaleId = w.LocaleId } into wGRP
                from w in wGRP.DefaultIfEmpty()
                select new Models.Views.MaterialStockItem
                {
                    Id = s.Id,
                    LocaleId = s.LocaleId,
                    MaterialId = s.MaterialId,
                    MaterialName = m.MaterialName,
                    MaterialNameEng = m.MaterialNameEng,
                    WarehouseId = s.WarehouseId,
                    WarehouseNo = w.WarehouseNo,
                    SourceType = s.SourceType,
                    IODate = s.IODate,
                    OrderNo = s.OrderNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Where(i => firtStockOutIds.Contains(i.Id))
            .ToList();

            // var result = MaterialStockItem.Get()
            //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            //     .Where(i => firtStockOutIds.Contains(i.Id))
            //     .ToList();

            return result.AsQueryable();
        }
        // public IQueryable<Models.Views.POItem> GetValidPOItems(string predicate, string[] filters)
        // {
        //    var result= (


        //    )
        //     .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
        //     .ToList();
        //     return result;

        // }
    }
}
