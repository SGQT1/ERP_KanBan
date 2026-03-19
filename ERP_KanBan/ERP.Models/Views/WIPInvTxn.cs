using System;
using System.Collections.Generic;

namespace ERP.Models.Views;

public partial class WIPInvTxn
{
    public decimal Id { get; set; }

    public decimal LocaleId { get; set; }

    public string OrderNo { get; set; }

    public string WarehourseNo { get; set; }

    public decimal TotalQty { get; set; }

    public string ModifyUserName { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public int WIPType { get; set; }
}
