using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class PackMarkService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.PackPlanService PackPlan { get; set; }
        private ERP.Services.Business.Entities.PackMarkService PackMark { get; set; }
        public PackMarkService(
            ERP.Services.Business.Entities.PackMarkService packMarkService,
            ERP.Services.Business.Entities.PackPlanService packPlanService,
            ERP.Services.Business.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackMark = packMarkService;
            PackPlan = packPlanService;
            Orders = ordersService;
        }

        public ERP.Models.Views.PackMark Get(int orders, int localeId, string edition)
        {
            var packMark = PackMark.Get().Where(i => i.RefOrdersId == orders && i.LocaleId == localeId && i.Edition == edition).FirstOrDefault();
            packMark.MarkPhoto = PackMark.GetMarkPhoto(packMark.MarkPhotoURL);
            packMark.SideMarkPhoto = PackMark.GetMarkPhoto(packMark.SideMarkPhotoURL);
            packMark.Add1Photo = PackMark.GetMarkPhoto(packMark.Add1PhotoURL);
            
            //get column from orders.
            var order = Orders.Get().Where(i => i.Id == orders && i.OrderNo == packMark.RefOrderNo).FirstOrDefault();
            packMark.RefCustomer = order.Customer;
            packMark.RefBrand = order.Brand;
            packMark.RefCompany = order.CompanyNo;
            packMark.RefStyleNo = order.StyleNo;
            packMark.RefShoeName = order.ShoeName;
            packMark.RefQty = order.OrderQty;
            return packMark;
        }
        // public ERP.Models.Views.PackMark BuildPackMark(int orders, string no, int localeId, string edition)
        public ERP.Models.Views.PackMark BuildPackMark(PackMark packMark)
        {
            var order = Orders.Get().Where(i => i.OrderNo == packMark.RefOrderNo && i.Id == packMark.RefOrdersId).FirstOrDefault();
            if(order != null){
                var simMark = PackMark.GetSimMark(packMark);

                packMark.MarkTitle = simMark == null ? "" : simMark.MarkTitle;
                packMark.MarkDesc = order.Mark;
                packMark.MarkPhoto = "";
                    
                packMark.SubMarkTitle = simMark == null ? "" : simMark.SubMarkTitle;
                packMark.SideMarkDesc = order.SideMark;
                packMark.SideMarkPhoto = "";

                packMark.Add1Desc = simMark == null ? "" : simMark.Add1Desc;
                packMark.Add1Photo = "";

                packMark.DeliveryAddress = simMark == null? "" : simMark.DeliveryAddress;
                
                return packMark;
            }

            return null;
        }
        public ERP.Models.Views.PackMark SavePackMark(PackMark packMark)
        {
            try
            {
                UnitOfWork.BeginTransaction();
                if (packMark != null)
                {
                    var _packLabel = PackMark.Get()
                        .Where(i => i.LocaleId == packMark.LocaleId && i.RefOrdersId == packMark.RefOrdersId && i.Edition == packMark.Edition)
                        .FirstOrDefault();
                    if (_packLabel == null)
                    {
                        packMark = PackMark.Create(packMark);
                    }
                    else
                    {
                        packMark = PackMark.Update(packMark);

                        //update Orders Mark
                        var order = Orders.Get(i => i.Id == packMark.RefOrdersId && i.LocaleId == i.LocaleId).FirstOrDefault();
                        order.Mark = packMark.MarkDesc;
                        order.SideMark = packMark.SideMarkDesc;
                        Orders.Update(order);
                    }
                }
                UnitOfWork.Commit();
                return Get((int)packMark.RefOrdersId,(int)packMark.LocaleId,packMark.Edition);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

        }
        public void Remove(PackMark packMark)
        {
            try
            {
                UnitOfWork.BeginTransaction();
                PackMark.Remove(packMark);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
