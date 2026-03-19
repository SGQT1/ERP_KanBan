using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MRPQueueService : BusinessService
    {
        private Services.Entities.OrdersService Orders { get; }
        private Services.Entities.QueueDoMRPService QueueDoMRP { get; }
        private Services.Entities.QueueDoMRPLogService QueueDoMRPLog { get; }
        private Services.Entities.MRPService MRP { get; }
        public MRPQueueService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.QueueDoMRPService queueDoMRPService,
            Services.Entities.QueueDoMRPLogService queueDoMRPLogService,
            Services.Entities.MRPService mrpService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            QueueDoMRP = queueDoMRPService;
            QueueDoMRPLog = queueDoMRPLogService;
            Orders = ordersService;
            MRP = mrpService;
        }
        public IQueryable<Models.Views.MRPQueue> Get()
        {
            return
            (
                from q in QueueDoMRP.Get()
                join o in Orders.Get() on new { OrdersId = q.OrdersId, LocaleId = q.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId }
                select new Models.Views.MRPQueue
                {
                    Id = q.Id,
                    LocaleId = q.LocaleId,
                    BatchNo = q.BatchNo,
                    OrdersId = q.OrdersId,
                    SubmitTime = q.SubmitTime,
                    Launcher = q.Launcher,
                    ReportEmail = q.ReportEmail,
                    NotBeforeTime = q.NotBeforeTime,
                    NotAfterTime = q.NotAfterTime,
                    Priority = q.Priority,
                    NotRetryTimes = q.NotRetryTimes,
                    HasRetriedTimes = q.HasRetriedTimes,
                    HasNotifyTimes = q.HasNotifyTimes,
                    ProcessTime = q.ProcessTime,
                    FinishTime = q.FinishTime,

                    CompanyId = o.CompanyId,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    ETD = o.ETD,
                    OrderNo = o.OrderNo,
                    ArticleId = o.ArticleId,
                    StyleId = o.StyleId,
                    Version = o.Version,
                    ShoeName = o.ShoeName,
                    ArticleNo = o.ArticleNo,
                    StyleNo = o.StyleNo,
                    OrderQty = o.OrderQty,
                    BrandCodeId = o.BrandCodeId,
                    Season = o.Season,
                    Company = o.CompanyNo,
                }
            );
        }
        public void CreateRange(IEnumerable<Models.Views.MRPQueue> mrpQueues)
        {
            QueueDoMRP.CreateRange(Build(mrpQueues));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.QueueDoMRP, bool>> predicate)
        {
            QueueDoMRP.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.QueueDoMRP> Build(IEnumerable<Models.Views.MRPQueue> items)
        {
            return items.Select(item => new ERP.Models.Entities.QueueDoMRP
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                BatchNo = item.BatchNo,
                OrdersId = item.OrdersId,
                SubmitTime = item.SubmitTime,
                Launcher = item.Launcher,
                ReportEmail = item.ReportEmail,
                NotBeforeTime = item.NotBeforeTime,
                NotAfterTime = item.NotAfterTime,
                Priority = item.Priority,
                NotRetryTimes = item.NotRetryTimes,
                HasRetriedTimes = item.HasRetriedTimes,
                HasNotifyTimes = item.HasNotifyTimes,
                ProcessTime = item.ProcessTime,
                FinishTime = item.FinishTime,
            });
        }

        public IQueryable<Models.Views.MRPQueueLog> GetLog(string predicate)
        {
            try
            {
                //performance issue, Have If-else in sql will be slow (KDM),Can't not fount out resean
                var byOrders = (
                    from q in QueueDoMRPLog.Get()
                    join o in Orders.Get() on new { OrdersId = q.OrdersId, LocaleId = q.LocaleId } equals new { OrdersId = o.Id, LocaleId = o.LocaleId }
                    select new Models.Views.MRPQueueLog
                    {
                        Id = q.Id,
                        LocaleId = q.LocaleId,
                        BatchNo = q.BatchNo,
                        OrdersId = q.OrdersId,
                        SubmitTime = q.SubmitTime,
                        Launcher = q.Launcher,
                        ReportEmail = q.ReportEmail,
                        NotBeforeTime = q.NotBeforeTime,
                        NotAfterTime = q.NotAfterTime,
                        Priority = q.Priority,
                        NotRetryTimes = q.NotRetryTimes,
                        HasRetriedTimes = q.HasRetriedTimes,
                        HasNotifyTimes = q.HasNotifyTimes,
                        ProcessTime = q.ProcessTime,
                        FinishTime = q.FinishTime,
                        ProcessLog = q.ProcessLog,
                        ProcessStatus = q.ProcessStatus,

                        CompanyId = o.CompanyId,
                        CSD = o.CSD,
                        LCSD = o.LCSD,
                        ETD = o.ETD,
                        OrderNo = o.OrderNo,
                        ArticleId = o.ArticleId,
                        StyleId = o.StyleId,
                        Version = o.Version,
                        ShoeName = o.ShoeName,
                        ArticleNo = o.ArticleNo,
                        StyleNo = o.StyleNo,
                        OrderQty = o.OrderQty,
                        BrandCodeId = o.BrandCodeId,
                        Season = o.Season,
                        Customer = o.Customer,
                        OWD = o.OWD,
                        Company = o.CompanyNo
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();

                var byMRP = (
                    from q in QueueDoMRPLog.Get()
                    join l in MRP.Get() on new { OrdersId = q.OrdersId, MRPVersion = q.MRPVersion, LocaleId = q.LocaleId } equals new { OrdersId = l.OrdersId, MRPVersion = (int?)l.MRPVersion, LocaleId = l.LocaleId }
                    // join l in MRP.Get() on new { OrdersId = q.OrdersId, MRPVersion = q.MRPVersion, LocaleId = q.LocaleId } equals new { OrdersId = l.OrdersId, MRPVersion = (int?)l.MRPVersion, LocaleId = l.LocaleId } into lGRP
                    // from l in lGRP.DefaultIfEmpty()
                    select new Models.Views.MRPQueueLog
                    {
                        Id = q.Id,
                        LocaleId = q.LocaleId,
                        BatchNo = q.BatchNo,
                        OrdersId = q.OrdersId,
                        SubmitTime = q.SubmitTime,
                        Launcher = q.Launcher,
                        ReportEmail = q.ReportEmail,
                        NotBeforeTime = q.NotBeforeTime,
                        NotAfterTime = q.NotAfterTime,
                        Priority = q.Priority,
                        NotRetryTimes = q.NotRetryTimes,
                        HasRetriedTimes = q.HasRetriedTimes,
                        HasNotifyTimes = q.HasNotifyTimes,
                        ProcessTime = q.ProcessTime,
                        FinishTime = q.FinishTime,
                        ProcessLog = q.ProcessLog,
                        ProcessStatus = q.ProcessStatus,

                        CompanyId = l.CompanyId,
                        CSD = l.CSD,
                        LCSD = l.LCSD,
                        ETD = l.ETD,
                        OrderNo = l.OrderNo,
                        ArticleId = l.ArticleId,
                        StyleId = l.StyleId,
                        Version = l.Version,
                        ShoeName = l.ShoeName,
                        ArticleNo = l.ArticleNo,
                        StyleNo = l.StyleNo,
                        OrderQty = l.OrderQty,
                        BrandCodeId = l.BrandCodeId,
                        Season = l.Season,
                        Customer = l.Customer,
                        OWD = l.OWD,
                        Company = l.CompanyNo,
                    }
                )
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .ToList();

                var result = (
                    from o in byOrders
                    join m in byMRP on new { MRPQueueLog = o.Id, OrdersId = o.OrdersId, LocaleId = o.LocaleId } equals new { MRPQueueLog = m.Id, OrdersId = m.OrdersId, LocaleId = m.LocaleId } into mGRP
                    from m in mGRP.DefaultIfEmpty()
                    select new Models.Views.MRPQueueLog
                    {
                        Id = o.Id,
                        LocaleId = o.LocaleId,
                        BatchNo = o.BatchNo,
                        OrdersId = o.OrdersId,
                        SubmitTime = o.SubmitTime,
                        Launcher = o.Launcher,
                        ReportEmail = o.ReportEmail,
                        NotBeforeTime = o.NotBeforeTime,
                        NotAfterTime = o.NotAfterTime,
                        Priority = o.Priority,
                        NotRetryTimes = o.NotRetryTimes,
                        HasRetriedTimes = o.HasRetriedTimes,
                        HasNotifyTimes = o.HasNotifyTimes,
                        ProcessTime = o.ProcessTime,
                        FinishTime = o.FinishTime,
                        ProcessLog = o.ProcessLog,
                        ProcessStatus = o.ProcessStatus,

                        CompanyId = m == null ? o.CompanyId : m.CompanyId,
                        CSD = m == null ? o.CSD : m.CSD,
                        LCSD = m == null ? o.LCSD : m.LCSD,
                        ETD = m == null ? o.ETD : m.ETD,
                        OrderNo = m == null ? o.OrderNo : m.OrderNo,
                        ArticleId = m == null ? o.ArticleId : m.ArticleId,
                        StyleId = m == null ? o.StyleId : m.StyleId,
                        Version = m == null ? o.Version : m.Version,
                        ShoeName = m == null ? o.ShoeName : m.ShoeName,
                        ArticleNo = m == null ? o.ArticleNo : m.ArticleNo,
                        StyleNo = m == null ? o.StyleNo : m.StyleNo,
                        OrderQty = m == null ? o.OrderQty : m.OrderQty,
                        BrandCodeId = m == null ? o.BrandCodeId : m.BrandCodeId,
                        Season = m == null ? o.Season : m.Season,
                        Customer = m == null ? o.Customer : m.Customer,
                        OWD = m == null ? o.OWD : m.OWD,
                        Company = m == null ? o.Company : m.Company,
                    }
                ).AsQueryable();
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}