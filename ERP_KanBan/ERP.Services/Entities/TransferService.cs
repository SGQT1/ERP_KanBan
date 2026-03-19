using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Services.Entities
{
    public class TransferService : EntityService<Transfer>
    {
        protected new TransferRepository Repository { get { return base.Repository as TransferRepository; } }

        public TransferService(TransferRepository repository) : base(repository)
        {
        }
    }
}
