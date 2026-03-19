using System;
using System.Collections.Generic;

namespace ERP.Models.Views;

public partial class WIPInvTxnIO
{
    public decimal Id { get; set; }

    public decimal LocaleId { get; set; }

    public Guid msrepl_tran_version { get; set; }

    public decimal WIPInvTxnId { get; set; }

    public int SourceType { get; set; }

    public string OrderNo { get; set; }

    public decimal OrderQty { get; set; }

    public DateTime IODate { get; set; }

    public decimal IOQty { get; set; }

    public decimal? MPSProcessId { get; set; }

    public string MPSProcessName { get; set; }

    public string SourUnit { get; set; }

    public string Remark { get; set; }
    
    public string ModifyUserName { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public string ArticleSizeRun { get; set; }
    public string ArticleHead { get; set; }
    public string ArticleSizeRunId { get; set; }
}
