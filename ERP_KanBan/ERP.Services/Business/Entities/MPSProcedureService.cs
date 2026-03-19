using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MPSProcedureService : BusinessService
    {
        private Services.Entities.MpsArticleService MPSArticle { get; }
        private Services.Entities.MpsStyleService MPSStyle { get; }
        private Services.Entities.MpsProcedureService MPSProcedure { get; }
        private Services.Entities.ProceduresService Procedures { get; }
        private Services.Entities.MpsProcessService MPSProcess { get; }

        public MPSProcedureService(
            Services.Entities.MpsProcedureService mpsProcedureService,
            Services.Entities.MpsArticleService mpsArticleService,
            Services.Entities.MpsStyleService mpsStyleService,
            Services.Entities.ProceduresService proceduresService,
            Services.Entities.MpsProcessService mpsProcessService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSArticle = mpsArticleService;
            MPSStyle = mpsStyleService;
            MPSProcedure = mpsProcedureService;
            Procedures = proceduresService;
            MPSProcess = mpsProcessService;
        }
        public IQueryable<Models.Views.MPSProcedure> Get()
        {
            return MPSProcedure.Get().Select(i => new Models.Views.MPSProcedure
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                MpsStyleId = i.MpsStyleId,
                MpsStyleItemId = i.MpsStyleItemId,
                ProcessId = i.ProcessId,
                ProceduresId = i.ProceduresId,
                PreProceduresId = i.PreProceduresId,
                InProcessNo = i.InProcessNo,
                CountType = i.CountType,
                PieceWorker = i.PieceWorker,
                PieceStandardPrice = i.PieceStandardPrice,
                PieceStandardTime = i.PieceStandardTime,
                PiecePairs = i.PiecePairs,
                PairsStandardTime = i.PairsStandardTime,
                PairsStandardPrice = i.PairsStandardPrice,
                AccomplishRate = i.AccomplishRate,
                ToStock = i.ToStock,
                SortKey = i.SortKey,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
            });
        }
        public IQueryable<ERP.Models.Views.MPSProcedure> GetWithItem()
        {
            var result = (
                from s in MPSStyle.Get()
                join a in MPSArticle.Get() on new { MpsArticleId = s.MpsArticleId, LocaleId = s.LocaleId } equals new { MpsArticleId = a.Id, LocaleId = a.LocaleId }
                join mp in MPSProcedure.Get() on new { MpsStyleId = s.Id, LocaleId = s.LocaleId } equals new { MpsStyleId = mp.MpsStyleId, LocaleId = mp.LocaleId }
                join p in Procedures.Get() on new { ProceduresId = mp.ProceduresId, LocaleId = mp.LocaleId } equals new { ProceduresId = p.Id, LocaleId = p.LocaleId }
                join mprs in MPSProcess.Get() on new { ProcessId = mp.ProcessId, LocaleId = mp.LocaleId } equals new { ProcessId = mprs.Id, LocaleId = mprs.LocaleId }
                select new ERP.Models.Views.MPSProcedure
                {
                    Id = mp.Id,
                    LocaleId = mp.LocaleId,
                    MpsStyleId = mp.MpsStyleId,
                    MpsStyleItemId = mp.MpsStyleItemId,
                    ProcessId = mp.ProcessId,
                    ProceduresId = mp.ProceduresId,
                    PreProceduresId = mp.PreProceduresId,
                    InProcessNo = mp.InProcessNo,
                    CountType = mp.CountType,
                    PieceWorker = mp.PieceWorker,
                    PieceStandardPrice = mp.PieceStandardPrice,
                    PieceStandardTime = mp.PieceStandardTime,
                    PiecePairs = mp.PiecePairs,
                    PairsStandardTime = mp.PairsStandardTime,
                    PairsStandardPrice = mp.PairsStandardPrice,
                    AccomplishRate = mp.AccomplishRate,
                    ToStock = mp.ToStock,
                    SortKey = mp.SortKey,
                    ModifyUserName = mp.ModifyUserName,
                    LastUpdateTime = mp.LastUpdateTime,
                    MpsStyleNo = s.StyleNo,
                    ProcedureNameTw = p.ProcedureNameTw,
                    ProcessNameTw = mprs.ProcessNameTw,
                }
            );
            return result;
        }

        public IQueryable<ERP.Models.Views.MPSStyle> GetWithMPSStyle(string predicate)
        {
            var result = (
                from s in MPSStyle.Get()
                join a in MPSArticle.Get() on new { MpsArticleId = s.MpsArticleId, LocaleId = s.LocaleId } equals new { MpsArticleId = a.Id, LocaleId = a.LocaleId }
                join p in MPSProcedure.Get() on new { MpsStyleId = s.Id, LocaleId = s.LocaleId } equals new { MpsStyleId = p.MpsStyleId, LocaleId = p.LocaleId } into pGRP
                from p in pGRP.DefaultIfEmpty()
                select new
                {
                    Id = s.Id,
                    MpsArticleId = s.MpsArticleId,
                    StyleNo = s.StyleNo,
                    ColorDesc = s.ColorDesc,
                    LocaleId = s.LocaleId,
                    SizeCountryCodeId = s.SizeCountryCodeId,
                    ModifyUserName = s.ModifyUserName,
                    LastUpdateTime = s.LastUpdateTime,
                    RefOrderNo = s.RefOrderNo,
                    DoUsage = s.DoUsage,
                    DollarNameTw = s.DollarNameTw,
                    UnitRelaxTime = s.UnitRelaxTime,
                    UnitStandardTime = s.UnitStandardTime,
                    UnitLaborCost = s.UnitLaborCost,
                    RefOrderNoOfMaterial = s.RefOrderNoOfMaterial,
                    HasProcedure = p != null ? 1 : 0,
                    ArticleNo = a.ArticleNo,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.MPSStyle
            {
                Id = i.Id,
                MpsArticleId = i.MpsArticleId,
                StyleNo = i.StyleNo,
                ColorDesc = i.ColorDesc,
                LocaleId = i.LocaleId,
                SizeCountryCodeId = i.SizeCountryCodeId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                RefOrderNo = i.RefOrderNo,
                DoUsage = i.DoUsage,
                DollarNameTw = i.DollarNameTw,
                UnitRelaxTime = i.UnitRelaxTime,
                UnitStandardTime = i.UnitStandardTime,
                UnitLaborCost = i.UnitLaborCost,
                RefOrderNoOfMaterial = i.RefOrderNoOfMaterial,
                ArticleNo = i.ArticleNo,
                HasProcedure = i.HasProcedure,
            })
            .Distinct();
            return result;
        }


        public void CreateRange(IEnumerable<Models.Views.MPSProcedure> items)
        {
            MPSProcedure.CreateRange(BuildRange(items));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.MpsProcedure, bool>> predicate)
        {
            MPSProcedure.RemoveRange(predicate);
        }
        private IEnumerable<ERP.Models.Entities.MpsProcedure> BuildRange(IEnumerable<Models.Views.MPSProcedure> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsProcedure
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                MpsStyleId = item.MpsStyleId,
                MpsStyleItemId = item.MpsStyleItemId,
                ProcessId = item.ProcessId,
                ProceduresId = item.ProceduresId,
                PreProceduresId = item.PreProceduresId,
                InProcessNo = item.InProcessNo,
                CountType = item.CountType,
                PieceWorker = item.PieceWorker,
                PieceStandardPrice = item.PieceStandardPrice,
                PieceStandardTime = item.PieceStandardTime,
                PiecePairs = item.PiecePairs,
                PairsStandardTime = item.PairsStandardTime,
                PairsStandardPrice = item.PairsStandardPrice,
                AccomplishRate = item.AccomplishRate,
                ToStock = item.ToStock,
                SortKey = item.SortKey,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
            });
        }
    }
}