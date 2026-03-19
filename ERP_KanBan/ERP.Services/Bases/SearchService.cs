using System;
using ERP.Data.Utilities;

namespace ERP.Services.Bases
{
    public abstract class SearchService
    {
        protected UnitOfWork UnitOfWork { get; }

        public SearchService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}