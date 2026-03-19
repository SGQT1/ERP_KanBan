using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcessService : EntityService<MpsProcess>
    {
        protected new MpsProcessRepository Repository { get { return base.Repository as MpsProcessRepository; } }
        public MpsProcessService(MpsProcessRepository repository) : base(repository)
        {
        }
    }
}

