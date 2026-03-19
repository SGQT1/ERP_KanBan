using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Words;

namespace ERP.Services.Business.Entities
{

    public class MPSDailyMaterialService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyMaterialService MPSDailyMaterial { get; set; }
        private ERP.Services.Entities.MpsStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Entities.MpsDailyLangService MPSDailyMaterialLang { get; set; }
        private ERP.Services.Entities.PartService Part { get; set; }


        public MPSDailyMaterialService(
            ERP.Services.Entities.MpsDailyMaterialService mpsDailyMaterialService,
            ERP.Services.Entities.MpsStyleItemService mpsStyleItemService,
            ERP.Services.Entities.MpsDailyLangService mpsDailyLangService,
            ERP.Services.Entities.PartService partService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyMaterial = mpsDailyMaterialService;
            MPSStyleItem = mpsStyleItemService;
            MPSDailyMaterialLang = mpsDailyLangService;
            Part = partService;
        }

        public IQueryable<Models.Views.MPSDailyMaterial> Get()
        {
            // 暫時取用 MPSStyleItem
            var items = (
                from m in MPSDailyMaterial.Get()
                join s in MPSStyleItem.Get() on new { MPSStyleItemId = m.MpsStyleItemId, LocaleId = m.LocaleId } equals new { MPSStyleItemId = s.Id, LocaleId = s.LocaleId } into sGRP
                from s in sGRP.DefaultIfEmpty()
                join p in Part.Get() on new { Part = s.PartNameTw, LocaleId = s.LocaleId } equals new { Part = p.PartNameTw, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new Models.Views.MPSDailyMaterial
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MPSDailyId = m.MpsDailyId,
                    TotalUsage = m.TotalUsage,
                    PreTotalUsage = m.PreTotalUsage,
                    MpsStyleItemId = m.MpsStyleItemId,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                    PartNameTw = m.PartNameTw != null ? m.PartNameTw : s.PartNameTw,
                    PartNameEn = m.PartNameEn != null ? m.PartNameEn : s.PartNameEn,
                    MaterialNameTw = m.MaterialNameTw != null ? m.MaterialNameTw : s.MaterialNameTw,
                    MaterialNameEn = m.MaterialNameEn != null ? m.MaterialNameEn : s.MaterialNameEn,
                    UnitNameTw = m.UnitNameTw != null ? m.UnitNameTw : s.UnitNameTw,
                    UnitNameEn = m.UnitNameEn != null ? m.UnitNameEn : s.UnitNameEn,
                    PieceOfPair = m.PieceOfPair != null ? m.PieceOfPair : s.PieceOfPair,
                    AlternateType = m.AlternateType != null ? m.AlternateType : s.AlternateType,
                    RefKnifeNo = m.RefKnifeNo != null ? m.RefKnifeNo : s.RefKnifeNo,
                    PartNo = m.PartNo != null ? m.PartNo : p.PartNameTw,

                    // PartNameTw = s != null ? s.PartNameTw : m.PartNameTw,
                    // PartNameEn = s != null ? s.PartNameEn : m.PartNameEn,
                    // MaterialNameTw = s != null ? s.MaterialNameTw : m.MaterialNameTw,
                    // MaterialNameEn = s != null ? s.MaterialNameEn : m.MaterialNameEn,
                    // UnitNameTw = s != null ? s.UnitNameTw : m.UnitNameTw,
                    // UnitNameEn = s != null ? s.UnitNameEn : m.UnitNameEn,
                    // PieceOfPair = s != null ? s.PieceOfPair : m.PieceOfPair,
                    // AlternateType = s != null ? s.AlternateType : m.AlternateType,
                    // RefKnifeNo = s != null ? s.RefKnifeNo : m.RefKnifeNo,
                    // PartNo = p != null ? p.PartNo : m.PartNo,
                    // MRPItemId = m.MRPItemId,
                }
            );
            return items;
        }

