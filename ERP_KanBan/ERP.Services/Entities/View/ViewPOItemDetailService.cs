using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ViewPOItemDetailService : EntityService<VIEW_POITEM_DETAIL>
    {
        protected new ViewPOItemDetailRepository Repository { get { return base.Repository as ViewPOItemDetailRepository; } }

        public ViewPOItemDetailService(ViewPOItemDetailRepository repository) : base(repository)
        {
        }
    }
}