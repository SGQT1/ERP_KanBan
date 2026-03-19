using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsStyleService : EntityService<MpsStyle>
    {
        protected new MpsStyleRepository Repository { get { return base.Repository as MpsStyleRepository; } }
        public MpsStyleService(MpsStyleRepository repository) : base(repository)
        {
        }
    }
}

