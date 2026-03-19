using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class MPSDailyAddPrintLogService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyAddPrintLogService MPSDailyAddPrintLog { get; set; }

        public MPSDailyAddPrintLogService(
            ERP.Services.Entities.MpsDailyAddPrintLogService mpsDailyAddPrintLog,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyAddPrintLog = mpsDailyAddPrintLog;
        }
        public IQueryable<Models.Views.MPSDailyAddPrintLog> Get()
        {
            return MPSDailyAddPrintLog.Get().Select(i => new Models.Views.MPSDailyAddPrintLog
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                RefDailyNo = i.RefDailyNo,
                RefLocaleId = i.RefLocaleId,
                PrintUserName = i.PrintUserName,
                PrintTime = i.PrintTime
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSDailyAddPrintLog> items)
        {
            MPSDailyAddPrintLog.CreateRange(BuildRange(items));
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyAddPrintLog> BuildRange(IEnumerable<Models.Views.MPSDailyAddPrintLog> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyAddPrintLog
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefDailyNo = item.RefDailyNo,
                RefLocaleId = item.RefLocaleId,
                PrintUserName = item.PrintUserName,
                PrintTime = DateTime.Now,
            });
        }


    }
}