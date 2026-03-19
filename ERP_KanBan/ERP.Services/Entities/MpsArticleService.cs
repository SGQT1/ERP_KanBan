using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Services.Entities
{
    public class MpsArticleService : EntityService<MpsArticle>
    {
        protected new MpsArticleRepository Repository { get { return base.Repository as MpsArticleRepository; } }
        public MpsArticleService(MpsArticleRepository repository) : base(repository)
        {
        }
    }
}

