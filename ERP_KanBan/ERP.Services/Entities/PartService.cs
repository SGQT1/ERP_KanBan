using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class PartService : EntityService<Part>
    {
        protected new PartRepository Repository { get { return base.Repository as PartRepository; } }

        public PartService(PartRepository repository) : base(repository)
        {
        }
    }
}
