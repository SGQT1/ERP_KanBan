using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using Newtonsoft.Json;

namespace ERP.Services.Business
{
    public class OrdersInspPlanService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersInspPlanService OrdersInspPlan { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        public OrdersInspPlanService(
            ERP.Services.Business.Entities.OrdersInspPlanService ordersInspPlanService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OrdersInspPlan = ordersInspPlanService;
            Orders = ordersService;
        }

        public IQueryable<ERP.Models.Views.OrdersInspPlan> Get(string predicate, string[] filters)
        {
            var result = new List<ERP.Models.Views.OrdersInspPlan>();
            try
            {
                var status = 0;
                if (filters != null && filters.Length > 0)
                {
                    var extenFilters = JsonConvert.DeserializeObject<ExtentionItem>(filters[0]);
                    status = (int)extenFilters.Field1;
                }

                var items = (
                    from o in Orders.Get()
                    join oip in OrdersInspPlan.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = oip.OrdersId, LocaleId = oip.LocaleId } into oipGRP
                    from oip in oipGRP.DefaultIfEmpty()
                    select new
                    {
                        Id = (decimal?)oip.Id,
                        RefLocaleId = (decimal?)oip.LocaleId,
                        LastUpdateTime = (DateTime?)oip.LastUpdateTime,
                        ModifyUserName = oip.ModifyUserName,
                        STILineCode = oip.STILineCode,
                        ASSLineCode = oip.ASSLineCode,
                        InspPlanDate = (DateTime?)oip.InspPlanDate,
                        WeeklyInspPlanDate = (DateTime?)oip.WeeklyInspPlanDate,

                        RefOrdersId = (decimal?)oip.Id,
                        RefOrderNo = oip.OrderNo,
                        RefCustomerOrderNo = oip.CustomerOrderNo,
                        RefCSD = (DateTime?)oip.CSD,
                        RefLCSD = (DateTime?)oip.LCSD,

                        OrderQty = o.OrderQty,
                        OrdersId = o.Id,
                        LocaleId = o.LocaleId,
                        OrderNo = o.OrderNo,
                        CustomerOrderNo = o.CustomerOrderNo,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                        StyleNo = o.StyleNo,
                        ArticleName = o.ArticleName,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Distinct()
                .ToList();

                result = items.Select(i => new ERP.Models.Views.OrdersInspPlan
                {
                    Id = i.Id ?? 0,
                    LocaleId = i.RefLocaleId ?? i.LocaleId,
                    LastUpdateTime = i.LastUpdateTime,
                    ModifyUserName = i.ModifyUserName,
                    OrdersId = i.RefOrdersId ?? i.OrdersId,
                    OrderNo = i.RefOrderNo ?? i.OrderNo,
                    STILineCode = i.STILineCode ?? "",
                    ASSLineCode = i.ASSLineCode ?? "",
                    InspPlanDate = i.InspPlanDate,
                    WeeklyInspPlanDate = i.WeeklyInspPlanDate,
                    CustomerOrderNo = i.RefCustomerOrderNo ?? i.CustomerOrderNo,
                    CSD = i.RefCSD ?? i.CSD,
                    LCSD = i.RefLCSD ?? i.LCSD,
                    OrderQty = i.OrderQty,
                    StyleNo = i.StyleNo,
                    ArticleName = i.ArticleName,
                })
                .OrderBy(i => i.OrderNo)
                .ToList();


                if (status == 1)
                {
                    result = result.Where(i => i.Id == 0).ToList();
                }
                else if (status == 2)
                {
                    result = result.Where(i => i.Id > 0).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.AsQueryable();
        }

        public List<ERP.Models.Views.OrdersInspPlan> Save(List<ERP.Models.Views.OrdersInspPlan> items)
        {
            try
            {
                if (items.Any())
                {
                    var orderIds = items.Select(i => i.OrdersId).Distinct().ToList();
                    var localeIds = items.Select(i => i.LocaleId).Distinct().ToList();

                    OrdersInspPlan.RemoveRange(i => localeIds.Contains(i.LocaleId) && orderIds.Contains(i.OrdersId));
                    OrdersInspPlan.CreateRange(items);

                    items = OrdersInspPlan.Get().Where(i => orderIds.Contains(i.OrdersId) && localeIds.Contains(i.LocaleId)).ToList();
                    
                    UnitOfWork.BeginTransaction();
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            return items;
        }

        public List<ERP.Models.Views.OrdersInspPlan> Remove(List<ERP.Models.Views.OrdersInspPlan> items)
        {
            // var ordersStockLocale = group.OrdersStockLocale;
            // var ordersStockLocaleIn = group.OrdersStockLocaleIn;

            // UnitOfWork.BeginTransaction();
            // try
            // {
            //     if (ordersStockLocaleIn.Any())
            //     {
            //         var labelCodes = ordersStockLocaleIn.Select(i => i.CTNLabelCode);
            //         OrdersStockLocaleIn.RemoveRange(i => i.LocaleId == ordersStockLocale.LocaleId && labelCodes.Contains(i.CTNLabelCode));
            //     }

            //     UnitOfWork.Commit();
            // }
            // catch (Exception e)
            // {
            //     UnitOfWork.Rollback();
            //     throw e;
            // }
            // return Get((int)ordersStockLocale.Id, (int)ordersStockLocale.LocaleId);

            return items;
        }

    }
}