using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyLangAddService : EntityService<MpsDailyLangAdd>
    {
        protected new MpsDailyLangAddRepository Repository { get { return base.Repository as MpsDailyLangAddRepository; } }
        public MpsDailyLangAddService(MpsDailyLangAddRepository repository) : base(repository)
        {
        }
    }
}

