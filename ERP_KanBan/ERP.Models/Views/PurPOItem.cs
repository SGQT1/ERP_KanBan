using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class PurPOItem
    {
        public decimal OrdersId { get; set; }            // PurBatchItem
        public decimal MaterialId { get; set; }          // PurBatchItem
        public decimal? ParentMaterialId { get; set; }   // PurBatchItem
        public int? MRPVersion { get; set; }             // PurBatchItem
        public decimal OnHandQty { get; set; }           // PurBatchItem
        public decimal PlanUnitCodeId { get; set; }      // PurBatchItem
        public int? PurAlternateType { get; set; }       // PurBatchItem
        public decimal PurPurLocaleId { get; set; }      // PurBatchItem
        public decimal PurReceivingLocaleId { get; set; }// PurBatchItem
        public decimal PurPaymentLocaleId { get; set; }  // PurBatchItem
        public decimal BatchId { get; set; }             // PurBatchItem
        public decimal PurVendorId { get; set; }         //PurBatchItem
        public decimal PurUnitPrice { get; set; }        // PurBatchItem
        public decimal PurUnitCodeId { get; set; }       // PurBatchItem
        public decimal PurQty { get; set; }              // PurBatchItem
        public decimal PurPayCodeId { get; set; }        // PurBatchItem
        public decimal PurPlanQty { get; set; }          // PurBatchItem
        public decimal Id { get; set; }                  // PurBatchItem
        public decimal LocaleId { get; set; }            // PurBatchItem
        public decimal RefQuotId { get; set; }           // PurBatchItem
        public string ModifyUserName { get; set; }       // PurBatchItem
        public DateTime LastUpdateTime { get; set; }     // PurBatchItem
        public decimal PayDollarCodeId { get; set; }     // PurBatchItem
        public decimal RefLocaleId { get; set; }         // PurBatchItem
        public decimal RefItemId { get; set; }           // PurBatchItem
        

        public int? PurSamplingMethod { get; set; }      // material
        public decimal? CategoryCodeId { get; set; }      // material
        public string? MaterialNameTw { get; set; }       // material
        public string? MaterialNameEn { get; set; }       // material

        public string? StyleNo { get; set; }             // Orders
        public string? PurRemark { get; set; }           // Orders
        public string? OrderNo { get; set; }             // Orders
        public DateTime? LCSD { get; set; }             // Orders
        public DateTime? CSD { get; set; }              // Orders

        public string? Vendor { get; set; }              // Vendor
        public decimal? PurPaymentCodeId { get; set; }  // Vendor
        public int? PurPaymentPoint { get; set; }       // Vendor
        public string? PurVendor { get; set; }           // Vendor

        public int? IsShowSizeRun { get; set; } // POItem
        public int? AlternateType { get; set; } // POItem
        public string? PONo { get; set; } // POItem
        public string? ReceivedBarcode { get; set; } // POItem
        public decimal? PurLocaleId { get; set; } // POItem
        public decimal? ReceivingLocaleId { get; set; } // POItem
        public decimal? PaymentLocaleId { get; set; } // POItem
        public decimal? PayCodeId { get; set; } // POItem
        public decimal? PlanQty { get; set; }  // POItem  
        public decimal? POQty { get; set; } // POQty 
        public decimal? UnitCodeId { get; set; } // POItem
        public decimal? DollarCodeId { get; set; }// POItem
        public decimal? UnitPrice { get; set; } // POItem
        public decimal? VendorId { get; set; }// POItem    
        public decimal? POId { get; set; } // POItem
        public decimal? POItemId { get; set; } // POItem
        public int? Status { get; set; } // POItem
        public int? POType { get; set; }    // POItem
        public int? SamplingMethod { get; set; } // POItem
        public DateTime? FactoryETD { get; set; }// POItem
        public string? Remark { get; set; }// POItem
        public decimal? CompanyId { get; set; }// POItem
        public string? Seq { get; set; } // POItem
        public decimal? PaymentCodeId { get; set; } // POItem
        public int? PaymentPoint { get; set; } // POItem
        public decimal? ReceivedLogId { get; set; } // POItem
        public decimal? QuotUnitPrice { get; set; } // POItem
        public DateTime? PODate { get; set; }// POItem
        public int? IsOverQty { get; set; }// POItem
        
        public string? POTeam { get; set; }
        public int? Diff { get; set; } // diff
        public decimal? NewPurQty { get; set; }          // PurBatchItem
    }
}
