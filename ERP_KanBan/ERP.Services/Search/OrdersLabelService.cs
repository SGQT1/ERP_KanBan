using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace ERP.Services.Search
{
    public class OrdersLabelService : SearchService
    {

        private Services.Entities.OrdersService Orders { get; set; }
        private Services.Entities.LabelArticleService LabelArticle { get; set; }
        private Services.Entities.LabelCustomerService LabelCustomer { get; set; }

        public OrdersLabelService(
            Services.Entities.OrdersService ordersService,
            Services.Entities.LabelArticleService labelArticleService,
            Services.Entities.LabelCustomerService labelCustomerService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            LabelArticle = labelArticleService;
            LabelCustomer = labelCustomerService;
        }


        public IQueryable<Models.Views.OrdersLabel> GetOrdersLabel(string predicate)
        {
            var ordersLabel = (
                from o in Orders.Get()
                join la in LabelArticle.Get() on new { ArticleId = o.ArticleId, LocaleId = o.LocaleId } equals new { ArticleId = la.ArticleId, LocaleId = la.LocaleId } into laGRP
                from la in laGRP.DefaultIfEmpty()
                join lc in LabelCustomer.Get() on new { CustomerId = o.CustomerId, LocaleId = o.LocaleId } equals new { CustomerId = lc.CustomerId, LocaleId = lc.LocaleId } into lcGRP
                from lc in lcGRP.DefaultIfEmpty()
                select new Models.Views.OrdersLabel
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    CompanyId = o.CompanyId,
                    CompanyNo = o.CompanyNo,
                    OrderNo = o.OrderNo,
                    CSD = o.CSD,
                    LCSD = o.LCSD,
                    OWD = o.OWD,
                    OrderQty = o.OrderQty,
                    ArticleId = o.ArticleId,
                    ArticleNo = o.ArticleNo,
                    ArticleName = o.ArticleName,
                    CustomerId = o.CustomerId,
                    Customer = o.Customer,
                    BrandCodeId = o.BrandCodeId,
                    Brand = o.Brand,

                    LabelArticle01 = la.LabelArticle01,
                    LabelArticle01PhotoURL = la.LabelArticle01PhotoURL,
                    // LabelArticle01Photo = la.LabelArticle01Photo,
                    LabelArticle02 = la.LabelArticle02,
                    LabelArticle02PhotoURL = la.LabelArticle02PhotoURL,
                    // LabelArticle02Photo = la.LabelArticle02Photo,
                    LabelArticle03 = la.LabelArticle03,
                    LabelArticle03PhotoURL = la.LabelArticle03PhotoURL,
                    // LabelArticle03Photo = la.LabelArticle03Photo,
                    LabelArticle04 = la.LabelArticle04,
                    LabelArticle04PhotoURL = la.LabelArticle04PhotoURL,
                    // LabelArticle04Photo = la.LabelArticle04Photo,
                    LabelArticle05 = la.LabelArticle05,
                    LabelArticle05PhotoURL = la.LabelArticle05PhotoURL,
                    // LabelArticle05Photo = la.LabelArticle05Photo,
                    LabelArticle06 = la.LabelArticle06,
                    LabelArticle06PhotoURL = la.LabelArticle06PhotoURL,
                    // LabelArticle06Photo = la.LabelArticle06Photo,
                    LabelArticle07 = la.LabelArticle07,
                    LabelArticle07PhotoURL = la.LabelArticle07PhotoURL,
                    // LabelArticle07Photo = la.LabelArticle07Photo,


                    LabelCustomer01 = lc.LabelCustomer01,
                    LabelCustomer01PhotoURL = lc.LabelCustomer01PhotoURL,
                    // LabelCustomer01Photo = lc.LabelCustomer01Photo,
                    LabelCustomer02 = lc.LabelCustomer02,
                    LabelCustomer02PhotoURL = lc.LabelCustomer02PhotoURL,
                    // LabelCustomer02Photo = lc.LabelCustomer02Photo,
                    LabelCustomer03 = lc.LabelCustomer03,
                    LabelCustomer03PhotoURL = lc.LabelCustomer03PhotoURL,
                    // LabelCustomer03Photo = lc.LabelCustomer03Photo,
                    LabelCustomer04 = lc.LabelCustomer04,
                    LabelCustomer04PhotoURL = lc.LabelCustomer04PhotoURL,
                    // LabelCustomer04Photo = lc.LabelCustomer04Photo,
                    LabelCustomer05 = lc.LabelCustomer05,
                    LabelCustomer05PhotoURL = lc.LabelCustomer05PhotoURL,
                    // LabelCustomer05Photo = lc.LabelCustomer05Photo,
                    LabelCustomer06 = lc.LabelCustomer06,
                    LabelCustomer06PhotoURL = lc.LabelCustomer06PhotoURL,
                    // LabelCustomer06Photo = lc.LabelCustomer06Photo,
                    LabelCustomer07 = lc.LabelCustomer07,
                    LabelCustomer07PhotoURL = lc.LabelCustomer07PhotoURL,
                    // LabelCustomer07Photo = lc.LabelCustomer07Photo,
                    LabelCustomer08 = lc.LabelCustomer08,
                    LabelCustomer08PhotoURL = lc.LabelCustomer08PhotoURL,
                    // LabelCustomer08Photo = lc.LabelCustomer08Photo,
                    LabelCustomer09 = lc.LabelCustomer09,
                    LabelCustomer09PhotoURL = lc.LabelCustomer09PhotoURL,
                    // LabelCustomer09Photo = lc.LabelCustomer09Photo,
                    LabelCustomer10 = lc.LabelCustomer10,
                    LabelCustomer10PhotoURL = lc.LabelCustomer10PhotoURL,
                    // LabelCustomer10Photo = lc.LabelCustomer10Photo,
                    LabelCustomer11 = lc.LabelCustomer11,
                    LabelCustomer11PhotoURL = lc.LabelCustomer11PhotoURL,
                    // LabelCustomer11Photo = lc.LabelCustomer11Photo,
                    LabelCustomer12 = lc.LabelCustomer12,
                    LabelCustomer12PhotoURL = lc.LabelCustomer12PhotoURL,
                    // LabelCustomer12Photo = lc.LabelCustomer12Photo,
                    LabelCustomer13 = lc.LabelCustomer13,
                    LabelCustomer13PhotoURL = lc.LabelCustomer13PhotoURL,
                    // LabelCustomer13Photo = lc.LabelCustomer13Photo,
                    LabelCustomer14 = lc.LabelCustomer14,
                    LabelCustomer14PhotoURL = lc.LabelCustomer14PhotoURL,
                    // LabelCustomer14Photo = lc.LabelCustomer14Photo,
                    LabelCustomer15 = lc.LabelCustomer15,
                    LabelCustomer15PhotoURL = lc.LabelCustomer15PhotoURL,
                    // LabelCustomer15Photo = lc.LabelCustomer15Photo,
                    PackDesc = lc.PackDesc,
                    PackDescPhotoURL = lc.PackDescPhotoURL,
                    // PackDescPhoto = lc.PackDescPhoto,
                    PackDescEng = lc.PackDescEng,
                    PackDescEngPhotoURL = lc.PackDescEngPhotoURL,
                    // PackDescEngPhoto = lc.PackDescEngPhoto,
                }

            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .ToList();
            return ordersLabel.AsQueryable();
        }

    }
}