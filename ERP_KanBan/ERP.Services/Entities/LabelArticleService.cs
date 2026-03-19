using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class LabelArticleService : EntityService<LabelArticle>
    {
        protected new LabelArticleRepository Repository { get { return base.Repository as LabelArticleRepository; } }

        public LabelArticleService(LabelArticleRepository repository) : base(repository)
        {
        }
    }
}