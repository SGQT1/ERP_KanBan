using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class CustomerService : BusinessService
    {
        private ERP.Services.Business.Entities.CustomerService Customer { get; set; }
        private ERP.Services.Business.Entities.LabelCustomerService LabelCustomer { get; set; }

        public CustomerService(
            ERP.Services.Business.Entities.CustomerService customerService,
            ERP.Services.Business.Entities.LabelCustomerService labelCustomerService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Customer = customerService;
            LabelCustomer = labelCustomerService;
        }

        public ERP.Models.Views.CustomerGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.CustomerGroup {};
            var customer = Customer.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
         
            if (customer != null)
            {
                group.Customer = customer;
            }
            return group;
        }

        public ERP.Models.Views.CustomerGroup Save(ERP.Models.Views.CustomerGroup item)
        {
            var customer = item.Customer;

            UnitOfWork.BeginTransaction();
            try
            {
                
                // Id >> exist, ChineseName >> duplicate
                var _item = Customer.Get().Where(i => i.LocaleId == customer.LocaleId && i.Id == customer.Id).FirstOrDefault();

                if (_item != null)
                {
                    customer.Id = _item.Id;
                    customer.LocaleId = _item.LocaleId;

                    customer = Customer.Update(customer);
                }
                else
                {
                    customer = Customer.Create(customer);
                }

                UnitOfWork.Commit();
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
                return item;
            }
            return Get((int)customer.Id, (int)customer.LocaleId);
        }
        public void Remove(ERP.Models.Views.CustomerGroup item)
        {
            var customer = item.Customer;
            
            UnitOfWork.BeginTransaction();
            try
            {
                LabelCustomer.RemoveRange(i => i.CustomerId == customer.Id && i.LocaleId == customer.LocaleId);
                Customer.Remove(customer);
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
    }
}