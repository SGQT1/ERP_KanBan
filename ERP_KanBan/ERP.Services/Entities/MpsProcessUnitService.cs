using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsProcessUnitService : EntityService<MpsProcessUnit>
    {
        protected new MpsProcessUnitRepository Repository { get { return base.Repository as MpsProcessUnitRepository; } }
        public MpsProcessUnitService(MpsProcessUnitRepository repository) : base(repository)
        {
        }
    }
}
