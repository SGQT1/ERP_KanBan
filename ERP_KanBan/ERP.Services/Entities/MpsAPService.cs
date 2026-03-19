using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsAPService : EntityService<MpsAP>
    {
        protected new MpsAPRepository Repository { get { return base.Repository as MpsAPRepository; } }
        public MpsAPService(MpsAPRepository repository) : base(repository)
        {
        }
    }
}
