using System;
using ERP.Data.Utilities;

namespace ERP.Services.Bases
{
    public abstract class BusinessService
    {
        protected UnitOfWork UnitOfWork { get; }

        public BusinessService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}