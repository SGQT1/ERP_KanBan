using System;
using System.Collections.Generic;

namespace ERP.Models.Entities;

public partial class OrdersInspPlan
{
    public decimal Id { get; set; }
    public decimal LocaleId { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public string ModifyUserName { get; set; }
    public decimal OrdersId { get; set; }
    public string OrderNo { get; set; }
    public string STILineCode { get; set; }
    public string ASSLineCode { get; set; }
    public DateTime? InspPlanDate { get; set; }
    public DateTime? WeeklyInspPlanDate { get; set; }
    public string CustomerOrderNo { get; set; }
    public DateTime CSD { get; set; }
    public DateTime? LCSD { get; set; }
}
