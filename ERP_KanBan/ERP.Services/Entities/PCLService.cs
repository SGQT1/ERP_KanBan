using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class PCLService : EntityService<PCL>
    {
        protected new PCLRepository Repository { get { return base.Repository as PCLRepository; } }

        public PCLService(PCLRepository repository) : base(repository)
        {
        }
    }
}
