using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackArticleItemService : BusinessService
    {
        private ERP.Services.Entities.PackMappingService PackMapping { get; set; }

        private ERP.Services.Entities.PackMappingItem1Service PackMappingItem1 { get; set; }
        private ERP.Services.Entities.PackMappingItem2Service PackMappingItem2 { get; set; }
        private ERP.Services.Business.Entities.PackSpecService PackSpec { get; set; }

        public PackArticleItemService(
            ERP.Services.Entities.PackMappingItem1Service packMappingItem1Service,
            ERP.Services.Entities.PackMappingItem2Service packMappingItem2Service,
            ERP.Services.Business.Entities.PackSpecService packSpecService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackMappingItem1 = packMappingItem1Service;
            PackMappingItem2 = packMappingItem2Service;
            PackSpec = packSpecService;
        }
        public IEnumerable<Models.Views.PackArticleItem> Get(int packMappingId, int localeId)
        {
            var p1 = PackMappingItem1.Get().Where(i => i.PackMappingId == packMappingId && i.LocaleId == localeId).ToList();
            var p2 = PackMappingItem2.Get().Where(i => i.PackMappingId == packMappingId && i.LocaleId == localeId).ToList();
            var ps = PackSpec.Get().Where(i => i.LocaleId == localeId).ToList();

            var packArticleItems = (
                from i1 in p1
                from i2 in p2.Where(i => i.Type == 1)
                from i3 in p2.Where(i => i.Type == 2)
                where (i1.PackMappingId == i2.PackMappingId && i1.LocaleId == i2.LocaleId) &&
                    (i1.PackMappingId == i3.PackMappingId && i1.LocaleId == i3.LocaleId) &&
                    (i1.ArticleInnerSize >= i2.BeginArticleInnerSize && i1.ArticleInnerSize <= i2.EndArticleInnerSize) &&
                    (i1.ArticleInnerSize >= i3.BeginArticleInnerSize && i1.ArticleInnerSize <= i3.EndArticleInnerSize)
                    select new Models.Views.PackArticleItem
                    {
                        PackMappingId = i1.PackMappingId,
                        LocaleId = i1.LocaleId,
                        PackMappingItem1Id = i1.Id,
                        PackMappingItem2Id = i2.Id,
                        ArticleSize = i1.ArticleSize,
                        ArticleSizeSuffix = i1.ArticleSizeSuffix,
                        ArticleInnerSize = i1.ArticleInnerSize,
                        PairsOfCTN = i1.PairsOfCTN,
                        GWOfCTN = i1.GWOfCTN,
                        NWOfCTN = i1.NWOfCTN,
                        MEAS = i1.MEAS,
                        GWOfCTNCLB = i1.GWOfCTNCLB,
                        MEASCLB = i1.MEASCLB,
                        CTNSpec = i2.Spec,
                        CTNSpecCLB = i2.SpecCLB,
                        // CTNL = i2.CTNL,
                        // CTNW = i2.CTNW,
                        // CTNH = i2.CTNH,
                        CTNL = i2.CTNL != null ? i2.CTNL : ps.Where(i => i.Spec == i2.Spec && i.Type == 1).FirstOrDefault() != null ? ps.Where(i => i.Spec == i2.Spec && i.Type == 1).FirstOrDefault().L:"",
                        CTNW = i2.CTNW != null ? i2.CTNW : ps.Where(i => i.Spec == i2.Spec && i.Type == 1).FirstOrDefault() != null ? ps.Where(i => i.Spec == i2.Spec && i.Type == 1).FirstOrDefault().W:"",
                        CTNH = i2.CTNH != null ? i2.CTNH : ps.Where(i => i.Spec == i2.Spec && i.Type == 1).FirstOrDefault() != null ? ps.Where(i => i.Spec == i2.Spec && i.Type == 1).FirstOrDefault().H:"",
                        BOXSpec = i3.Spec,
                        BOXSpecCLB = i3.SpecCLB,
                        // BOXH = i3.CTNH,
                        // BOXW = i3.CTNW,
                        // BOXL = i3.CTNL
                        BOXL = i3.CTNL != null ? i3.CTNL : ps.Where(i => i.Spec == i3.Spec && i.Type == 2).FirstOrDefault() != null ? ps.Where(i => i.Spec == i3.Spec && i.Type == 2).FirstOrDefault().L:"",
                        BOXH = i3.CTNH != null ? i3.CTNH : ps.Where(i => i.Spec == i3.Spec && i.Type == 2).FirstOrDefault() != null ? ps.Where(i => i.Spec == i3.Spec && i.Type == 2).FirstOrDefault().W:"",
                        BOXW = i3.CTNW != null ? i3.CTNW : ps.Where(i => i.Spec == i3.Spec && i.Type == 2).FirstOrDefault() != null ? ps.Where(i => i.Spec == i3.Spec && i.Type == 2).FirstOrDefault().H:"",
                    }
            );
            return packArticleItems;
        }
        public void CreateRange(IEnumerable<Models.Views.PackArticleItem> packArticleItems)
        {
            PackMappingItem1.CreateRange(Item1BuildRange(packArticleItems));
            PackMappingItem2.CreateRange(Item2BuildRange(packArticleItems));
        }
        public void RemoveRange(int packMappingId, int localeId)
        {
            PackMappingItem1.RemoveRange(i => i.PackMappingId == packMappingId && i.LocaleId == localeId);
            PackMappingItem2.RemoveRange(i => i.PackMappingId == packMappingId && i.LocaleId == localeId);
        }

        public IEnumerable<Models.Entities.PackMappingItem1> Item1BuildRange(IEnumerable<Models.Views.PackArticleItem> packArticleItems)
        {
            return packArticleItems.Select(i => new Models.Entities.PackMappingItem1
            {
                // Id = i.PackMappingItem1Id,
                LocaleId = i.LocaleId,
                PackMappingId = i.PackMappingId,
                ArticleSize = i.ArticleSize,
                ArticleSizeSuffix = i.ArticleSizeSuffix,
                ArticleInnerSize = i.ArticleInnerSize,
                GWOfCTN = i.GWOfCTN,
                NWOfCTN = i.NWOfCTN,
                MEAS = i.MEAS,
                GWOfCTNCLB = i.GWOfCTNCLB,
                MEASCLB = i.MEASCLB,
                PairsOfCTN = i.PairsOfCTN
            });
        }
        public IEnumerable<Models.Entities.PackMappingItem2> Item2BuildRange(IEnumerable<Models.Views.PackArticleItem> packArticleItems)
        {
            var item2 = new List<Models.Entities.PackMappingItem2>() { };

            var cnts = packArticleItems.Select(i => i.CTNSpec).Distinct();
            var boxes = packArticleItems.Select(i => i.BOXSpec).Distinct();

            //carton
            foreach (var cnt in cnts)
            {
                var items = packArticleItems.Where(i => i.CTNSpec == cnt).ToList();
                var beginItem = items.Where(i => i.ArticleInnerSize == items.Min(i2 => i2.ArticleInnerSize)).FirstOrDefault();
                var endItem = items.Where(i => i.ArticleInnerSize == items.Max(i2 => i2.ArticleInnerSize)).FirstOrDefault();

                item2.Add(new Models.Entities.PackMappingItem2()
                {
                    LocaleId = beginItem.LocaleId,
                    PackMappingId = beginItem.PackMappingId,
                    Type = 1,
                    BeginArticleSize = beginItem.ArticleSize,
                    BeginArticleSizeSuffix = beginItem.ArticleSizeSuffix,
                    BeginArticleInnerSize = beginItem.ArticleInnerSize,
                    EndArticleSize = endItem.ArticleSize,
                    EndArticleSizeSuffix = endItem.ArticleSizeSuffix,
                    EndArticleInnerSize = endItem.ArticleInnerSize,
                    Spec = cnt,
                    CTNL = beginItem.CTNL,
                    CTNW = beginItem.CTNW,
                    CTNH = beginItem.CTNH,
                });
            }
            //box
            foreach (var box in boxes)
            {
                var items = packArticleItems.Where(i => i.BOXSpec == box).ToList();
                var beginItem = items.Where(i => i.ArticleInnerSize == items.Min(i2 => i2.ArticleInnerSize)).FirstOrDefault();
                var endItem = items.Where(i => i.ArticleInnerSize == items.Max(i2 => i2.ArticleInnerSize)).FirstOrDefault();

                item2.Add(new Models.Entities.PackMappingItem2()
                {
                    LocaleId = beginItem.LocaleId,
                    PackMappingId = beginItem.PackMappingId,
                    Type = 2,
                    BeginArticleSize = beginItem.ArticleSize,
                    BeginArticleSizeSuffix = beginItem.ArticleSizeSuffix,
                    BeginArticleInnerSize = beginItem.ArticleInnerSize,
                    EndArticleSize = endItem.ArticleSize,
                    EndArticleSizeSuffix = endItem.ArticleSizeSuffix,
                    EndArticleInnerSize = endItem.ArticleInnerSize,
                    Spec = box,
                    CTNL = beginItem.BOXL,
                    CTNW = beginItem.BOXW,
                    CTNH = beginItem.BOXH,
                });
            }
            return item2;
        }
    }
}