        public IQueryable<Models.Views.MPSDailyMaterial> Get1()
        {
            // 暫時取用 MPSStyleItem
            var items = (
                from m in MPSDailyMaterial.Get()
                join s in MPSStyleItem.Get() on new { MPSStyleItemId = m.MpsStyleItemId, LocaleId = m.LocaleId } equals new { MPSStyleItemId = s.Id, LocaleId = s.LocaleId } into sGRP
                from s in sGRP.DefaultIfEmpty()
                join p in Part.Get() on new { Part = s.PartNameTw, LocaleId = s.LocaleId } equals new { Part = p.PartNameTw, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new Models.Views.MPSDailyMaterial
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    MPSDailyId = m.MpsDailyId,
                    TotalUsage = m.TotalUsage,
                    PreTotalUsage = m.PreTotalUsage,
                    MpsStyleItemId = m.MpsStyleItemId,
                    ModifyUserName = m.ModifyUserName,
                    LastUpdateTime = m.LastUpdateTime,
                    PartNameTw = s != null ? s.PartNameTw : "",
                    PartNameEn = s != null ? s.PartNameEn : "",
                    MaterialNameTw = s != null ? s.MaterialNameTw : "",
                    MaterialNameEn = s != null ? s.MaterialNameEn : "",
                    UnitNameTw = s != null ? s.UnitNameTw : "",
                    UnitNameEn = s != null ? s.UnitNameEn : "",

                    PieceOfPair = s != null ? s.PieceOfPair : 0,
                    AlternateType = s != null ? s.AlternateType : 0,
                    RefKnifeNo = s != null ? s.RefKnifeNo : "",
                    PartNo = p != null ? p.PartNo : "",
                    // MRPItemId = m.MRPItemId,
                }
            );
            return items;
        }
        public Models.Views.MPSDailyMaterial Create(Models.Views.MPSDailyMaterial item)
        {
            var _item = MPSDailyMaterial.CreateKeepId(Build(item));

            var partNameCn = WordsHelper.ToSimplifiedChinese(item.UnitNameTw);
            var materialNameCn = WordsHelper.ToSimplifiedChinese(item.MaterialNameTw);
            var unitNameCn = WordsHelper.ToSimplifiedChinese(item.UnitNameTw);

            MPSDailyMaterialLang.Create(new Models.Entities.MpsDailyLang
            {
                PartNameCn = item.PartNameTw,
                PartNameVn = item.PartNameEn,
                MaterialNameCn = materialNameCn,
                MaterialNameVn = item.MaterialNameEn,
                UnitNameCn = unitNameCn,
                UnitNameVn = item.UnitNameEn,

                MpsDailyId = item.MPSDailyId,
                MpsStyleItemId = item.MpsStyleItemId,
                LocaleId = item.LocaleId,
            });
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSDailyMaterial Update(Models.Views.MPSDailyMaterial item)
        {
            var _item = MPSDailyMaterial.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.MPSDailyMaterial item)
        {
            MPSDailyMaterial.Remove(Build(item));
        }
        public void Remove(int mpsDailyId, int localeId)
        {
            MPSDailyMaterialLang.RemoveRange(i => i.LocaleId == localeId && i.MpsDailyId == mpsDailyId);
            MPSDailyMaterial.RemoveRange(i => i.LocaleId == localeId && i.MpsDailyId == mpsDailyId);
        }
        private Models.Entities.MpsDailyMaterial Build(Models.Views.MPSDailyMaterial item)
        {
            return new Models.Entities.MpsDailyMaterial()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyId = item.MPSDailyId,
                TotalUsage = item.TotalUsage,
                PreTotalUsage = item.PreTotalUsage,
                MpsStyleItemId = item.MpsStyleItemId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PartNameTw = item.PartNameTw,
                PartNameEn = item.PartNameEn,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                UnitNameTw = item.UnitNameEn,
                UnitNameEn = item.UnitNameEn,
                PieceOfPair = item.PieceOfPair,
                AlternateType = item.AlternateType,
                RefKnifeNo = item.RefKnifeNo,
                PartNo = item.PartNo,
            };
        }

        public void CreateRange(IEnumerable<Models.Views.MPSDailyMaterial> items)
        {
            MPSDailyMaterial.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsDailyMaterial, bool>> predicate)
        {
            MPSDailyMaterial.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyMaterial> BuildRange(IEnumerable<Models.Views.MPSDailyMaterial> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyMaterial
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsDailyId = item.MPSDailyId,
                TotalUsage = item.TotalUsage,
                PreTotalUsage = item.PreTotalUsage,
                MpsStyleItemId = item.MpsStyleItemId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PartNameTw = item.PartNameTw,
                PartNameEn = item.PartNameEn,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                UnitNameTw = item.UnitNameEn,
                UnitNameEn = item.UnitNameEn,
                PieceOfPair = item.PieceOfPair,
                AlternateType = item.AlternateType,
                RefKnifeNo = item.RefKnifeNo,
                PartNo = item.PartNo,
            });
        }

    }
}
