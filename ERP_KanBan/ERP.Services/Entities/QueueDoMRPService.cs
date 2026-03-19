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
    public class QueueDoMRPService : EntityService<QueueDoMRP>
    {
        protected new QueueDoMRPRepository Repository { get { return base.Repository as QueueDoMRPRepository; } }

        public QueueDoMRPService(QueueDoMRPRepository repository) : base(repository)
        {
        }
    }
}
