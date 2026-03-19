using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ArticlePartService : EntityService<ArticlePart>
    {
        protected new ArticlePartRepository Repository { get { return base.Repository as ArticlePartRepository; } }

        public ArticlePartService(ArticlePartRepository repository) : base(repository)
        {
        }
    }
}