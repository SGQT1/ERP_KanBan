using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcessSetService : EntityService<MpsProcessSet>
    {
        protected new MpsProcessSetRepository Repository { get { return base.Repository as MpsProcessSetRepository; } }
        public MpsProcessSetService(MpsProcessSetRepository repository) : base(repository)
        {
        }
    }
}

