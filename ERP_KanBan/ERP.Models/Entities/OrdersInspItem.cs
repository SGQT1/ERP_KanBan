using System;
using System.Collections.Generic;

namespace ERP.Models.Entities;

public partial class OrdersInspItem
{
    public decimal Id { get; set; }

    public decimal LocaleId { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public string ModifyUserName { get; set; }

    public decimal OrdersInspId { get; set; }

    public string CTNLabelCode { get; set; }

    public string SubLabelCode { get; set; }

    public int? SeqNo { get; set; }
}
