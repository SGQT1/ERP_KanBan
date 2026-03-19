using System;
using ERP.Data.Utilities;

namespace ERP.Services.KanBan.Bases
{
    public abstract class KanBanService
    {
        protected UnitOfWork UnitOfWork { get; }

        public KanBanService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}