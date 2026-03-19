using System;
using ERP.Data.Utilities;

namespace ERP.Services.Bases
{
    public abstract class ReportService
    {
        protected UnitOfWork UnitOfWork { get; }

        public ReportService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}