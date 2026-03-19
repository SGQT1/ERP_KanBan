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
using Microsoft.Extensions.Caching.Memory;

namespace ERP.Services.Business
{
    public class CacheService : BusinessService
    {
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.TypeService Type { get; set; }
        private ERP.Services.Business.Entities.CompanyService Company { get; set; }
        private ERP.Services.Business.Entities.CustomerService Customer { get; set; }
        private ERP.Services.Business.Entities.WarehouseService Warehouse { get; set; }
        private ERP.Services.Business.Entities.OrgUnitService OrgUnit { get; set; }

        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Business.Entities.BondProductChinaService BondProductChina { get; set; }
        private ERP.Services.Business.Entities.BondMaterialChinaService BondMaterialChina { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureVendorService MPSVendor { get; set; }
        private ERP.Services.Business.Entities.MPSProceduresService MPSProcedures { get; set; }
        private ERP.Services.Business.Entities.MPSOrdersService MPSOrders { get; set; }
        private IMemoryCache MemoryCache { get; set; }
        public CacheService(
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.TypeService typeService,
            ERP.Services.Business.Entities.CompanyService companyService,
            ERP.Services.Business.Entities.CustomerService customerService,
            ERP.Services.Business.Entities.WarehouseService warehouseService,
            ERP.Services.Business.Entities.OrgUnitService orgUnitService,
            ERP.Services.Business.Entities.MaterialService materialService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.VendorService vendorService,
            ERP.Services.Business.Entities.BondProductChinaService bondProductChinaService,
            ERP.Services.Business.Entities.BondMaterialChinaService bondMaterialChinaService,
            ERP.Services.Business.Entities.MPSProcedureVendorService mpsVendorService,
            ERP.Services.Business.Entities.MPSProceduresService mpsProceduresService,
            ERP.Services.Business.Entities.MPSOrdersService mpsOrdersService,
            IMemoryCache cache,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            CodeItem = codeItemService;
            Type = typeService;
            Company = companyService;
            Customer = customerService;
            Warehouse = warehouseService;
            OrgUnit = orgUnitService;

            Material = materialService;
            Orders = ordersService;
            Vendor = vendorService;

            MemoryCache = cache;
            BondProductChina = bondProductChinaService;
            BondMaterialChina = bondMaterialChinaService;
            MPSVendor = mpsVendorService;
            MPSProcedures = mpsProceduresService;
            MPSOrders = mpsOrdersService;
        }

        public Cache GetCache(int localeId)
        {
            
            var codeitem = CodeItem.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => new CodeItemCache
                {
                    Id = i.Id,
                    NameTW = i.NameTW,
                    NameEng = i.NameEng,
                    CodeType = i.CodeType,
                    CodeNo = i.CodeNo,
                    Disable = i.Disable
                })
                .OrderBy(i => i.CodeType)
                .ThenBy(i => i.NameTW)
                .ToList();

            var company = Company.Get()
                .Select(i => new CompanyCache
                {
                    Id = i.Id,
                    CompanyNo = i.CompanyNo,
                    Enable = i.Enable,
                    IsVirtual = i.IsVirtual,
                })
                .OrderBy(i => i.CompanyNo)
                .ToList();

            var customer = Customer.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => new CustomerCache
                {
                    Id = i.Id,
                    ChineseName = i.ChineseName,
                })
                .OrderBy(i => i.ChineseName)
                .ToList();

            var warehouse = Warehouse.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => new WarehouseCache {
                    Id = i.Id,
                    WarehouseNo = i.WarehouseNo,
                    Type = i.TypeCode,
                    LocaleId = i.LocaleId,
                })                
                .OrderBy(i => i.WarehouseNo)
                .ToList();

