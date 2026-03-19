using System;
using System.Collections.Generic;

namespace ERP.Models.Views;

public partial class OrdersInsp
{
    public decimal Id { get; set; }

    public decimal LocaleId { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public string ModifyUserName { get; set; }

    public decimal OrdersId { get; set; }

    public string OrderNo { get; set; }

    public DateTime? InspDate { get; set; }

    /// <summary>
    /// 驗貨類型，第一次驗貨，複驗
    /// </summary>
    public int? InspType { get; set; }

    /// <summary>
    /// 驗貨方，線上驗貨，第三方，客人
    /// </summary>
    public int? InspSourceType { get; set; }

    /// <summary>
    /// 驗貨員
    /// </summary>
    public string Inspector { get; set; }

    /// <summary>
    /// 驗貨數量
    /// </summary>
    public decimal? InspQty { get; set; }

    /// <summary>
    /// 驗貨結果
    /// </summary>
    public int? InspResult { get; set; }

    /// <summary>
    /// 不量數
    /// </summary>
    public decimal? DefectQty { get; set; }

    public int? DefectType { get; set; }

    public string DefectDesc { get; set; }

    public string CompanyNo { get; set; }

    public string Brand { get; set; }

    public string ArticleNo { get; set; }

    public string StyleNo { get; set; }

    public string Customer { get; set; }

    /// <summary>
    /// 訂單數量
    /// </summary>
    public decimal OrderQty { get; set; }

    /// <summary>
    /// 客戶期望交期
    /// </summary>
    public DateTime CSD { get; set; }

    public DateTime? LCSD { get; set; }

    public string ArticleName { get; set; }

    public string STILineCode { get; set; }

    public string ASSLineCode { get; set; }
}
