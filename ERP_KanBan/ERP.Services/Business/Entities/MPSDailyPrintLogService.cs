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
    public class MPSDailyPrintLogService : BusinessService
    {
        private ERP.Services.Entities.MpsDailyPrintLogService MPSDailyPrintLog { get; set; }

        public MPSDailyPrintLogService(
            ERP.Services.Entities.MpsDailyPrintLogService mpsDailyPrintLog,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSDailyPrintLog = mpsDailyPrintLog;
        }
        public IQueryable<Models.Views.MPSDailyPrintLog> Get()
        {
            return MPSDailyPrintLog.Get().Select(i => new Models.Views.MPSDailyPrintLog
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                RefDailyNo = i.RefDailyNo,
                RefLocaleId = i.RefLocaleId,
                PrintUserName = i.PrintUserName,
                PrintTime = i.PrintTime
            });
        }
        public void CreateRange(IEnumerable<Models.Views.MPSDailyPrintLog> items)
        {
            MPSDailyPrintLog.CreateRange(BuildRange(items));
        }
        private IEnumerable<ERP.Models.Entities.MpsDailyPrintLog> BuildRange(IEnumerable<Models.Views.MPSDailyPrintLog> items)
        {
            return items.Select(item => new ERP.Models.Entities.MpsDailyPrintLog
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