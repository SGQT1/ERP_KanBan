using ERP.Data.Repositories;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ViewOrdersItemSizeRunService : EntityService<VIEW_ORDERSITEM_SIZERUN>
    {
        protected new ViewOrdersItemSizeRunRepository Repository { get { return base.Repository as ViewOrdersItemSizeRunRepository; } }

        public ViewOrdersItemSizeRunService(ViewOrdersItemSizeRunRepository repository) : base(repository)
        {
        }
    }
}