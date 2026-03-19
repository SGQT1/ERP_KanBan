using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views.Common;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class OutsoleTCService : BusinessService
    {
        private ERP.Services.Business.Entities.OutsoleTCService OutsoleTC { get; set; }
        public OutsoleTCService(
            ERP.Services.Business.Entities.OutsoleTCService outsoleTCService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OutsoleTC = outsoleTCService;
        }
        public ERP.Models.Views.OutsoleTCGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.OutsoleTCGroup { };
            var outsoleTC = OutsoleTC.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (outsoleTC != null)
            {
                group.OutsoleTC = outsoleTC;
                group.OutsoleTCHistory = OutsoleTC.Get().Where(i => i.LocaleId == localeId && i.TCType == outsoleTC.TCType && i.OutsoleNo == outsoleTC.OutsoleNo).ToList();
            }
            return group;
        }

        public IEnumerable<ERP.Models.Views.OutsoleTC> GetOutsoleTCHistory(int localeId, int type, string outsole)
        {
            return OutsoleTC.Get().Where(i => i.LocaleId == localeId && i.TCType == type && i.OutsoleNo == outsole).ToList();
        }

        public ERP.Models.Views.OutsoleTCGroup Save(ERP.Models.Views.OutsoleTC item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var _item = OutsoleTC.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();
                if (_item != null)
                {
                    item = OutsoleTC.Update(item);
                }
                else
                {
                    item = OutsoleTC.Create(item);
                }
                UnitOfWork.Commit();
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(ERP.Models.Views.OutsoleTC item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                OutsoleTC.Remove(item);
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }

        public IEnumerable<ERP.Models.Views.OutsoleTC> CloseOutsoleTC(IEnumerable<ERP.Models.Views.OutsoleTC> outsoleTCs)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                if (outsoleTCs != null || outsoleTCs.Count() > 0)
                {
                    var confirmer = outsoleTCs.Select(i => i.Confirmer).FirstOrDefault();
                    var localeId = outsoleTCs.Select(i => i.LocaleId).FirstOrDefault();
                    var confirmIds = outsoleTCs.Where(i => i.IsClose == true).Select(i => i.Id);
                    var unConfirmIds = outsoleTCs.Where(i => i.IsClose == false).Select(i => i.Id);
                    OutsoleTC.UpdateConfirm((int)localeId, confirmer, confirmIds, unConfirmIds);
                }
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
            return outsoleTCs;
        }
    }
}