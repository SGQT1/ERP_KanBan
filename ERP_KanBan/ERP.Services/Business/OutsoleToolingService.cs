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
    public class OutsoleToolingService : BusinessService
    {
        private ERP.Services.Business.Entities.OutsoleToolingService OutsoleTooling { get; set; }
        public OutsoleToolingService(
            ERP.Services.Business.Entities.OutsoleToolingService outsoleToolingService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            OutsoleTooling = outsoleToolingService;
        }

        public ERP.Models.Views.OutsoleTooling GetOutsoleTooling (int id, int localeId) {
            return OutsoleTooling.Get().Where (i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
        }
        public ERP.Models.Views.OutsoleTooling SaveOutsoleTooling (ERP.Models.Views.OutsoleTooling item) {
            UnitOfWork.BeginTransaction ();
            try {
                var _item = OutsoleTooling.Get ().Where (i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();
                if (_item != null) {
                    item = OutsoleTooling.Update (item);
                } else {
                    item = OutsoleTooling.Create (item);
                }
                UnitOfWork.Commit ();
                return GetOutsoleTooling ((int) item.Id, (int) item.LocaleId);
            } catch {
                UnitOfWork.Rollback ();
                return item;
            }
        }
        public void RemoveOutsoleTooling (ERP.Models.Views.OutsoleTooling item) {
            UnitOfWork.BeginTransaction ();
            try {
                OutsoleTooling.Remove (item);
                UnitOfWork.Commit();
            }
            catch {
                UnitOfWork.Rollback ();
            }
        }
    }
}
