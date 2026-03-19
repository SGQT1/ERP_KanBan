using System;
using System.Collections.Generic;

namespace ERP.Models.Entities;

public partial class WIPInvTxnIOSize
{
    public decimal Id { get; set; }

    public decimal LocaleId { get; set; }
    public string ModifyUserName { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public Guid msrepl_tran_version { get; set; }

    public decimal WIPInvTxnIOId { get; set; }

    public string DisplaySize { get; set; }

    public decimal ShoeSize { get; set; }

    public string ShoeSizeSuffix { get; set; }

    public double ShoeInnerSize { get; set; }

    public decimal SizeQty { get; set; }

    public decimal SizeIOQty { get; set; }
}
