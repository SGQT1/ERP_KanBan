using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class TransferService : BusinessService
    {
        private Services.Entities.TransferService Transfer { get; }

        public TransferService(
            Services.Entities.TransferService transferService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Transfer = transferService;
        }
        public IQueryable<Models.Views.Transfer> Get()
        {
            var result = (
                from t in Transfer.Get()
                select new Models.Views.Transfer
                {
                    Id = t.Id,
                    LocaleId = t.LocaleId,
                    ContainerNo = t.ContainerNo,
                    ShipmentNo = t.ShipmentNo,
                    ShippingDate = t.ShippingDate,
                    Vessel = t.Vessel,
                    OBDate = t.OBDate,
                    ArrivalDate = t.ArrivalDate,
                    ShippingPortId = t.ShippingPortId,
                    ShippingPortName = t.ShippingPortName,
                    TargetPortId = t.TargetPortId,
                    TargetPortName = t.TargetPortName,
                    ModifyUserName = t.ModifyUserName,
                    LastUpdateTime = t.LastUpdateTime,
                    PaymentLocaleId = t.PaymentLocaleId,
                    UnitPricePercent = t.UnitPricePercent,
                    Remark = t.Remark,
                });
            return result;
        }
        public Models.Views.Transfer Create(Models.Views.Transfer item)
        {
            var _item = Transfer.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Transfer Update(Models.Views.Transfer item)
        {
            var _item = Transfer.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Transfer item)
        {
            Transfer.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Transfer Build(Models.Views.Transfer item)
        {
            return new Models.Entities.Transfer()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ContainerNo = item.ContainerNo,
                ShipmentNo = item.ShipmentNo,
                ShippingDate = item.ShippingDate,
                Vessel = item.Vessel,
                OBDate = item.OBDate,
                ArrivalDate = item.ArrivalDate,
                ShippingPortId = item.ShippingPortId,
                ShippingPortName = item.ShippingPortName,
                TargetPortId = item.TargetPortId,
                TargetPortName = item.TargetPortName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PaymentLocaleId = item.PaymentLocaleId,
                UnitPricePercent = item.UnitPricePercent,
                Remark = item.Remark,
            };
        }

    }
}