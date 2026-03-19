using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;

namespace ERP.Services.Report
{
    public class POPrintService : ReportService
    {
        private ERP.Services.Business.Entities.POItemPrintLogService POItemPrintLog;

        public POPrintService(
            ERP.Services.Business.Entities.POItemPrintLogService poItemPrintLogService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            POItemPrintLog = poItemPrintLogService;
        }

        public void PringLog(IEnumerable<Models.Views.POItemPrintLog> items) {
            POItemPrintLog.CreateRange(items);
        }

    }

}