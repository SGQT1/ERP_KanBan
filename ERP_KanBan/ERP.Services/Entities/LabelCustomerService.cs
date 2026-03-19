using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class LabelCustomerService : EntityService<LabelCustomer>
    {
        protected new LabelCustomerRepository Repository { get { return base.Repository as LabelCustomerRepository; } }

        public LabelCustomerService(LabelCustomerRepository repository) : base(repository)
        {
        }
    }
}