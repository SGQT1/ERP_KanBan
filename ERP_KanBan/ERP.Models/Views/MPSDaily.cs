using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public partial class MPSDaily
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string DailyNo { get; set; }
        public string PreDailyNo { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DailyDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string OrderNo { get; set; }
        public string MaterialNameTw { get; set; }
        public string MaterialNameEn { get; set; }
        public decimal MaterialId { get; set; }
        public string UnitNameTw { get; set; }
        public string UnitNameEn { get; set; }
        public decimal UnitCodeId { get; set; }
        public decimal OrgUnitId { get; set; }
        public string OrgUnitNameTw { get; set; }
        public string OrgUnitNameEn { get; set; }
        public string OrgUnitNameCn { get; set; }
        public string OrgUnitNameVn { get; set; }
        public decimal Qty { get; set; }
        public int DailyMode { get; set; }
        public int DailyType { get; set; }
        public int DoDaily { get; set; }
        public DateTime? DoDate { get; set; }
        public decimal MpsLiveItemId { get; set; }
        public decimal Multi { get; set; }
        public string Remark { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int IsForOrders { get; set; }
        public decimal? MPSDailyId { get; set; }
        public string Part { get; set; }
        public decimal? TotalUsage { get; set; }
        public string ArticleNo { get; set;}
        public string StyleNo { get; set; }
        public string ShoeName { get; set; }
        public DateTime CSD { get; set; }
        public decimal? CompanyId { get; set; }
        public string CompanyNo { get; set; }

        public decimal? MPSProcessId { get; set; }
        public string MPSProcessNameTw { get; set; }
        public string MPSProcessNameEn { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public bool HasStockOut { get; set;}

    }
}
