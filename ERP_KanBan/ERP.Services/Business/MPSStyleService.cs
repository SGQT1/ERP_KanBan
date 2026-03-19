using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Models.Views.Report;
using ERP.Services.Bases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class MPSStyleService : BusinessService
    {
        private ERP.Services.Business.Entities.MPSArticleService MPSArticle { get; set; }
        private ERP.Services.Business.Entities.MPSStyleService MPSStyle { get; set; }
        private ERP.Services.Business.Entities.MPSStyleItemService MPSStyleItem { get; set; }
        private ERP.Services.Business.Entities.MPSStyleItemUsageService MPSStyleItemUsage { get; set; }
        private ERP.Services.Business.Entities.StyleItemService StyleItem { get; set; }
        private ERP.Services.Business.Entities.ArticleSizeRunUsageService ArticleSizeRunUsage { get; set; }
        private ERP.Services.Business.Entities.StyleSizeRunUsageService StyleSizeRunUsage { get; set; }

        private ERP.Services.Entities.MRPItemService MRPItem { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Entities.MpsOrdersService MPSOrders { get; set; }

        public MPSStyleService(
            ERP.Services.Business.Entities.MPSArticleService mpsArticleService,
            ERP.Services.Business.Entities.MPSStyleService mpsStyleService,
            ERP.Services.Business.Entities.MPSStyleItemService mpsStyleItemService,
            ERP.Services.Business.Entities.MPSStyleItemUsageService mpsStyleItemUsageService,
            ERP.Services.Business.Entities.StyleItemService styleItemService,
            ERP.Services.Business.Entities.ArticleSizeRunUsageService articleSizeRunUsageService,
            ERP.Services.Business.Entities.StyleSizeRunUsageService styleSizeRunUsageService,
            ERP.Services.Entities.OrdersService ordersService,
            ERP.Services.Entities.MRPItemService mrpItemService,
            ERP.Services.Entities.MpsOrdersService mpsOrdersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            MPSArticle = mpsArticleService;
            MPSStyle = mpsStyleService;
            MPSStyleItem = mpsStyleItemService;
            MPSStyleItemUsage = mpsStyleItemUsageService;
            StyleItem = styleItemService;
            ArticleSizeRunUsage = articleSizeRunUsageService;
            StyleSizeRunUsage = styleSizeRunUsageService;

            Orders = ordersService;
            MRPItem = mrpItemService;
            MPSOrders = mpsOrdersService;
        }

        public ERP.Models.Views.MPSStyleGroup Get(int id, int localeId)
        {
            var group = new MPSStyleGroup();

            var mpsStyle = MPSStyle.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (mpsStyle != null)
            {
                // get Dispatch Qty(Size Run)
                var mpsStyleItems = MPSStyleItem.Get().Where(i => i.MpsStyleId == mpsStyle.Id && i.LocaleId == mpsStyle.LocaleId).ToList();
                var ids = mpsStyleItems.Select(i => i.Id).ToArray();
                var mpsStyleItemUsages = MPSStyleItemUsage.Get().Where(i => i.LocaleId == mpsStyle.LocaleId && ids.Contains(i.MpsStyleItemId)).ToList();


                // Step3.3 生成MPSStyleItemUsage
                // 取訂單專用Usage
                var articleParts = mpsStyleItems.Select(i => i.PartNameTw).ToArray();
                var articleOnlyUsages = ArticleSizeRunUsage.GetWithStyle().Where(i => articleParts.Contains(i.PartNameTw) && i.ArticleId == mpsStyle.ArticleId && i.StyleId == mpsStyle.StyleId && i.LocaleId == localeId)
                    .Select(i => new { i.ArticleSize, i.ArticleSizeSuffix, i.ArticleInnerSize, i.UnitUsage, i.LocaleId, i.StyleItemId, i.PartNameTw }).Distinct().ToList();
                // 取型體專用Usage
                var styleOnlyUsages = StyleSizeRunUsage.Get().Where(i => i.LocaleId == localeId && i.StyleId == mpsStyle.StyleId && i.OrdersId == 0).ToList();

                var articleOnlyItemUsages = articleOnlyUsages.Select(i => new Models.Views.MPSStyleItemUsage
                {
                    Id = 0,
                    MpsStyleItemId = mpsStyleItems.Where(m => m.PartNameTw == i.PartNameTw).Max(i => i.Id),
                    ArticleSize = i.ArticleSize,
                    ArticleSizeSuffix = i.ArticleSizeSuffix,
                    ArticleInnerSize = i.ArticleInnerSize,
                    PreUnitUsage = i.UnitUsage,
                    UnitUsage = i.UnitUsage,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    LocaleId = i.LocaleId,
                    MpsStyleId = 0,
                    // StyleItemId = i.StyleItemId,
                    StyleItemId = mpsStyleItems.Where(m => m.PartNameTw == i.PartNameTw).Max(i => i.Id),
                }).ToList();
                var styelOnlyItemUsages = (
                    from su in styleOnlyUsages
                    join si in mpsStyleItems on new { MaterialName = su.MaterialName, LocaleId = su.LocaleId } equals new { MaterialName = si.MaterialNameTw, LocaleId = si.LocaleId }
                    select new Models.Views.MPSStyleItemUsage
                    {
                        Id = 0,
                        MpsStyleItemId = mpsStyleItems.Where(m => m.StyleItemId == si.StyleItemId).Max(si => si.Id),
                        ArticleSize = su.ArticleSize ?? 0,
                        ArticleSizeSuffix = su.ArticleSizeSuffix,
                        ArticleInnerSize = su.ArticleInnerSize ?? 0,
                        PreUnitUsage = su.UnitUsage,
                        UnitUsage = su.UnitUsage,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        LocaleId = su.LocaleId,
                        MpsStyleId = 0,
                        // StyleItemId = si.StyleItemId,
                        StyleItemId = mpsStyleItems.Where(m => m.StyleItemId == si.StyleItemId).Max(si => si.Id),
                    })
                .ToList();

                // 合併
                var allUsages = articleOnlyItemUsages.Union(styelOnlyItemUsages);
                var newStyleItemUsages = allUsages.ToList();
                // Step4 補上是否有用量的檢查
                // Step4.1 取預設的size(有些沒有用量的部位使用)
                var defautSizeUsageItems = mpsStyleItemUsages.Select(i => new
                {
                    ArticleSize = i.ArticleSize,
                    ArticleSizeSuffix = i.ArticleSizeSuffix,
                    ArticleInnerSize = i.ArticleInnerSize,
                })
                .Distinct()
                .ToList();

                mpsStyleItems.ForEach(i =>
                {
                    i.HasUsage = mpsStyleItemUsages.Where(u => u.MpsStyleItemId == i.Id).Any() ? 1 : 0;

                    var partUsage = newStyleItemUsages.Where(u => u.MpsStyleItemId == i.Id).Any() ? 1 : 0;
                    if (partUsage == 0)
                    {
                        //把沒有用量的部位加入0的預設用量;
                        var _styleItemUsages = defautSizeUsageItems.Select(d => new Models.Views.MPSStyleItemUsage
                        {
                            Id = 0,
                            MpsStyleItemId = i.Id,
                            ArticleSize = d.ArticleSize,
                            ArticleSizeSuffix = d.ArticleSizeSuffix,
                            ArticleInnerSize = d.ArticleInnerSize,
                            PreUnitUsage = 0,
                            UnitUsage = 0,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            LocaleId = i.LocaleId,
                            MpsStyleId = i.MpsStyleId,
                            StyleItemId = i.StyleItemId,
                        });
                        newStyleItemUsages.AddRange(_styleItemUsages);
                    }
                });

                group.MPSStyle = mpsStyle;
                group.MPSStyleItem = mpsStyleItems.OrderBy(i => i.PartNo).ToList();
                group.MPSStyleItemUsage = mpsStyleItemUsages.OrderBy(i => i.MpsStyleItemId).ToList();
                group.NewStyleItemUsage = newStyleItemUsages.OrderBy(i => i.MpsStyleItemId).ToList();
            }
            return group;
        }

        public IQueryable<Models.Views.Orders> GetOrdersStyleForImport()
        {
            var mrpOrdersIds = MRPItem.Get().Select(i => new { i.OrdersId, i.LocaleId }).Distinct();
            var msStyleNos = MPSOrders.Get().Select(i => new { i.StyleNo, i.LocaleId }).Distinct();
            var result = (
                from o in Orders.Get().Where(i => i.Status != 3 && i.OrderQty > 0)
                join a in MPSArticle.Get() on new { ArticeNo = o.ArticleNo, LocaleId = o.LocaleId } equals new { ArticeNo = a.ArticleNo, LocaleId = a.LocaleId }
                // join mo in msStyleNos on new { StyleNo = o.StyleNo, LocaleId = o.LocaleId } equals new { StyleNo = mo.StyleNo, LocaleId = mo.LocaleId }
                // join mi in mrpOrdersIds on new { OrdersId = o.Id, LocaleId = o.LocaleId } equals new { OrdersId = mi.OrdersId, LocaleId = mi.LocaleId }
                select new Models.Views.Orders
                {
                    Id = o.Id,
                    LocaleId = o.LocaleId,
                    ArticleNo = a.ArticleNo,
                    ArticleId = a.ArticleId,
                    StyleNo = o.StyleNo,
                    StyleId = o.StyleId,
                    OrderNo = o.OrderNo,
                    Brand = o.Brand
                }
            );

            return result;
        }

        public ERP.Models.Views.MPSStyleGroup BuildMPSStyleGroup(int ordersId, int localeId)
        {
            var group = new MPSStyleGroup();

            try
            {
                var orders = (
                    from o in Orders.Get().Where(i => i.Id == ordersId && i.LocaleId == localeId)
                    join a in MPSArticle.Get() on new { ArticeNo = o.ArticleNo, LocaleId = o.LocaleId } equals new { ArticeNo = a.ArticleNo, LocaleId = a.LocaleId }
                    select new
                    {
                        Id = o.Id,
                        LocaleId = o.LocaleId,
                        OrderNo = o.OrderNo,
                        StyleNo = o.StyleNo,
                        StyleId = o.StyleId,
                        ArticleId = o.ArticleId,
                        MPSArticleId = a.Id,
                        MPSArticleNo = a.ArticleNo,
                        SizeCountryCodeId = o.SizeCountryCodeId,
                    }
                ).FirstOrDefault();

                if (orders != null)
                {
                    // Step1 取有做過派工用量的型體
                    var existMPSStyle = MPSStyle.Get().Where(i => i.StyleNo == orders.StyleNo && i.LocaleId == orders.LocaleId).FirstOrDefault();
                    var existMPSStyleItems = MPSStyleItem.Get().Where(i => i.StyleNo == orders.StyleNo && i.LocaleId == orders.LocaleId).ToList();

                    // Step2 取管制表及型體
                    var mrpItems = MRPItem.Get().Where(i => i.OrdersId == ordersId && i.LocaleId == localeId && i.PartId > -2 && i.ParentMaterialId == 0 && i.Total >= 0).ToList();
                    var styleItems = StyleItem.Get().Where(i => i.StyleId == orders.StyleId && i.LocaleId == localeId).ToList();

                    // Step3 生成MPSStyleGroup
                    // Step3.1 生成MPSStyle
                    var mpsStyle = new MPSStyle
                    {
                        Id = existMPSStyle != null ? existMPSStyle.Id : 0,
                        MpsArticleId = existMPSStyle != null ? existMPSStyle.MpsArticleId : orders.MPSArticleId,
                        StyleNo = existMPSStyle != null ? existMPSStyle.StyleNo : orders.StyleNo,
                        LocaleId = existMPSStyle != null ? existMPSStyle.LocaleId : orders.LocaleId,
                        SizeCountryCodeId = existMPSStyle != null ? existMPSStyle.SizeCountryCodeId : (decimal)orders.SizeCountryCodeId,
                        ArticleNo = existMPSStyle != null ? existMPSStyle.ArticleNo : orders.MPSArticleNo,

                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        RefOrderNo = orders.OrderNo,
                        DoUsage = 0,
                        UnitRelaxTime = 0,
                        UnitStandardTime = 0,
                        UnitLaborCost = 0,
                        HasProcedure = 0,
                    };
                    // Step3.2 生成MPSStyleItems
                    var mpsStyleItems = mrpItems.Select(i => new Models.Views.MPSStyleItem
                    {
                        Id = 0, // 用MRPItemId當做MpsStyleItemId
                        MpsStyleId = 0,
                        PartNo = i.PartNo,
                        PartNameTw = i.PartNameTw,
                        PartNameEn = i.PartNameEn,
                        MaterialNameTw = i.MaterialNameTw,
                        MaterialNameEn = i.MaterialNameEn,
                        UnitNameTw = i.UnitNameTw,
                        UnitNameEn = i.UnitNameEn,
                        // RefKnifeNo = styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && s.MaterialNameTw == i.MaterialNameTw) != null ? styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && s.MaterialNameTw == i.MaterialNameTw).Max(s => s.KnifeNo): "",
                        // PieceOfPair = styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && s.MaterialNameTw == i.MaterialNameTw) != null ? styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && s.MaterialNameTw == i.MaterialNameTw).Max(s => (int)s.PieceOfPair) : 0,
                        // StyleItemId = styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && s.MaterialNameTw == i.MaterialNameTw) != null ? styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && s.MaterialNameTw == i.MaterialNameTw).Max(s => s.Id) : 0,
                        AlternateType = i.SizeDivision,
                        UsageGiveBegin = 0,
                        UsageGiveEnd = 0,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        LocaleId = localeId,
                        Type = 1,
                        // TotalUsage = i.Total,
                        PartId = i.PartId,
                        StyleItemId = i.Id, // 用來識別Usage的Id,
                    })
                    .OrderBy(i => i.PartNo)
                    .ToList();
                    mpsStyleItems.ForEach(i =>
                    {
                        // 如果有資料，Id, LocaleId, MpsStyleId 都要用舊的
                        var mpsStyleItem = i.PartId > 0 ? existMPSStyleItems.Where(msi => msi.PartNameTw == i.PartNameTw && msi.LocaleId == i.LocaleId).FirstOrDefault() :
                                                          existMPSStyleItems.Where(msi => msi.PartNameTw == i.PartNameTw && msi.MaterialNameTw == i.MaterialNameTw && msi.LocaleId == i.LocaleId).FirstOrDefault();
                        if (mpsStyleItem != null)
                        {
                            i.Id = mpsStyleItem.Id;
                            i.MpsStyleId = mpsStyleItem.MpsStyleId;
                            i.LocaleId = mpsStyleItem.LocaleId;
                        }

                        // 更新斬刀、雙數等，有比對的到StyleItems的就用新的
                        var item = styleItems.Where(s => s.LocaleId == i.LocaleId && s.PartNameTw == i.PartNameTw && i.PartId > 0).FirstOrDefault();
                        if (item != null)
                        {
                            i.RefKnifeNo = item.KnifeNo;
                            i.PieceOfPair = item.PieceOfPair ?? 0;
                            i.StyleItemId = item.Id;
                        }
                        else
                        {
                            i.RefKnifeNo = "";
                            i.PieceOfPair = 0;
                        }
                    });

                    // Step3.3 生成MPSStyleItemUsage
                    // 取訂單專用Usage
                    var articleOnlyIds = mpsStyleItems.Where(i => i.PartId > 0).Select(i => i.StyleItemId).ToArray();
                    var articleOnlyUsages = ArticleSizeRunUsage.GetWithStyle().Where(i => i.ArticleId == orders.ArticleId && i.LocaleId == localeId && articleOnlyIds.Contains(i.StyleItemId)).ToList();
                    // 取型體專用Usage
                    var styleOnlyUsages = StyleSizeRunUsage.Get().Where(i => i.LocaleId == localeId && i.StyleId == orders.StyleId && i.OrdersId == 0).ToList();

                    var articleOnlyItemUsages = articleOnlyUsages.Select(i => new Models.Views.MPSStyleItemUsage
                    {
                        Id = 0,
                        MpsStyleItemId = mpsStyleItems.Where(m => m.StyleItemId == i.StyleItemId).Max(i => i.Id),
                        ArticleSize = i.ArticleSize,
                        ArticleSizeSuffix = i.ArticleSizeSuffix,
                        ArticleInnerSize = i.ArticleInnerSize,
                        PreUnitUsage = i.UnitUsage,
                        UnitUsage = i.UnitUsage,
                        ModifyUserName = "",
                        LastUpdateTime = DateTime.Now,
                        LocaleId = i.LocaleId,
                        MpsStyleId = 0,
                        StyleItemId = i.StyleItemId,
                    }).ToList();

                    var styelOnlyItemUsages = (
                        from su in styleOnlyUsages
                        join si in mpsStyleItems on new { MaterialName = su.MaterialName, LocaleId = su.LocaleId } equals new { MaterialName = si.MaterialNameTw, LocaleId = si.LocaleId }
                        select new Models.Views.MPSStyleItemUsage
                        {
                            Id = 0,
                            MpsStyleItemId = mpsStyleItems.Where(m => m.StyleItemId == si.StyleItemId).Max(si => si.Id),
                            ArticleSize = su.ArticleSize ?? 0,
                            ArticleSizeSuffix = su.ArticleSizeSuffix,
                            ArticleInnerSize = su.ArticleInnerSize ?? 0,
                            PreUnitUsage = su.UnitUsage,
                            UnitUsage = su.UnitUsage,
                            ModifyUserName = "",
                            LastUpdateTime = DateTime.Now,
                            LocaleId = su.LocaleId,
                            MpsStyleId = 0,
                            StyleItemId = si.StyleItemId,
                        })
                    .ToList();

                    // 合併
                    var mpsStyleItemUsages = articleOnlyItemUsages.Union(styelOnlyItemUsages);
                    var newStyleItemUsages = mpsStyleItemUsages.ToList();
                    // Step4 補上是否有用量的檢查
                    // Step4.1 取預設的size(有些沒有用量的部位使用)
                    var defautSizeUsageItems = mpsStyleItemUsages.Select(i => new
                    {
                        ArticleSize = i.ArticleSize,
                        ArticleSizeSuffix = i.ArticleSizeSuffix,
                        ArticleInnerSize = i.ArticleInnerSize,
                    })
                    .Distinct()
                    .ToList();

                    mpsStyleItems.ForEach(i =>
                    {
                        if (i.Id > 0)
                        {
                            i.HasUsage = mpsStyleItemUsages.Where(u => u.MpsStyleItemId == i.Id).Any() ? 1 : 0;
                        }
                        else
                        {
                            i.HasUsage = mpsStyleItemUsages.Where(u => u.StyleItemId == i.StyleItemId).Any() ? 1 : 0;
                        }

                        if (i.HasUsage == 0)
                        {
                            //把沒有用量的部位加入0的預設用量;
                            var _styleItemUsages = defautSizeUsageItems.Select(d => new Models.Views.MPSStyleItemUsage
                            {
                                Id = 0,
                                MpsStyleItemId = i.Id,
                                ArticleSize = d.ArticleSize,
                                ArticleSizeSuffix = d.ArticleSizeSuffix,
                                ArticleInnerSize = d.ArticleInnerSize,
                                PreUnitUsage = 0,
                                UnitUsage = 0,
                                ModifyUserName = "",
                                LastUpdateTime = DateTime.Now,
                                LocaleId = i.LocaleId,
                                MpsStyleId = i.MpsStyleId,
                                StyleItemId = i.StyleItemId,
                            });
                            newStyleItemUsages.AddRange(_styleItemUsages);
                        }

                    });

                    group.MPSStyle = mpsStyle;
                    group.MPSStyleItem = mpsStyleItems;
                    group.MPSStyleItemUsage = mpsStyleItemUsages;
                    group.NewStyleItemUsage = newStyleItemUsages;
                }

            }
            catch (Exception e)
            {

            }

            return group;
        }
        public ERP.Models.Views.MPSStyleGroup SaveMPSStyleGroup(MPSStyleGroup group)
        {
            var mpsStyle = group.MPSStyle;
            var mpsStyleItem = group.MPSStyleItem.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (mpsStyle != null)
                {
                    //Plan
                    {
                        var _mpsStyle = MPSStyle.Get().Where(i => i.LocaleId == mpsStyle.LocaleId && i.Id == mpsStyle.Id).FirstOrDefault();
                        if (_mpsStyle != null)
                        {
                            mpsStyle.Id = _mpsStyle.Id;
                            mpsStyle.LocaleId = _mpsStyle.LocaleId;

                            mpsStyle = MPSStyle.Update(mpsStyle);
                        }
                        else
                        {
                            mpsStyle = MPSStyle.Create(mpsStyle);
                        }
                    }
                    //items
                    {
                        if (mpsStyle.Id != 0)
                        {
                            var itemIds = MPSStyleItem.Get().Where(i => i.MpsStyleId == mpsStyle.Id && i.LocaleId == mpsStyle.LocaleId).Select(i => i.Id).ToArray();
                            MPSStyleItem.RemoveRange(i => i.MpsStyleId == mpsStyle.Id && i.LocaleId == mpsStyle.LocaleId);
                            MPSStyleItemUsage.RemoveRange(i => itemIds.Contains(i.MpsStyleItemId) && i.LocaleId == mpsStyle.LocaleId);

                            // MPSStyleItem.CreateRange(mpsStyleItem);
                            mpsStyleItem.ForEach(m =>
                            {
                                m.MpsStyleId = mpsStyle.Id;
                                m.LocaleId = mpsStyle.LocaleId;
                                m.ModifyUserName = mpsStyle.ModifyUserName;
                                m.LastUpdateTime = mpsStyle.LastUpdateTime;

                                var _mpsStyleItem = MPSStyleItem.Create(m);
                                var mpsStyleItemUsage = group.MPSStyleItemUsage
                                .Where(i => i.StyleItemId == m.StyleItemId && i.LocaleId == i.LocaleId)
                                .Select(i => new Models.Views.MPSStyleItemUsage
                                {
                                    Id = i.Id,
                                    MpsStyleItemId = _mpsStyleItem.Id,
                                    ArticleSize = i.ArticleSize,
                                    ArticleSizeSuffix = i.ArticleSizeSuffix,
                                    ArticleInnerSize = i.ArticleInnerSize,
                                    PreUnitUsage = i.PreUnitUsage,
                                    UnitUsage = i.PreUnitUsage,
                                    ModifyUserName = mpsStyle.ModifyUserName,
                                    LastUpdateTime = mpsStyle.LastUpdateTime,
                                    LocaleId = mpsStyle.LocaleId,
                                    MpsStyleId = mpsStyle.Id,
                                    StyleItemId = i.StyleItemId,
                                });

                                MPSStyleItemUsage.CreateRange(mpsStyleItemUsage);
                            });

                        }
                    }
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)mpsStyle.Id, (int)mpsStyle.LocaleId);
        }
        public void RemoveMPSStyleGroup(MPSStyleGroup group)
        {
            var mpsStyle = group.MPSStyle;
            var mpsStyleItemIds = group.MPSStyleItem.Select(i => i.Id);
            try
            {
                UnitOfWork.BeginTransaction();
                if (mpsStyle != null)
                {
                    MPSStyleItemUsage.RemoveRange(i => mpsStyleItemIds.Contains(i.MpsStyleItemId) && i.LocaleId == mpsStyle.LocaleId);
                    MPSStyleItem.RemoveRange(i => i.MpsStyleId == mpsStyle.Id && i.LocaleId == mpsStyle.LocaleId);
                    MPSStyle.Remove(mpsStyle);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public ERP.Models.Views.MPSStyleGroup SaveMPSStyleItemGroup(MPSStyleItemGroup group)
        {
            var mpsStyle = group.MPSStyle;
            var mpsStyleItem = group.MPSStyleItem;
            try
            {
                UnitOfWork.BeginTransaction();
                if (mpsStyle != null)
                {
                    //Plan
                    {
                        var _mpsStyle = MPSStyle.Get().Where(i => i.LocaleId == mpsStyle.LocaleId && i.Id == mpsStyle.Id).FirstOrDefault();
                        if (_mpsStyle != null)
                        {
                            mpsStyle.Id = _mpsStyle.Id;
                            mpsStyle.LocaleId = _mpsStyle.LocaleId;

                            mpsStyle = MPSStyle.Update(mpsStyle);
                        }
                        else
                        {
                            mpsStyle = MPSStyle.Create(mpsStyle);
                        }
                    }
                    //items
                    {
                        if (mpsStyle.Id != 0)
                        {
                            MPSStyleItem.RemoveRange(i => i.Id == mpsStyleItem.Id && i.LocaleId == mpsStyle.LocaleId);
                            MPSStyleItemUsage.RemoveRange(i => i.MpsStyleItemId == mpsStyleItem.Id && i.LocaleId == mpsStyle.LocaleId);


                            mpsStyleItem.MpsStyleId = mpsStyle.Id;
                            mpsStyleItem.LocaleId = mpsStyle.LocaleId;

                            var _mpsStyleItem = MPSStyleItem.Create(mpsStyleItem);

                            var mpsStyleItemUsage = group.MPSStyleItemUsage
                                .Select(i => new Models.Views.MPSStyleItemUsage
                                {
                                    Id = i.Id,
                                    MpsStyleItemId = _mpsStyleItem.Id,
                                    ArticleSize = i.ArticleSize,
                                    ArticleSizeSuffix = i.ArticleSizeSuffix,
                                    ArticleInnerSize = i.ArticleInnerSize,
                                    PreUnitUsage = i.PreUnitUsage,
                                    UnitUsage = i.PreUnitUsage,
                                    ModifyUserName = mpsStyle.ModifyUserName,
                                    LastUpdateTime = mpsStyle.LastUpdateTime,
                                    LocaleId = mpsStyle.LocaleId,
                                    MpsStyleId = mpsStyle.Id,
                                    StyleItemId = i.StyleItemId,
                                });

                            MPSStyleItemUsage.CreateRange(mpsStyleItemUsage);

                        }
                    }
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)mpsStyle.Id, (int)mpsStyle.LocaleId);
        }
        public MPSStyleGroup RemoveMPSStyleItemGroup(MPSStyleItemGroup group)
        {
            var mpsStyle = group.MPSStyle;
            var mpsStyleItem = group.MPSStyleItem;
            try
            {
                UnitOfWork.BeginTransaction();
                if (mpsStyle != null)
                {
                    MPSStyleItemUsage.RemoveRange(i => i.MpsStyleItemId == mpsStyleItem.Id && i.LocaleId == mpsStyleItem.LocaleId);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            return Get((int)mpsStyle.Id, (int)mpsStyle.LocaleId);
        }

    }
}