           var orgUnit = OrgUnit.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => new OrgUnitCache {
                    Id = i.Id,
                    UnitNameTw = i.UnitNameTw,
                    UnitNameEn = i.UnitNameEn
                })                
                .OrderBy(i => i.UnitNameTw)
                .ToList();
            
            var bondProduct = BondProductChina.Get()
                .Select(i => new BondProductCache {
                    Id = i.Id,
                    BondProductName = i.BondProductName,
                })                
                .OrderBy(i => i.BondProductName)
                .ToList();
            var bondMaterial = BondMaterialChina.Get()
                .Select(i => new BondMaterialCache {
                    Id = i.Id,
                    BondMaterialName = i.BondMaterialName,
                })                
                .OrderBy(i => i.BondMaterialName)
                .ToList();

            var mpsVendor = MPSVendor.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => new MPSVendorCache {
                    Id = i.Id,
                    ShortNameTw = i.ShortNameTw,
                    NameTw = i.NameTw,
                    NameLocal = i.NameTw
                })                
                .OrderBy(i => i.ShortNameTw)
                .ToList();
            
            var procedures = MPSProcedures.Get()
                .Where(i => i.LocaleId == localeId)
                .Select(i => new MPSProcedureCache {
                    Id = i.Id,
                    ProcedureNo = i.ProcedureNo,
                    ProcedureName = i.ProcedureName,
                })                
                .OrderBy(i => i.ProcedureName)
                .ToList();

            return new Cache
            {
                BooleanType = Type.GetBooleanType().ToList(),
                DivisionType = Type.GetDivisionType().ToList(),
                ProductType = Type.GetProductType().ToList(),
                OrderType = Type.GetOrderType().ToList(),
                PayType = Type.GetPayType().ToList(),
                DeliveryTerm = Type.GetDeliveryTerm().ToList(),
                PackingType = Type.GetPackingType().ToList(),
                TransitType = Type.GetTransitType().ToList(),
                OrderStatus = Type.GetOrderStatus().ToList(),
                POStatus = Type.GetPOStatus().ToList(),
                PriceType = Type.GetPriceType().ToList(),
                TaxType = Type.GetTaxType().ToList(),
                ShipmentType = Type.GetShipmentType().ToList(),
                POType = Type.GetPOType().ToList(),
                QuotationType = Type.GetQutotionType().ToList(),
                StockIOType = Type.GetStockIOType().ToList(),
                DailyType = Type.GetDailyType().ToList(),
                MaterialStockType = Type.GetMaterialStockType().ToList(),
                InspectResultType = Type.GetInspectResultType().ToList(),
                InspectType = Type.GetInspectType().ToList(),
                AccountType = Type.GetAccountType().ToList(),
                DeliveryType = Type.GetDeliveryType().ToList(),
                AlternateType = Type.GetAlternateType().ToList(),
                AlternateShowType = Type.GetAlternatesShowType().ToList(),
                TCType = Type.GetTCType().ToList(),
                MPSPOType = Type.GetMPSPOType().ToList(),
                MPSFeeType  = Type.GetMPSFeeType().ToList(),
                MPSAddFeeType  = Type.GetMPSAddFeeType().ToList(),
                MPSDeliveryType = Type.GetMPSDeliveryType().ToList(),
                RDPOType = Type.GetRDPOType().ToList(),

                CodeItem = codeitem,
                Company = company,
                Customer = customer,
                Warehouse = warehouse,
                OrgUnit = orgUnit,
                BondProduct = bondProduct,
                BondMaterial = bondMaterial,
                MPSVendor = mpsVendor,
                MPSProcedures = procedures,
            };
        }

        public void LoadCache(int localeId) {
            // var materialStartTime = DateTime.Now;
            // var materials = Material.GetCache().Where(i => i.LocaleId == localeId).ToList();
            // MemoryCache.Set("MaterialCache", materials);

            // var ordersStartTime = DateTime.Now;
            // var orders = Orders.GetCache().Where(i => i.LocaleId == localeId).ToList();
            // MemoryCache.Set("OrdersCache", orders);

            // var mpsOrdersStartTime = DateTime.Now;
            // var mpsOrders = MPSOrders.GetCache().Where(i => i.LocaleId == localeId).ToList();
            // MemoryCache.Set("MPSOrdersCache", mpsOrders);

            // var vendorStartTime = DateTime.Now;
            // var vendors = Vendor.GetVendorCache().ToList();
            // MemoryCache.Set("VendorCache", vendors);
           
            // var completedTime = DateTime.Now;
            
            // Console.WriteLine("======Load Cache=======");
            // Console.WriteLine($"[CACHE INIT] Start loading material cache at {materialStartTime:yyyy-MM-dd HH:mm:ss}");
            // Console.WriteLine($"[CACHE INIT] Loaded {materials.Count} materials into cache.");
            // Console.WriteLine($"[CACHE INIT] Start loading orders cache at {ordersStartTime:yyyy-MM-dd HH:mm:ss}");
            // Console.WriteLine($"[CACHE INIT] Loaded {orders.Count} orders into cache.");
            // Console.WriteLine($"[CACHE INIT] Start loading mpsOrders cache at {mpsOrdersStartTime:yyyy-MM-dd HH:mm:ss}");
            // Console.WriteLine($"[CACHE INIT] Loaded {mpsOrders.Count} mpsOrders into cache.");
            // Console.WriteLine($"[CACHE INIT] Start loading vendors cache at {vendorStartTime:yyyy-MM-dd HH:mm:ss}");
            // Console.WriteLine($"[CACHE INIT] Loaded {vendors.Count} vendors into cache.");
            // Console.WriteLine($"[CACHE INIT] Completed at {completedTime:yyyy-MM-dd HH:mm:ss}");
        }

        public void LoadMaterialCache(int localeId) {

            var materialStartTime = DateTime.Now;
            var materials = Material.GetCache().Where(i => i.LocaleId == localeId).ToList();
            MemoryCache.Set("MaterialCache", materials);
            
            var completedTime = DateTime.Now;
            Console.WriteLine("======Load Cache=======");
            Console.WriteLine($"[CACHE INIT] Start loading material cache at {materialStartTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"[CACHE INIT] Loaded {materials.Count} materials into cache.");
            Console.WriteLine($"[CACHE INIT] Completed at {completedTime:yyyy-MM-dd HH:mm:ss}");
        }

        public void LoadOrdersCache(int localeId) {

            var ordersStartTime = DateTime.Now;
            var orders = Orders.GetCache().Where(i => i.LocaleId == localeId).ToList();
            MemoryCache.Set("OrdersCache", orders);

            var completedTime = DateTime.Now;
            Console.WriteLine("======Load Cache=======");
            Console.WriteLine($"[CACHE INIT] Start loading orders cache at {ordersStartTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"[CACHE INIT] Loaded {orders.Count} orders into cache.");
            Console.WriteLine($"[CACHE INIT] Completed at {completedTime:yyyy-MM-dd HH:mm:ss}");
        }

        public void LoadVendorCache(int localeId) {
            var vendorStartTime = DateTime.Now;
            var vendors = Vendor.GetVendorCache().ToList();
            MemoryCache.Set("VendorCache", vendors);

            var completedTime = DateTime.Now;
            Console.WriteLine("======Load Cache=======");
            Console.WriteLine($"[CACHE INIT] Start loading vendors cache at {vendorStartTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"[CACHE INIT] Loaded {vendors.Count} vendors into cache.");
            Console.WriteLine($"[CACHE INIT] Completed at {completedTime:yyyy-MM-dd HH:mm:ss}");
        }

        public void LoadMPSOrdersCache(int localeId) {
            var mpsOrdersStartTime = DateTime.Now;
            var mpsOrders = MPSOrders.Get().Where(i => i.LocaleId == localeId).ToList();
            MemoryCache.Set("MPSOrdersCache", mpsOrders);

            var completedTime = DateTime.Now;
            Console.WriteLine("======Load Cache=======");
            Console.WriteLine($"[CACHE INIT] Start loading mps-orders cache at {mpsOrdersStartTime:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"[CACHE INIT] Loaded {mpsOrders.Count} mpsOrders into cache.");
            Console.WriteLine($"[CACHE INIT] Completed at {completedTime:yyyy-MM-dd HH:mm:ss}");
        }
    }
}