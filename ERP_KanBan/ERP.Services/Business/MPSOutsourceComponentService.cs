using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSOutsourceComponentService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSArticleService MPSArticle { get; set; }
        private ERP.Services.Business.Entities.MPSStyleService MPSStyle { get; set; }
        private ERP.Services.Business.Entities.MPSStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Business.Entities.MPSProcedureService MPSProcedure { get; set; }
        private ERP.Services.Business.Entities.MPSProceduresService MPSProcedures { get; set; }

        public MPSOutsourceComponentService(
            ERP.Services.Business.Entities.MPSArticleService mpsArticleService,
            ERP.Services.Business.Entities.MPSStyleService mpsStyleService,
            ERP.Services.Business.Entities.MPSStyleItemService mpsStyleItemService,
            ERP.Services.Business.Entities.MPSProcedureService mpsProcedureService,
            ERP.Services.Business.Entities.MPSProceduresService mpsProceduresService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSArticle = mpsArticleService;
            MPSStyle = mpsStyleService;
            MPSStyleItem = mpsStyleItemService;
            MPSProcedure = mpsProcedureService;
            MPSProcedures = mpsProceduresService;
        }

        public ERP.Models.Views.MPSOutsourceComponentGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.MPSOutsourceComponentGroup();

            var style = MPSStyle.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (style != null)
            {
                group.MPSStyle = style;
                group.MPSProcedure = MPSProcedure.Get().Where(i => i.MpsStyleId == style.Id && i.LocaleId == style.LocaleId).ToList();

                // StyleItem 在前端可以作為匯入Procedure的資料，也可以作為 list
                var sytleItems = MPSStyleItem.Get().Where(i => i.MpsStyleId == style.Id && i.LocaleId == style.LocaleId).ToList();
                sytleItems.ForEach(i =>
                {
                    i.ProcessId = i.ProcessId == null ? 0 : i.ProcessId;
                    i.ProceduresId = i.ProceduresId == null ? 0 : i.ProceduresId;
                    i.PreProceduresId = i.ProceduresId;// i.PreProceduresId == null ? 0 : i.PreProceduresId;
                    i.ToStock = i.ToStock == null ? 1 : i.ToStock;
                    i.AccomplishRate = i.AccomplishRate == null ? 0 : i.AccomplishRate;
                    i.CountType = i.CountType == null ? 1 : i.CountType;
                    i.PieceWorker = i.PieceWorker == null ? 1 : i.PieceWorker;
                    i.PieceStandardTime = i.PieceStandardTime == null ? 1 : i.PieceStandardTime;
                    i.PieceStandardPrice = i.PieceStandardPrice == null ? 0 : i.PieceStandardPrice;
                    i.PiecePairs = i.PiecePairs == null ? 1 : i.PiecePairs;
                    i.InProcessNo = "";
                    // i.InProcessNo = i.ProceduresId == null ? "" : style.StyleNo + "-" + i.ProcedureNo + "-" + i.ProceduresId.ToString();
                });
                group.MPSStyleItem = sytleItems.Distinct();
            }
            return group;
        }

        public ERP.Models.Views.MPSOutsourceComponentGroup Save(MPSOutsourceComponentGroup group)
        {
            var style = group.MPSStyle;
            var procedure = group.MPSProcedure.ToList();
            try
            {
                UnitOfWork.BeginTransaction();

                if (procedure.Count() > 0)
                {
                    //Update MPSStyle
                    {
                        var times = procedure.Sum(i => i.PieceStandardTime);
                        var cost = procedure.Sum(i => i.PieceStandardPrice);

                        style.UnitStandardTime = times;
                        style.UnitLaborCost = cost;
                        MPSStyle.UpdateCost(style);
                    }

                    //MaterialQuot Item
                    {
                        var mpsProcedures = MPSProcedures.Get().Where(i => i.LocaleId == style.LocaleId).ToList();
                        procedure.ForEach(i =>
                        {
                            var item = mpsProcedures.Where(p => p.Id == i.ProceduresId).FirstOrDefault();

                            i.InProcessNo = item != null ? style.StyleNo + "-" + item.ProcedureNo + "-" + item.Id.ToString() : style.StyleNo + "-" + i.SortKey.ToString("000");

                        });
                        MPSProcedure.RemoveRange(i => i.MpsStyleId == style.Id && i.LocaleId == style.LocaleId);
                        MPSProcedure.CreateRange(procedure);
                    }
                }
                UnitOfWork.Commit();
                return Get((int)style.Id, (int)style.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.MPSOutsourceComponentGroup Remove(MPSOutsourceComponentGroup group)
        {
            var style = group.MPSStyle;
            try
            {
                UnitOfWork.BeginTransaction();
                //Update MPSStyle
                {
                    style.UnitStandardTime = 0;
                    style.UnitLaborCost = 0;
                    style.DollarNameTw = null;

                    MPSStyle.UpdateCost(style);
                }
                MPSProcedure.RemoveRange(i => i.MpsStyleId == style.Id && i.LocaleId == style.LocaleId);
                UnitOfWork.Commit();
                return Get((int)style.Id, (int)style.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

    }
}
