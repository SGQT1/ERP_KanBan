using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class WIPInvTxnService : BusinessService
    {
        private ERP.Services.Business.Entities.WIPInvTxnService WIPInvTxn { get; set; }
        private ERP.Services.Business.Entities.WIPInvTxnIOService WIPInvTxnIO { get; set; }
        private ERP.Services.Business.Entities.WIPInvTxnIOSizeService WIPInvTxnIOSize { get; set; }
        public WIPInvTxnService(
            ERP.Services.Business.Entities.WIPInvTxnService wipInvTxnService,
            ERP.Services.Business.Entities.WIPInvTxnIOService wipInvTxnIOService,
            ERP.Services.Business.Entities.WIPInvTxnIOSizeService wipInvTxnIOSizeService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            WIPInvTxn = wipInvTxnService;
            WIPInvTxnIO = wipInvTxnIOService;
            WIPInvTxnIOSize = wipInvTxnIOSizeService;
        }

        public ERP.Models.Views.WIPInvTxnGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.WIPInvTxnGroup();

            var wip = WIPInvTxn.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (wip != null)
            {
                var items = WIPInvTxnIO.Get().Where(i => i.WIPInvTxnId == id && i.LocaleId == localeId).ToList();
                group.WIPInvTxn = wip;
                group.WIPInvTxnIO = items;
            }
            return group;
        }
        public ERP.Models.Views.WIPInvTxnGroup Save(WIPInvTxnGroup item)
        {
            var wipInvTxn = item.WIPInvTxn;
            var wipInvTxnIO = item.WIPInvTxnIO;

            UnitOfWork.BeginTransaction();
            try
            {
                // Id >> exist, ChineseName >> duplicate
                var _item = WIPInvTxn.Get().Where(i => i.Id == wipInvTxn.Id && i.LocaleId == wipInvTxn.LocaleId).FirstOrDefault();

                if (_item != null)
                {
                    wipInvTxn.Id = _item.Id;
                    wipInvTxn.LocaleId = _item.LocaleId;
                    wipInvTxn = WIPInvTxn.Update(wipInvTxn);
                }
                else
                {
                    wipInvTxn = WIPInvTxn.Create(wipInvTxn);
                }

                UnitOfWork.Commit();
                return Get((int)wipInvTxn.Id, (int)wipInvTxn.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void Remove(WIPInvTxnGroup group)
        {
            var wipInvTxn = group.WIPInvTxn;
            var wipInvTxnIO = group.WIPInvTxnIO;

            UnitOfWork.BeginTransaction();
            try
            {
                // WIPInvTxnIO.Remove(wipInvTxnIO);
                // WIPInvTxn.Remove(wipInvTxn);

                // UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
