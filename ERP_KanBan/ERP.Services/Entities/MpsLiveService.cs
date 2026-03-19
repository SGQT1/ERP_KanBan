using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsLiveService : EntityService<MpsLive>
    {
        protected new MpsLiveRepository Repository { get { return base.Repository as MpsLiveRepository; } }
        public MpsLiveService(MpsLiveRepository repository) : base(repository)
        {
        }
    }
}

