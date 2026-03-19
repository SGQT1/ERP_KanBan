using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsDailyLangService : EntityService<MpsDailyLang>
    {
        protected new MpsDailyLangRepository Repository { get { return base.Repository as MpsDailyLangRepository; } }
        public MpsDailyLangService(MpsDailyLangRepository repository) : base(repository)
        {
        }
    }
}

