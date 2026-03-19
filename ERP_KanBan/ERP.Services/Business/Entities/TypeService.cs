using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class TypeService : BusinessService
    {
        public TypeService(UnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<BooleanType> GetBooleanType()
        { //1 = Yes, 0 = No
            var types = new List<BooleanType>
            {
                new BooleanType() { Id = 1, Key = "Yes", NameTw = "是", NameEn = "Y" },
                new BooleanType() { Id = 0, Key = "No", NameTw = "否", NameEn = "N" },
            };
            return types.AsQueryable();
        }
        public IQueryable<ProductType> GetProductType()
        { //1 = 銷樣, 2 = 量產, 3 = 半成品, 4=預購
            var types = new List<ProductType>
            {
                new ProductType() { Id = 1, Key = "SalesmanSample", NameTw = "銷樣", NameEn = "Salesman Sample" },
                new ProductType() { Id = 2, Key = "Production", NameTw = "量產", NameEn = "Production" },
                new ProductType() { Id = 3, Key = "Component", NameTw = "半成品", NameEn = "Component" },
                new ProductType() { Id = 4, Key = "PreOrder", NameTw = "預購", NameEn = "PreOrder" },
            };
            return types.AsQueryable();
        }
        public IQueryable<OrderType> GetOrderType()
        { //1 = 預告訂單, 2 = 正視訂單,
            var types = new List<OrderType>
            {
                new OrderType() { Id = 1, Key = "Forecast", NameTw = "預告訂單", NameEn = "Forecast" },
                new OrderType() { Id = 2, Key = "Official", NameTw = "正式訂單", NameEn = "Official" },
            };
            return types.AsQueryable();
        }
        public IQueryable<PayType> GetPayType()
        { //0 = 月結, 1 = 出貨前T/T, 2 = 出貨後T/T, 3 = L/C
            var types = new List<PayType>
            {
                new PayType() { Id = 0, Key = "Net30", NameTw = "月結", NameEn = "Net30" },
                new PayType() { Id = 1, Key = "BeforeTT", NameTw = "出貨前T/T", NameEn = "Before T/T" },
                new PayType() { Id = 2, Key = "AftertTT", NameTw = "出貨後T/T", NameEn = "After T/T" },
                new PayType() { Id = 3, Key = "LC", NameTw = "L/C", NameEn = "L/C" },
                new PayType() { Id = 4, Key = "Cheque", NameTw = "Cheque", NameEn = "Cheque" },
            };
            return types.AsQueryable();
        }
        public IQueryable<TransitType> GetTransitType()
        { //0= 海運 ,1 = 空運, 2 = 路運, 3=其他
            var types = new List<TransitType>
            {
                new TransitType() { Id = 0, Key = "Sea", NameTw = "海運", NameEn = "Sea" },
                new TransitType() { Id = 1, Key = "Air", NameTw = "空運", NameEn = "Air" },
                new TransitType() { Id = 2, Key = "Land", NameTw = "陸運", NameEn = "Land" },
                new TransitType() { Id = 4, Key = "Sea&Air", NameTw = "海空聯運", NameEn = "Sea & Air" },
                new TransitType() { Id = 5, Key = "Exspress", NameTw = "快遞", NameEn = "Exspress" },
                new TransitType() { Id = 3, Key = "Others", NameTw = "其他", NameEn = "Others" },
            };
            return types.AsQueryable();
        }
        public IQueryable<DeliveryTerm> GetDeliveryTerm()
        {
            var types = new List<DeliveryTerm>
            {
                new DeliveryTerm() { Id = 0, Key = "EXW", NameTw = "EXW", NameEn = "EXW" },
                new DeliveryTerm() { Id = 1, Key = "FAS", NameTw = "FAS/T", NameEn = "FAS" },
                new DeliveryTerm() { Id = 2, Key = "FCA", NameTw = "FCA", NameEn = "FCA" },
                new DeliveryTerm() { Id = 3, Key = "FOB", NameTw = "FOB", NameEn = "FOB" },
                new DeliveryTerm() { Id = 4, Key = "CFR", NameTw = "CFR", NameEn = "CFR" },
                new DeliveryTerm() { Id = 5, Key = "CIF", NameTw = "CIF", NameEn = "CIF" },
                new DeliveryTerm() { Id = 6, Key = "CIP", NameTw = "CIP", NameEn = "CIP" },
                new DeliveryTerm() { Id = 7, Key = "CPT", NameTw = "CPT", NameEn = "CPT" },
                new DeliveryTerm() { Id = 8, Key = "DAF", NameTw = "DAF", NameEn = "DAF" },
                new DeliveryTerm() { Id = 9, Key = "DDP", NameTw = "DDP", NameEn = "DDP" },
                new DeliveryTerm() { Id = 10, Key = "DDU", NameTw = "DDU", NameEn = "DDU" },
                new DeliveryTerm() { Id = 11, Key = "DEQ", NameTw = "DEQ", NameEn = "DEQ" },
                new DeliveryTerm() { Id = 12, Key = "DEQ", NameTw = "DEQ", NameEn = "DEQ" },
                new DeliveryTerm() { Id = 13, Key = "C&F", NameTw = "C&F", NameEn = "C&F" },
            };
            return types.AsQueryable();
        }
        public IQueryable<PackingType> GetPackingType()
        { //0 = 單號裝, 1 = 混裝,
            var types = new List<PackingType>
            {
                new PackingType() { Id = 0, Key = "Solid", NameTw = "單號裝", NameEn = "Solid" },
                new PackingType() { Id = 1, Key = "Mix", NameTw = "混裝", NameEn = "Mix" },
            };
            return types.AsQueryable();
        }
        public IQueryable<OrderStatus> GetOrderStatus()
        { //狀態(0=準備中;1=生產中;2=結案;3=作廢)
            var types = new List<OrderStatus>
            {
                new OrderStatus() { Id = 0, Key = "Preparing", NameTw = "準備中", NameEn = "Preparing" },
                new OrderStatus() { Id = 1, Key = "InProduction", NameTw = "生產中", NameEn = "InProduction" },
                new OrderStatus() { Id = 2, Key = "Closed", NameTw = "結案", NameEn = "Closed" },
                new OrderStatus() { Id = 3, Key = "Cancel", NameTw = "作廢", NameEn = "Cancel" },
            };
            return types.AsQueryable();
        }
        public IQueryable<POStatus> GetPOStatus()
        { //狀態(0=結案;1=進行中;2=作廢)
            var types = new List<POStatus>
            {
                new POStatus() { Id = 0, Key = "Closed", NameTw = "結案", NameEn = "Closed" },
                new POStatus() { Id = 1, Key = "Activity", NameTw = "進行中", NameEn = "Activity" },
                new POStatus() { Id = 2, Key = "Cancel", NameTw = "作廢", NameEn = "Cancel" },
            };
            return types.AsQueryable();
        }
        public IQueryable<PriceType> GetPriceType()
        {
            //狀態(0=international;1=sub;)
            var types = new List<PriceType>
            {
                new PriceType() { Id = 1, Key = "INTERNATIONAL", NameTw = "INTERNATIONAL", NameEn = "INTERNATIONAL" },
                new PriceType() { Id = 2, Key = "SUB", NameTw = "SUB", NameEn = "SUB" },
            };
            return types.AsQueryable();
        }
        public IQueryable<TaxType> GetTaxType()
        {
            //狀態(0=免稅;1=應稅;2=零稅率)
            var types = new List<TaxType>
            {
                new TaxType() { Id = 0, Key = "TaxExemption", NameTw = "免稅", NameEn = "Tax Exemption" },
                new TaxType() { Id = 1, Key = "Tax", NameTw = "應稅", NameEn = "Tax" },
                new TaxType() { Id = 2, Key = "ZeroTaxRate", NameTw = "零稅率", NameEn = "Zero Tax Rate" },
            };
            return types.AsQueryable();
        }
        public IQueryable<ShipmentType> GetShipmentType()
        {
            //狀態(0=Export;1=應稅;)
            var types = new List<ShipmentType>
            {
                new ShipmentType() { Id = 1, Key = "Export", NameTw = "外銷", NameEn = "Export" },
                new ShipmentType() { Id = 2, Key = "Domestic", NameTw = "內銷", NameEn = "Domestic" },
            };
            return types.AsQueryable();
        }
        public IQueryable<POType> GetPOType()
        {
            //1=批次採購, 2=託外加工採購, 3=指定採購, 4=單筆採購, 5=補單採購, 6=補料託外加工採購)
            var types = new List<POType>
            {
                new POType() { Id = 1, Key = "1", NameTw = "(正單)批次採購", NameEn = "(批次採購)Batch" },
                new POType() { Id = 2, Key = "2", NameTw = "(正單)託外加工採購", NameEn = "(託外加工)Outsourcing" },
                new POType() { Id = 3, Key = "3", NameTw = "(正單)指定採購", NameEn = "(指定採購)Special" },
                new POType() { Id = 4, Key = "4", NameTw = "(正單)單筆採購", NameEn = "(單筆採購)Single" },
                new POType() { Id = 5, Key = "5", NameTw = "(補單)補單採購", NameEn = "(補單)Addition" },
                new POType() { Id = 6, Key = "6", NameTw = "(補單)補料託外加工採購", NameEn = "(補料託外)Addition Outsourcing" },
            };
            return types.AsQueryable();
        }
        public IQueryable<QuotationType> GetQutotionType()
        {
            var types = new List<QuotationType>
            {
                new QuotationType() { Id = 1, Key = "Production", NameTw = "量產", NameEn = "Production" },
                new QuotationType() { Id = 2, Key = "Sample", NameTw = "樣品", NameEn = "Sample" },
                // new QuotationType() { Id = 3, Key = "XXX", NameTw = "XXX", NameEn = "XXX" },
            };
            return types.AsQueryable();
        }
        public IQueryable<StockIOType> GetStockIOType()
        {
            var types = new List<StockIOType>
            {
                new StockIOType() { Id = 0, Key = "來料入庫", NameTw = "來料入庫", NameEn = "來料入庫" },
                new StockIOType() { Id = 1, Key = "分批領料出庫", NameTw = "分批領料出庫", NameEn = "分批領料出庫" },
                new StockIOType() { Id = 2, Key = "轉廠入庫", NameTw = "轉廠入庫", NameEn = "轉廠入庫" },
                new StockIOType() { Id = 3, Key = "轉廠出庫", NameTw = "轉廠出庫", NameEn = "轉廠出庫" },
                new StockIOType() { Id = 4, Key = "託外入庫", NameTw = "託外入庫", NameEn = "託外入庫" },
                new StockIOType() { Id = 5, Key = "託外出庫", NameTw = "託外出庫", NameEn = "託外出庫" },
                new StockIOType() { Id = 6, Key = "退料入庫", NameTw = "退料入庫", NameEn = "退料入庫" },
                new StockIOType() { Id = 7, Key = "補料出庫", NameTw = "補料出庫", NameEn = "補料出庫" },
                new StockIOType() { Id = 8, Key = "其他入庫", NameTw = "其他入庫", NameEn = "其他入庫" },
                new StockIOType() { Id = 9, Key = "其他出庫", NameTw = "其他出庫", NameEn = "其他出庫" },
                new StockIOType() { Id = 10, Key = "退貨出庫", NameTw = "退貨出庫", NameEn = "退貨出庫" },
                new StockIOType() { Id = 11, Key = "期末調整", NameTw = "期末調整", NameEn = "期末調整" },
                new StockIOType() { Id = 12, Key = "共用領料出庫", NameTw = "共用領料出庫", NameEn = "共用領料出庫" },
                new StockIOType() { Id = 13, Key = "轉可用庫存", NameTw = "轉可用庫存", NameEn = "轉可用庫存" },
                new StockIOType() { Id = 14, Key = "轉批次庫存", NameTw = "轉批次庫存", NameEn = "轉批次庫存" },
                new StockIOType() { Id = 15, Key = "批次庫存互轉", NameTw = "批次庫存互轉", NameEn = "批次庫存互轉" },
            };
            return types.AsQueryable();
        }
        public IQueryable<DailyType> GetDailyType()
        {
            var types = new List<DailyType>
            {
                new DailyType() { Id = 1, Key = "PlanDispatch", NameTw = "計畫派工", NameEn = "Plan" },
                new DailyType() { Id = 2, Key = "BalanceDispatch", NameTw = "差異派工", NameEn = "Balance" },
                new DailyType() { Id = 3, Key = "AddDispatch", NameTw = "補料派工", NameEn = "Add" },
                new DailyType() { Id = 4, Key = "OthersDispatch", NameTw = "其他派工", NameEn = "Others" },
            };
            return types.AsQueryable();
        }

        public IQueryable<MaterialStockType> GetMaterialStockType()
        {
            var types = new List<MaterialStockType>
            {
                new MaterialStockType() { Id = 1, Key = "Develop", NameTw = "開發", NameEn = "Develop" },
                new MaterialStockType() { Id = 2, Key = "Material", NameTw = "原物料", NameEn = "Material" },
                new MaterialStockType() { Id = 3, Key = "Component", NameTw = "半成品", NameEn = "Component" },
                new MaterialStockType() { Id = 4, Key = "Product", NameTw = "成品", NameEn = "Product" },
                new MaterialStockType() { Id = 5, Key = "General Affairs", NameTw = "總務", NameEn = "General Affairs" },
                new MaterialStockType() { Id = 6, Key = "Tooling", NameTw = "模製具", NameEn = "Tooling" },
                new MaterialStockType() { Id = 7, Key = "Scrap", NameTw = "報廢", NameEn = "Scrap" },
                new MaterialStockType() { Id = 8, Key = "Outsource", NameTw = "委外代工", NameEn = "Outsource" },
                new MaterialStockType() { Id = 9, Key = "B Grade", NameTw = "瑕疵品", NameEn = "B Grade" },
            };
            return types.AsQueryable();
        }
        public IQueryable<InspectResultType> GetInspectResultType()
        {
            var types = new List<InspectResultType>
            {
                new InspectResultType() { Id = 0, Key = "0", NameTw = "待決定", NameEn = "Develop" },
                new InspectResultType() { Id = 1, Key = "1", NameTw = "退貨", NameEn = "Develop" },
                new InspectResultType() { Id = 2, Key = "2", NameTw = "允收", NameEn = "Material" },
                new InspectResultType() { Id = 3, Key = "3", NameTw = "特採", NameEn = "Component" },
            };
            return types.AsQueryable();
        }
        public IQueryable<InspectType> GetInspectType()
        {
            var types = new List<InspectType>
            {
                new InspectType() { Id = 0, Key = "0", NameTw = "全檢", NameEn = "全檢" },
                new InspectType() { Id = 1, Key = "1", NameTw = "抽驗", NameEn = "抽驗" },
                new InspectType() { Id = 2, Key = "2", NameTw = "免驗", NameEn = "免驗" },
            };
            return types.AsQueryable();
        }
        public IQueryable<AccountType> GetAccountType()
        {
            var types = new List<AccountType>
            {
                new AccountType() { Id = 0, Key = "0", NameTw = "列帳", NameEn = "列帳" },
                new AccountType() { Id = 1, Key = "1", NameTw = "補貨不列帳", NameEn = "補貨不列帳" },
                new AccountType() { Id = 2, Key = "2", NameTw = "轉廠不列帳", NameEn = "轉廠不列帳" },
            };
            return types.AsQueryable();
        }
        public IQueryable<DeliveryType> GetDeliveryType()
        {
            var types = new List<DeliveryType>
            {
                new DeliveryType() { Id = 0, Key = "0", NameTw = "檢驗入庫", NameEn = "檢驗入庫" },
                new DeliveryType() { Id = 1, Key = "1", NameTw = "收貨清點", NameEn = "收貨清點" },
            };
            return types.AsQueryable();
        }
        public IQueryable<AlternateType> GetAlternateType()
        {
            var types = new List<AlternateType>
            {
                new AlternateType() { Id = 0, Key = "0", NameTw = "訂單", NameEn = "Orders" },
                new AlternateType() { Id = 1, Key = "1", NameTw = "斬刀", NameEn = "Knife" },
                new AlternateType() { Id = 2, Key = "2", NameTw = "大底", NameEn = "Outsole" },
                new AlternateType() { Id = 3, Key = "3", NameTw = "楦頭", NameEn = "Last" },
                new AlternateType() { Id = 4, Key = "4", NameTw = "鞋墊", NameEn = "Shell" },
                new AlternateType() { Id = 5, Key = "5", NameTw = "其他1", NameEn = "Other1" },
                new AlternateType() { Id = 6, Key = "6", NameTw = "其它2", NameEn = "Other2" },
            };
            return types.AsQueryable();
        }
        public IQueryable<AlternateType> GetAlternatesShowType()
        {
            var types = new List<AlternateType>
            {
                new AlternateType() { Id = 0, Key = "0", NameTw = "", NameEn = "" },
                new AlternateType() { Id = 1, Key = "1", NameTw = "斬刀", NameEn = "Knife" },
                new AlternateType() { Id = 2, Key = "2", NameTw = "大底", NameEn = "Outsole" },
                new AlternateType() { Id = 3, Key = "3", NameTw = "楦頭", NameEn = "Last" },
                new AlternateType() { Id = 4, Key = "4", NameTw = "鞋墊", NameEn = "Shell" },
                new AlternateType() { Id = 5, Key = "5", NameTw = "其他1", NameEn = "Other1" },
                new AlternateType() { Id = 6, Key = "6", NameTw = "其它2", NameEn = "Other2" },
            };
            return types.AsQueryable();
        }
        public IQueryable<TCType> GetTCType()
        {
            var types = new List<TCType>
            {
                new TCType() { Id = 1, Key = "1", NameTw = "大底", NameEn = "Outsole" },
                new TCType() { Id = 2, Key = "2", NameTw = "中底", NameEn = "MID Outsole" },
                new TCType() { Id = 3, Key = "3", NameTw = "EVA", NameEn = "EVA" },
            };
            return types.AsQueryable();
        }
        public IQueryable<MPSPOType> GetMPSPOType()
        {
            var types = new List<MPSPOType>
            {
                new MPSPOType() { Id = 1, Key = "1", NameTw = "正單", NameEn = "正單" },
                new MPSPOType() { Id = 2, Key = "2", NameTw = "補單", NameEn = "補單" },
            };
            return types.AsQueryable();
        }
        public IQueryable<MPSFeeType> GetMPSFeeType()
        {
            var types = new List<MPSFeeType>
            {
                new MPSFeeType() { Id = 1, Key = "1", NameTw = "代工費(Fee)", NameEn = "代工費(Fee)" },
                new MPSFeeType() { Id = 2, Key = "2", NameTw = "含工代料(Cost)", NameEn = "含工代料(Cost)" },
            };
            return types.AsQueryable();
        }
        public IQueryable<MPSDeliveryType> GetMPSDeliveryType()
        {
            var types = new List<MPSDeliveryType>
            {
                new MPSDeliveryType() { Id = 0, Key = "0", NameTw = "驗收入庫", NameEn = "驗收入庫" },
                new MPSDeliveryType() { Id = 1, Key = "1", NameTw = "來料收貨", NameEn = "驗收入庫" },
            };
            return types.AsQueryable();
        }
        public IQueryable<MPSAddFeeType> GetMPSAddFeeType()
        {
            var types = new List<MPSAddFeeType>
            {
                // new MPSAddFeeType() { Id = null, Key = "\u00A0", NameTw = "\u00A0", NameEn = "\u00A0" },
                new MPSAddFeeType() { Id = 1, Key = "1", NameTw = "付款預補", NameEn = "Pay PreAddition" },
                new MPSAddFeeType() { Id = 2, Key = "2", NameTw = "免費預補", NameEn = "Fee PreAddition" },
                new MPSAddFeeType() { Id = 3, Key = "3", NameTw = "付款開補", NameEn = "Pay Addition" },
            };
            return types.AsQueryable();
        }
        public IQueryable<DivisionType> GetDivisionType()
        {
            var types = new List<DivisionType>
            {
                new DivisionType() { Id = 1, Key = "1", NameTw = "面部", NameEn = "Upper" },
                new DivisionType() { Id = 2, Key = "2", NameTw = "底部", NameEn = "Sole" },
                new DivisionType() { Id = 3, Key = "3", NameTw = "包材", NameEn = "Packaging" },
                new DivisionType() { Id = 4, Key = "4", NameTw = "其他", NameEn = "Others" },
            };
            return types.AsQueryable();
        }
        public IQueryable<RDPOType> GetRDPOType()
        {
            var types = new List<RDPOType>
            {
                new RDPOType() { Id = 1, Key = "1", NameTw = "開發調料", NameEn = "RD PO" },
                new RDPOType() { Id = 2, Key = "2", NameTw = "銷樣調料", NameEn = "Sales Sample PO" },
                new RDPOType() { Id = 3, Key = "3", NameTw = "工程調料", NameEn = "Engineering PO" },
            };
            return types.AsQueryable();
        }
    }
}