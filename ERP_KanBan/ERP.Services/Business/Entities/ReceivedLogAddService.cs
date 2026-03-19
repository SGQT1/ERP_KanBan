using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class ReceivedLogAddService : BusinessService
    {
        private Services.Entities.ReceivedLogAddService ReceivedLogAdd { get; }

        public ReceivedLogAddService(Services.Entities.ReceivedLogAddService receivedLogAddService, UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.ReceivedLogAdd = receivedLogAddService;
        }
        public IQueryable<Models.Views.ReceivedLogAdd> Get()
        {
            return ReceivedLogAdd.Get().Select(i => new Models.Views.ReceivedLogAdd
            {
                ReceivedLogId = i.ReceivedLogId,
                LocaleId = i.LocaleId,
                RefPONo = i.RefPONo,
                Type = i.Type,
                MaterialId = i.MaterialId,
                MaterialNameTw = i.MaterialNameTw,
                ParentMaterialId = i.ParentMaterialId,
                ParentMaterialNameTw = i.ParentMaterialNameTw,
                PCLUnitCodeId = i.PCLUnitCodeId,
                PCLUnitNameTw = i.PCLUnitNameTw,
                PurUnitCodeId = i.PurUnitCodeId,
                PurUnitNameTw = i.PurUnitNameTw,
                PayQty = i.PayQty,
                FreeQty = i.FreeQty,
                PurDollarCodeId = i.PurDollarCodeId,
                PurDollarNameTw = i.PurDollarNameTw,
                StockDollarCodeId = i.StockDollarCodeId,
                StockDollarNameTw = i.StockDollarNameTw,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                CloseMonth = i.CloseMonth,
            });
        }
    }
}