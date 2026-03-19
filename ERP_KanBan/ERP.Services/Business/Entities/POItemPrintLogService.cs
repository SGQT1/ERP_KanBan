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
    public class POItemPrintLogService : BusinessService
    {
        private ERP.Services.Entities.POItemPrintLogService POItemPrintLog { get; set; }

        public POItemPrintLogService(
            ERP.Services.Entities.POItemPrintLogService poItemPrintLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            POItemPrintLog = poItemPrintLogService;
        }
        public IQueryable<Models.Views.POItemPrintLog> Get()
        {
            return POItemPrintLog.Get().Select(i => new Models.Views.POItemPrintLog
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                RefPOItemId = i.RefPOItemId,
                RefLocaleId = i.RefLocaleId,
                PrintUserName = i.PrintUserName,
                PrintTime = i.PrintTime
            });
        }
        public void CreateRange(IEnumerable<Models.Views.POItemPrintLog> items)
        {
            POItemPrintLog.CreateRange(BuildRange(items));
        }
        private IEnumerable<ERP.Models.Entities.POItemPrintLog> BuildRange(IEnumerable<Models.Views.POItemPrintLog> items)
        {
            return items.Select(item => new ERP.Models.Entities.POItemPrintLog
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefPOItemId = item.RefPOItemId,
                RefLocaleId = item.RefLocaleId,
                PrintUserName = item.PrintUserName,
                PrintTime = DateTime.Now,
            });
        }


    }
}