using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views.Common;

namespace ERP.Models.Views
{
    public class Cache
    {
        public List<BooleanType> BooleanType { get; set; }
        public List<DivisionType> DivisionType { get; set; }
        public List<ProductType> ProductType { get; set; }
        public List<OrderType> OrderType { get; set; }
        public List<PayType> PayType { get; set; }
        public List<DeliveryTerm> DeliveryTerm { get; set; }
        public List<PackingType> PackingType { get; set; }
        public List<TransitType> TransitType { get; set; }
        public List<OrderStatus> OrderStatus { get; set; }
        public List<POStatus> POStatus { get; set; }
        public List<TaxType> TaxType { get; set; }
        public List<PriceType> PriceType { get; set; }
        public List<ShipmentType> ShipmentType { get; set; }
        public List<POType> POType { get; set; }
        public List<QuotationType> QuotationType { get; set; }
        public List<StockIOType> StockIOType { get; set; }
        public List<DailyType> DailyType { get; set; }
        public List<MaterialStockType> MaterialStockType { get; set; }
        public List<InspectResultType> InspectResultType { get; set; }
        public List<InspectType> InspectType { get; set; }
        public List<AccountType> AccountType { get; set; }
        public List<DeliveryType> DeliveryType { get; set; }
        public List<AlternateType> AlternateType { get; set; }
        public List<AlternateType> AlternateShowType { get; set; }
        public List<TCType> TCType { get; set; }
        public List<MPSPOType> MPSPOType { get; set; }
        public List<MPSFeeType> MPSFeeType { get; set; }
        public List<MPSAddFeeType> MPSAddFeeType { get; set; }
        public List<MPSDeliveryType> MPSDeliveryType { get; set; }
        public List<RDPOType> RDPOType { get; set; }
        

        public List<CodeItemCache> CodeItem { get; set; }
        public List<CompanyCache> Company { get; set; }
        public List<CustomerCache> Customer { get; set; }
        public List<WarehouseCache> Warehouse { get; set; }
        public List<OrgUnitCache> OrgUnit { get; set; }
        public List<BondProductCache> BondProduct { get; set; }
        public List<BondMaterialCache> BondMaterial { get; set; }
        public List<Vendor> Vendor { get; set; }
        public List<MPSVendorCache> MPSVendor { get; set; }
        public List<MPSProcedureCache> MPSProcedures { get; set; }
    }

    public class CodeItemCache
    {
        public decimal Id { get; set; }
        public string CodeType { get; set; }
        public int CodeNo { get; set; }
        public string NameTW { get; set; }
        public string NameEng { get; set; }
        public bool Disable { get; set; }

    }
    public class CompanyCache
    {
        public decimal Id { get; set; }
        public string CompanyNo { get; set; }
        public int? Enable { get; set; }
        public int? IsVirtual { get; set; }
        public bool Disable { get; set; }
    }
    public class CustomerCache
    {
        public decimal Id { get; set; }
        public string ChineseName { get; set; }
        public bool Disable { get; set; }
    }
    public class WarehouseCache
    {
        public decimal Id { get; set; }
        public string WarehouseNo { get; set; }
        public int Type { get; set; }
        public decimal LocaleId { get; set; }
        public bool Disable { get; set; }
    }
    public class OrgUnitCache
    {
        public decimal Id { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public bool Disable { get; set; }
    }
    public class LastCache
    {
        public decimal Id { get; set; }
        public string LastNo { get; set; }
        public bool Disable { get; set; }
    }
    public class OutsoleCache
    {
        public decimal Id { get; set; }
        public string OutsoleNo { get; set; }
        public bool Disable { get; set; }
    }
    public class BondProductCache
    {
        public decimal Id { get; set; }
        public string BondProductName { get; set; }
        public bool Disable { get; set; }
    }
    public class BondMaterialCache
    {
        public decimal Id { get; set; }
        public string BondMaterialName { get; set; }
        public bool Disable { get; set; }
    }
    public class MPSVendorCache
    {
        public decimal Id { get; set; }
        public string ShortNameTw { get; set; }
        public string NameTw { get; set; }
        public string NameLocal { get; set; }
        public bool Disable { get; set; }
    }
    public class MPSProcedureCache
    {
        public decimal Id { get; set; }
        public string ProcedureNo { get; set; }
        public string ProcedureName { get; set; }
        public bool Disable { get; set; }
    }
}