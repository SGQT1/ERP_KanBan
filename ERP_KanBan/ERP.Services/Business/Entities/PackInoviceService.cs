using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackInoviceService : BusinessService
    {
        private Services.Business.Entities.PackPlanService PackPlan { get; }
        private Services.Entities.OrdersPackInvoiceService OrdersPackInvoice { get; }

        public PackInoviceService(
            Services.Business.Entities.PackPlanService packPlanService,
            Services.Entities.OrdersPackInvoiceService ordersPackInvoiceService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackPlan = packPlanService;
            OrdersPackInvoice = ordersPackInvoiceService;
        }
        public IQueryable<Models.Views.PackInvoice> Get()
        {
            var invoices = (
                from pl in PackPlan.Get()
                join pi in OrdersPackInvoice.Get() on new { OrderNo = pl.OrderNo, Edition = pl.Edition, LocaleId = pl.LocaleId } 
                                                    equals new { OrderNo = pi.OrderNo, Edition = pi.Edition, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                select new Models.Views.PackInvoice
                {
                    PackPlanId = pl.Id,
                    PackPlanLocaleId = pl.LocaleId,
                    OrderNo = pl.OrderNo,
                    Edition = pl.Edition,
                    PackingQty = pl.PackingQty,
                    PackingCTNS = pl.PackingCTNS,
                    PackingNW = pl.PackingNW,
                    PackingGW = pl.PackingGW,
                    PackingMEAS = pl.PackingMEAS,
                    PackingCBM = pl.PackingCBM,
                    StyleNo = pl.RefStyleNo,
                    CSD = pl.RefCSD,
                    Customer = pl.RefCustomer,
                    CustomerOrderNo = pl.RefCustomerOrderNo,

                    Id = pi.Id,
                    LocaleId = pi.LocaleId,
                    // InvoiceNo = pi == null ? "" : pi.InvoiceNo,
                    InvoiceNo =  pi.InvoiceNo,
                    PLQty = pi.PLQty,
                    DoPL = pi.DoPL,
                    ModifyUserName = pi.ModifyUserName,
                    LastUpdateTime = pi.LastUpdateTime,
                    InvoiceDate = pi.InvoiceDate,
                    HasInvoice = pi == null ? false : true
                }
            );

            return invoices;
        }
        public void CreateRange(IEnumerable<Models.Views.PackInvoice> packInvoices)
        {
            OrdersPackInvoice.CreateRange(BuildRange(packInvoices));
        }
        public void UpdateRange(IEnumerable<Models.Views.PackInvoice> packInvoices)
        {
            var items = BuildRange(packInvoices).ToList();
            items.ForEach(i => {
                OrdersPackInvoice.Update(i);
            });
            
        }
        public void RemoveRange(List<decimal> Ids, int localeId)
        {
            OrdersPackInvoice.RemoveRange(i => Ids.Contains(i.Id) && i.LocaleId == localeId);
        }

        public IEnumerable<Models.Entities.OrdersPackInvoice> BuildRange(IEnumerable<Models.Views.PackInvoice> packInovices)
        {
            return packInovices.Select(item => new Models.Entities.OrdersPackInvoice
            {
                Id = item.Id == null ? 0 : (decimal)item.Id,
                LocaleId = (decimal)item.LocaleId,
                OrderNo = item.OrderNo,
                Edition = item.Edition,
                InvoiceNo = item.InvoiceNo,
                PLQty = (decimal)item.PLQty,
                DoPL = (int)item.DoPL,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime == null ? DateTime.Now : (DateTime)item.LastUpdateTime,
                InvoiceDate = item.InvoiceDate,
            });
        }

    }
}