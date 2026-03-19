using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Models.Views.Common
{
    public class ItemType:Item
    {  
        public string Key { get; set; }
    }
    public class BooleanType : ItemType { }
    public class ProductType : ItemType { }
    public class OrderType : ItemType { }
    public class PayType : ItemType { }
    public class DeliveryTerm : ItemType { }
    public class PackingType : ItemType { }
    public class TransitType : ItemType { }
    public class OrderStatus : ItemType { }
    public class POStatus : ItemType { }
    public class PriceType: ItemType {}
    public class TaxType: ItemType {}
    public class ShipmentType: ItemType {}
    public class POType: ItemType {}
    public class QuotationType: ItemType {}
    public class StockIOType: ItemType {}
    public class DailyType: ItemType {}
    public class MaterialStockType: ItemType {}
    public class InspectResultType: ItemType {}
    public class InspectType: ItemType {}
    public class AccountType: ItemType {}
    public class DeliveryType: ItemType {}
    public class AlternateType: ItemType {}
    public class TCType: ItemType {}
    public class MPSPOType: ItemType {}
    public class MPSFeeType: ItemType {}
    public class MPSAddFeeType: ItemType {}
    public class MPSDeliveryType: ItemType {}
    public class DivisionType: ItemType {}
    public class RDPOType: ItemType {}
}
