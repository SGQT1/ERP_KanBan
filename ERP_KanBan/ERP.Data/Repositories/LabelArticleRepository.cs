using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class LabelArticleRepository : Bases.BaseRepository<LabelArticle>
    {
        public LabelArticleRepository(DbContext db) : base(db)
        {
        }
    }
}