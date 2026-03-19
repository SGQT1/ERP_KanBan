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
    public class LabelCustomerService : BusinessService
    {
        private ERP.Services.Business.Entities.CustomerService Customer { get; set; }
        private ERP.Services.Business.Entities.LabelCustomerService LabelCustomer { get; set; }

        public LabelCustomerService(
            ERP.Services.Business.Entities.CustomerService customerService,
            ERP.Services.Business.Entities.LabelCustomerService labelCustomerService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Customer = customerService;
            LabelCustomer = labelCustomerService;
        }

        public ERP.Models.Views.LabelCustomerGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.LabelCustomerGroup {};
            var customer = Customer.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            var labelCustomer = LabelCustomer.Get().Where(i => i.CustomerId == id && i.LocaleId == localeId).FirstOrDefault();

            if(labelCustomer != null) {
                labelCustomer.LabelCustomer01Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer01PhotoURL);
                labelCustomer.LabelCustomer02Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer02PhotoURL);
                labelCustomer.LabelCustomer03Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer03PhotoURL);
                labelCustomer.LabelCustomer04Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer04PhotoURL);
                labelCustomer.LabelCustomer05Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer05PhotoURL);
                labelCustomer.LabelCustomer06Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer06PhotoURL);
                labelCustomer.LabelCustomer07Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer07PhotoURL);
                labelCustomer.LabelCustomer08Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer08PhotoURL);
                labelCustomer.LabelCustomer09Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer09PhotoURL);
                labelCustomer.LabelCustomer10Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer10PhotoURL);
                labelCustomer.LabelCustomer11Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer11PhotoURL);
                labelCustomer.LabelCustomer12Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer12PhotoURL);
                labelCustomer.LabelCustomer13Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer13PhotoURL);
                labelCustomer.LabelCustomer14Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer14PhotoURL);
                labelCustomer.LabelCustomer15Photo = LabelCustomer.GetPhoto(labelCustomer.LabelCustomer15PhotoURL);
                labelCustomer.PackDescPhoto = LabelCustomer.GetPhoto(labelCustomer.PackDescPhotoURL);
                labelCustomer.PackDescEngPhoto = LabelCustomer.GetPhoto(labelCustomer.PackDescEngPhotoURL);
            }

            if (customer != null)
            {
                group.Customer = customer;
                group.LabelCustomer = labelCustomer == null ? new Models.Views.LabelCustomer() : labelCustomer;
            }
            return group;
        }

        public ERP.Models.Views.LabelCustomerGroup Save(ERP.Models.Views.LabelCustomerGroup item)
        {
            var customer = item.Customer;
            var labelCustomer = item.LabelCustomer;

            UnitOfWork.BeginTransaction();
            try
            {
                var _item = LabelCustomer.Get().Where(i => i.Id == labelCustomer.Id && i.LocaleId == labelCustomer.LocaleId && i.CustomerId == customer.Id).FirstOrDefault();

                if (_item != null)
                {
                    labelCustomer.Id = _item.Id;
                    labelCustomer.CustomerId = customer.Id;
                    labelCustomer.LocaleId = _item.LocaleId;

                    labelCustomer = LabelCustomer.Update(labelCustomer);
                }
                else
                {
                    labelCustomer = LabelCustomer.Create(labelCustomer);
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
        public void Remove(ERP.Models.Views.LabelCustomerGroup item)
        {
            var customer = item.Customer;
            
            UnitOfWork.BeginTransaction();
            try
            {
                LabelCustomer.RemoveRange(i => i.CustomerId == customer.Id && i.LocaleId == customer.LocaleId);
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
    }
}