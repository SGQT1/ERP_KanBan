using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using System.Linq;

namespace ERP.Services.Entities
{
    public class CustomerService : EntityService<Customer>
    {
        protected new CustomerRepository Repository { get { return base.Repository as CustomerRepository; } }

        public CustomerService(CustomerRepository repository) : base(repository)
        {
        }
    }
}