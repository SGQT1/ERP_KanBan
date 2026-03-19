using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business.Entities
{

    public class MPSStyleItemService : BusinessService
    {
        private ERP.Services.Entities.MpsStyleService MPSStyle { get; set; }
        private ERP.Services.Entities.MpsStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Entities.MpsProcedureService MPSProcedure { get; set; }
        private ERP.Services.Entities.ProceduresService Procedures { get; set; }
        private ERP.Services.Entities.PartService Part { get; set; }

        public MPSStyleItemService(
            ERP.Services.Entities.MpsStyleService mpsStyleService,
            ERP.Services.Entities.MpsStyleItemService mpsStyleItemService,
            ERP.Services.Entities.MpsProcedureService mpsProcedureService,
            ERP.Services.Entities.ProceduresService proceduresService,
            ERP.Services.Entities.PartService partService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSStyle = mpsStyleService;
            MPSStyleItem = mpsStyleItemService;
            MPSProcedure = mpsProcedureService;
            Procedures = proceduresService;
            Part = partService;
        }

        public IQueryable<Models.Views.MPSStyleItem> GetEnpty()
        {
            return this.MPSStyleItem.Get().Select(i => new Models.Views.MPSStyleItem
            {
                Id = i.Id,
                MpsStyleId = i.MpsStyleId,
                PartNameTw = i.PartNameTw,
                PartNameEn = i.PartNameEn,
                MaterialNameTw = i.MaterialNameTw,
                MaterialNameEn = i.MaterialNameEn,
                UnitNameTw = i.UnitNameTw,
                UnitNameEn = i.UnitNameEn,
                RefKnifeNo = i.RefKnifeNo,
                PieceOfPair = i.PieceOfPair,
                AlternateType = i.AlternateType,
                UsageGiveBegin = i.UsageGiveBegin,
                UsageGiveEnd = i.UsageGiveEnd,
                Remark = i.Remark,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
                Type = i.Type,
                TotalUsage = i.TotalUsage,
            });
        }
        public IQueryable<Models.Views.MPSStyleItem> Get()
        {
            var mpsProcedures = (
                from mpd in MPSProcedure.Get()
                join pd in Procedures.Get() on new { ProceduresId = mpd.ProceduresId, LocaleId = mpd.LocaleId } equals new { ProceduresId = pd.Id, LocaleId = pd.LocaleId }
                select new { mpd.LocaleId, mpd.MpsStyleId, pd.ProcedureNameTw, mpd.ProcessId, mpd.PreProceduresId, mpd.ToStock, mpd.AccomplishRate, mpd.CountType, mpd.PieceWorker, mpd.PieceStandardTime, mpd.PieceStandardPrice, mpd.PiecePairs }
            );

            var result = (
                from si in MPSStyleItem.Get()
                join s in MPSStyle.Get() on new { MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId } equals new { MPSStyleId = s.Id, LocaleId = s.LocaleId }
                join p in mpsProcedures on new { Part = si.PartNameTw, MPSStyleId = si.MpsStyleId, LocaleId = si.LocaleId } equals new { Part = p.ProcedureNameTw, MPSStyleId = p.MpsStyleId, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new Models.Views.MPSStyleItem
                {
                    Id = si.Id,
                    MpsStyleId = si.MpsStyleId,
                    PartNameTw = si.PartNameTw,
                    PartNameEn = si.PartNameEn,
                    MaterialNameTw = si.MaterialNameTw,
                    MaterialNameEn = si.MaterialNameEn,
                    UnitNameTw = si.UnitNameTw,
                    UnitNameEn = si.UnitNameEn,
                    RefKnifeNo = si.RefKnifeNo,
                    PieceOfPair = si.PieceOfPair,
                    AlternateType = si.AlternateType,
                    UsageGiveBegin = si.UsageGiveBegin,
                    UsageGiveEnd = si.UsageGiveEnd,
                    Remark = si.Remark,
                    ModifyUserName = si.ModifyUserName,
                    LastUpdateTime = si.LastUpdateTime,

                    // for MPSProcedure
                    LocaleId = si.LocaleId,
                    Type = si.Type,
                    TotalUsage = si.TotalUsage,
                    ProcessId = mpsProcedures.Where(i => i.ProcedureNameTw == si.PartNameTw && i.LocaleId == si.LocaleId).Min(i => i.ProcessId),
                    ProceduresId = Procedures.Get().Where(i => i.ProcedureNameTw == si.PartNameTw && i.LocaleId == si.LocaleId).Max(i => i.Id),
                    ProcedureNo = Procedures.Get().Where(i => i.ProcedureNameTw == si.PartNameTw && i.LocaleId == si.LocaleId).Max(i => i.ProcedureNo),
                    PreProceduresId = p.PreProceduresId,
                    ToStock = p.ToStock,
                    AccomplishRate = p.AccomplishRate,
                    CountType = p.CountType,
                    PieceWorker = p.PieceWorker,
                    PieceStandardTime = p.PieceStandardTime,
                    PieceStandardPrice = p.PieceStandardPrice,
                    PiecePairs = p.PiecePairs,

                    StyleItemId = si.Id,
                    PartNo = Part.Get().Where(i => i.PartNameTw == si.PartNameTw && i.LocaleId == si.LocaleId).Max(i => i.PartNo),
                    PartId = Part.Get().Where(i => i.PartNameTw == si.PartNameTw && i.LocaleId == si.LocaleId).Max(i => i.Id),
                    StyleNo = s.StyleNo,
                });
            return result;
        } 
        public Models.Views.MPSStyleItem Create(Models.Views.MPSStyleItem item)
        {
            var _item = MPSStyleItem.CreateKeepId(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.MPSStyleItem Update(Models.Views.MPSStyleItem item)
        {
            var _item = MPSStyleItem.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        private ERP.Models.Entities.MpsStyleItem Build(Models.Views.MPSStyleItem item)
        {
            return new ERP.Models.Entities.MpsStyleItem
            {
                Id = item.Id,
                MpsStyleId = item.MpsStyleId,
                PartNameTw = item.PartNameTw,
                PartNameEn = item.PartNameEn,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                RefKnifeNo = item.RefKnifeNo,
                PieceOfPair = item.PieceOfPair,
                AlternateType = item.AlternateType,
                UsageGiveBegin = item.UsageGiveBegin,
                UsageGiveEnd = item.UsageGiveEnd,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                Type = item.Type,
                TotalUsage = item.TotalUsage,
            };
        }


        public void CreateRange(IEnumerable<Models.Views.MPSStyleItem> items)
        {
            MPSStyleItem.CreateRangeKeepId(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsStyleItem, bool>> predicate)
        {
            MPSStyleItem.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsStyleItem> BuildRange(IEnumerable<Models.Views.MPSStyleItem> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsStyleItem
            {
                Id = item.Id,
                MpsStyleId = item.MpsStyleId,
                PartNameTw = item.PartNameTw,
                PartNameEn = item.PartNameEn,
                MaterialNameTw = item.MaterialNameTw,
                MaterialNameEn = item.MaterialNameEn,
                UnitNameTw = item.UnitNameTw,
                UnitNameEn = item.UnitNameEn,
                RefKnifeNo = item.RefKnifeNo,
                PieceOfPair = item.PieceOfPair,
                AlternateType = item.AlternateType,
                UsageGiveBegin = item.UsageGiveBegin,
                UsageGiveEnd = item.UsageGiveEnd,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
                Type = item.Type,
                TotalUsage = item.TotalUsage,
            });
        }

    }
}
