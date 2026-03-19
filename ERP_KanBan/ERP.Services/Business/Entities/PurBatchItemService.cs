using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PurBatchItemService : BusinessService
    {
        private ERP.Services.Entities.PurBatchItemService PurBatchItem { get; set; }

        private ERP.Services.Entities.PurBatchItemService _PurBatchItem { get; set; }

        public PurBatchItemService(
            ERP.Services.Entities.PurBatchItemService purBatchItemService,
            ERP.Services.Entities.PurBatchItemService _purBatchItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PurBatchItem = purBatchItemService;
            _PurBatchItem = _purBatchItemService;
        }
        public IQueryable<Models.Views.PurBatchItem> Get()
        {
            return PurBatchItem.Get().Select(i => new Models.Views.PurBatchItem
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                BatchId = i.BatchId,
                OrdersId = i.OrdersId,
                MaterialId = i.MaterialId,
                PlanUnitCodeId = i.PlanUnitCodeId,
                PlanQty = i.PlanQty,
                RefQuotId = i.RefQuotId,
                VendorId = i.VendorId,
                PurUnitPrice = i.PurUnitPrice,
                DollarCodeId = i.DollarCodeId,
                PayCodeId = i.PayCodeId,
                PurUnitCodeId = i.PurUnitCodeId,
                PurQty = i.PurQty,
                PurLocaleId = i.PurLocaleId,
                ReceivingLocaleId = i.ReceivingLocaleId,
                PaymentLocaleId = i.PaymentLocaleId,
                POItemId = i.POItemId,
                OnHandQty = i.OnHandQty,
                ParentMaterialId = i.ParentMaterialId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                PayDollarCodeId = i.PayDollarCodeId,
                RefLocaleId = i.RefLocaleId,
                RefItemId = i.RefItemId,
                AlternateType = i.AlternateType,
                Status = i.Status,
                MRPVersion = i.MRPVersion,
            });

        }
        public void CreateRange(IEnumerable<Models.Views.PurBatchItem> items)
        {
            PurBatchItem.CreateRange(BuildRange(items));
        }

        public void RemoveRange(Expression<Func<ERP.Models.Entities.PurBatchItem, bool>> predicate)
        {
            PurBatchItem.RemoveRange(predicate);
        }
        public void UpdateRange(IEnumerable<Models.Views.PurBatchItem> items)
        {
            PurBatchItem.UpdateRange(BuildRange(items));
        }

        public Models.Views.PurBatchItem Create(Models.Views.PurBatchItem item)
        {
            var _item = PurBatchItem.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.PurBatchItem Update(Models.Views.PurBatchItem item)
        {
            PurBatchItem.Update(Build(item));
            return Get().Where(i => i.Id == item.Id && i.LocaleId == item.LocaleId).FirstOrDefault();
        }
        public Models.Views.PurBatchItem UpdatePart(Models.Views.PurBatchItem item)
        {
            // PurBatchItem.Update(Build(item));
            _PurBatchItem.UpdateRange(
                i => i.Id == item.Id && i.LocaleId == item.LocaleId,
                // u => new Models.Entities.PurBatchItem
                // {
                //     PurQty = item.PurQty,
                //     Status = item.Status,
                //     PlanUnitCodeId = item.PurUnitCodeId,
                //     PayDollarCodeId = item.PayDollarCodeId,
                //     ModifyUserName = item.ModifyUserName,
                // }
                u => u.SetProperty(p => p.PurQty, v => item.PurQty).SetProperty(p => p.Status, v => item.Status).SetProperty(p => p.PlanUnitCodeId, v => item.PurUnitCodeId)
                      .SetProperty(p => p.PayDollarCodeId, v => item.PayDollarCodeId).SetProperty(p => p.ModifyUserName, v => item.ModifyUserName)
            );
            return Get().Where(i => i.Id == item.Id && i.LocaleId == item.LocaleId).FirstOrDefault();
        }
        public IEnumerable<Models.Entities.PurBatchItem> BuildRange(IEnumerable<Models.Views.PurBatchItem> items)
        {
            return items.Select(item => new Models.Entities.PurBatchItem
            {
                Id = (decimal)item.Id,
                LocaleId = item.LocaleId,
                BatchId = item.BatchId,
                OrdersId = item.OrdersId,
                MaterialId = item.MaterialId,
                PlanUnitCodeId = item.PlanUnitCodeId,
                PlanQty = item.PlanQty,
                RefQuotId = item.RefQuotId,
                VendorId = (decimal)item.VendorId,
                PurUnitPrice = item.PurUnitPrice,
                DollarCodeId = (decimal)item.DollarCodeId,
                PayCodeId = (decimal)item.PayCodeId,
                PurUnitCodeId = (decimal)item.PurUnitCodeId,
                PurQty = item.PurQty,
                PurLocaleId = item.LocaleId,//(decimal)item.PurLocaleId,
                ReceivingLocaleId = (decimal)item.ReceivingLocaleId,
                PaymentLocaleId = (decimal)item.PaymentLocaleId,
                POItemId = item.POItemId,
                OnHandQty = item.OnHandQty,
                ParentMaterialId = item.ParentMaterialId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PayDollarCodeId = item.PayDollarCodeId,
                RefLocaleId = item.RefLocaleId,
                RefItemId = item.RefItemId,
                AlternateType = item.AlternateType,
                Status = item.Status,
                MRPVersion = item.MRPVersion,
            });
        }
        public Models.Entities.PurBatchItem Build(Models.Views.PurBatchItem item)
        {
            return new Models.Entities.PurBatchItem
            {
                Id = (decimal)item.Id,
                LocaleId = item.LocaleId,
                BatchId = item.BatchId,
                OrdersId = item.OrdersId,
                MaterialId = item.MaterialId,
                PlanUnitCodeId = item.PlanUnitCodeId,
                PlanQty = item.PlanQty,
                RefQuotId = item.RefQuotId,
                VendorId = (decimal)item.VendorId,
                PurUnitPrice = item.PurUnitPrice,
                DollarCodeId = (decimal)item.DollarCodeId,
                PayCodeId = (decimal)item.PayCodeId,
                PurUnitCodeId = (decimal)item.PurUnitCodeId,
                PurQty = item.PurQty,
                PurLocaleId = item.LocaleId,//(decimal)item.PurLocaleId,
                ReceivingLocaleId = (decimal)item.ReceivingLocaleId,
                PaymentLocaleId = (decimal)item.PaymentLocaleId,
                POItemId = item.POItemId,
                OnHandQty = item.OnHandQty,
                ParentMaterialId = item.ParentMaterialId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PayDollarCodeId = item.PayDollarCodeId,
                RefLocaleId = item.RefLocaleId,
                RefItemId = item.RefItemId,
                AlternateType = item.AlternateType,
                Status = item.Status,
                MRPVersion = item.MRPVersion,
            };
        }

    }
}