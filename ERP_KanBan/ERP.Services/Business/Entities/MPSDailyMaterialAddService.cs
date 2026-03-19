using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Words;

namespace ERP.Services.Business.Entities
{

    public class MPSDailyMaterialAddService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyMaterialAddService MPSDailyMaterialAdd { get; set; }
        private ERP.Services.Entities.MpsStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Entities.MpsDailyLangAddService MPSDailyMaterialLangAdd { get; set; }

        public MPSDailyMaterialAddService(
            ERP.Services.Entities.MpsDailyMaterialAddService mpsDailyMaterialAddService,
            ERP.Services.Entities.MpsStyleItemService mpsStyleItemService,
            ERP.Services.Entities.MpsDailyLangAddService mpsDailyLangAddService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyMaterialAdd = mpsDailyMaterialAddService;
            MPSStyleItem = mpsStyleItemService;
            MPSDailyMaterialLangAdd = mpsDailyLangAddService;
        }

        public IQueryable<Models.Views.MPSDailyMaterialAdd> Get()
        {
            // 暫時取用 MPSStyleItem
            var items = (
                from m in MPSDailyMaterialAdd.Get()
                join s in MPSStyleItem.Get() on new { MPSStyleItemId = m.MpsStyleItemId, LocaleId = m.LocaleId } equals new { MPSStyleItemId = s.Id, LocaleId = s.LocaleId } into sGRP
                from s in sGRP.DefaultIfEmpty()
                select new Models.Views.MPSDailyMaterialAdd
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MpsDailyAddId = m.MpsDailyAddId,
                    PartNo = m.PartNo,
                    PartNameTw = m.PartNameTw,
                    MaterialNameTw = m.MaterialNameTw,
                    UnitNameTw = m.UnitNameTw,
                    
                    PartNameEn = s.PartNameEn,
                    MaterialNameEn = s.MaterialNameEn,
                    UnitNameEn = s.UnitNameEn,

                    AlternateType = m.AlternateType,
                    PreTotalUsage = m.PreTotalUsage,
                    SubMulti = m.SubMulti,
                    TotalUsage = m.TotalUsage,
                    ProceduresId = m.ProceduresId,
                    MpsStyleItemId = m.MpsStyleItemId,
                    WarehouseNo = m.WarehouseNo,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                }
            );
            return items;
        }
        public Models.Views.MPSDailyMaterialAdd Create(Models.Views.MPSDailyMaterialAdd item)
        {
            var _item = MPSDailyMaterialAdd.CreateKeepId(Build(item));

            var partNameCn = WordsHelper.ToSimplifiedChinese(item.PartNameTw);
            var materialNameCn = WordsHelper.ToSimplifiedChinese(item.MaterialNameTw);
            var unitNameCn = WordsHelper.ToSimplifiedChinese(item.UnitNameTw);

            MPSDailyMaterialLangAdd.Create(new Models.Entities.MpsDailyLangAdd {
                PartNameCn = partNameCn,
                MaterialNameCn = materialNameCn,
                UnitNameCn = unitNameCn,

                PartNameVn = item.PartNameEn,
                MaterialNameVn = item.MaterialNameEn,
                UnitNameVn = item.UnitNameEn,

                PartNameOther = item.PartNameEn,
                MaterialNameOther = item.MaterialNameEn,
                UnitNameOther = item.UnitNameEn,
                
                MpsDailyAddId = item.MpsDailyAddId,
                MpsStyleItemId = item.MpsStyleItemId,
                LocaleId = item.LocaleId,
            });
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSDailyMaterialAdd Update(Models.Views.MPSDailyMaterialAdd item)
        {
            var _item = MPSDailyMaterialAdd.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSDailyMaterialAdd item)
        {
            MPSDailyMaterialAdd.Remove(Build(item));
            MPSDailyMaterialLangAdd.RemoveRange(i => i.LocaleId == item.LocaleId && i.MpsDailyAddId == item.Id);
        }
        public void Remove(int mpsDailyAddId, int localeId)
        {
            MPSDailyMaterialLangAdd.RemoveRange(i => i.LocaleId == localeId && i.MpsDailyAddId == mpsDailyAddId);
            MPSDailyMaterialAdd.RemoveRange(i => i.LocaleId == localeId && i.MpsDailyAddId == mpsDailyAddId);
        }
        private Models.Entities.MpsDailyMaterialAdd Build(Models.Views.MPSDailyMaterialAdd item)
        {
            return new Models.Entities.MpsDailyMaterialAdd()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyAddId = item.MpsDailyAddId,
                PartNo = item.PartNo,
                PartNameTw = item.PartNameTw,
                MaterialNameTw = item.MaterialNameTw,
                UnitNameTw = item.UnitNameTw,
                AlternateType = item.AlternateType,
                PreTotalUsage = item.PreTotalUsage,
                SubMulti = item.SubMulti,
                TotalUsage = item.TotalUsage,
                ProceduresId = item.ProceduresId,
                MpsStyleItemId = item.MpsStyleItemId,
                WarehouseNo = item.WarehouseNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.MPSDailyMaterialAdd> items)
        {
            MPSDailyMaterialAdd.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDailyMaterialAdd, bool>> predicate)
        {
            MPSDailyMaterialAdd.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyMaterialAdd> BuildRange(IEnumerable<Models.Views.MPSDailyMaterialAdd> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyMaterialAdd
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyAddId = item.MpsDailyAddId,
                PartNo = item.PartNo,
                PartNameTw = item.PartNameTw,
                MaterialNameTw = item.MaterialNameTw,
                UnitNameTw = item.UnitNameTw,
                AlternateType = item.AlternateType,
                PreTotalUsage = item.PreTotalUsage,
                SubMulti = item.SubMulti,
                TotalUsage = item.TotalUsage,
                ProceduresId = item.ProceduresId,
                MpsStyleItemId = item.MpsStyleItemId,
                WarehouseNo = item.WarehouseNo,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }

    }
}
