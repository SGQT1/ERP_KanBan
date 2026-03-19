using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PackPlanItemService : BusinessService
    {
        private ERP.Services.Business.Entities.OrdersService Orders { get; set; }
        private ERP.Services.Business.Entities.OrdersItemService OrdersItem { get; set; }

        private ERP.Services.Entities.OrdersPLService OrdersPL { get; set; }
        private ERP.Services.Entities.OrdersPackService OrdersPack { get; set; }
        private ERP.Services.Entities.OrdersPLForUSAService OrdersPLForUSA { get; set; }
        private ERP.Services.Entities.OrdersPLSpecService OrdersPLSpec { get; set; }
        private ERP.Services.Entities.OrdersPLPhotoService OrdersPLPhoto { get; set; }
        public PackPlanItemService(
            ERP.Services.Business.Entities.OrdersService ordersService,
            ERP.Services.Business.Entities.OrdersItemService ordersItemService,

            ERP.Services.Entities.OrdersPLService ordersPLService,
            ERP.Services.Entities.OrdersPackService ordersPackService,
            ERP.Services.Entities.OrdersPLForUSAService ordersPLForUSAService,
            ERP.Services.Entities.OrdersPLSpecService ordersPLSpecService,
            ERP.Services.Entities.OrdersPLPhotoService ordersPLPhotoService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersItem = ordersItemService;

            OrdersPL = ordersPLService;
            OrdersPack = ordersPackService;
            OrdersPLForUSA = ordersPLForUSAService;
            OrdersPLSpec = ordersPLSpecService;
            OrdersPLPhoto = ordersPLPhotoService;
        }
        /*
         * For Get:
         * 把 OrdersPack,OrdersPL,OrdersPLForUSA,OrdersPLSpec,OrdersPLForPhoto整合在PackPlanItem裡
         * 1.取出OrdersPL,OrdersPack(LocaleId,RefLocaleId,RefOrdersId,Edition)
         * 2.如果OrdersPL,OrdersPack有值，取 Orders,OrdersItem,SizeCountry，產生需要的 ArticleSize,ShoeSize,ContrySize
         * 3.產生PackPlanItem
         * 4.OrdersItem.GetSizeMapping >> 取得訂單Size對應的國家別size號碼
         * step1: 組合PackPlanItem的基本資料：GroupBy,ArticleSize,OrderSize ,ItemInnerSize,RefDisplaySize,PairOfCTN ,CTNS, NWOfCTN,GWOfCTN,MEASCBM,AdjQty
         * step4: 根據單裝或混裝，來取得的MinCNo,MaxCNo,CTNSpec,CTNL,CTNW,CTNH,BOXSpec
         *  單裝：MinCNo,MaxCNo >>把每個size的最大值加上該size的箱數，
         *       CTNSpec、BOXSpec >> 用RefDisplaySizeBegin/RefDisplaySizeEnd來找出該size的規格，RefDisplaySizeBegin/RefDisplaySizeEnd 等於該size
         *       CTNL、CTNW、CTNH >> 用MinCNo,MaxCNo來找對應的 OrdersPLForUSA的L、W、H
         *  混裝：用Group來找出各箱子中的 MinCNo,MaxCNo，
         *       CTNSpec、BOXSpec >> 一樣用各箱中的RefDisplaySizeBegin/RefDisplaySizeEnd 來找出對應的PLSpec,PLForUSA,差別在於RefDisplaySizeBegin是sting,需要用InnerSize找出最小或最大的size,
         *       CTNL、CTNW、CTNH >> 用MinCNo,MaxCNo來找對應的 OrdersPLForUSA的L、W、H
         * For Save:
         * OrdersPLForSpec
         * 1.單裝，從OrdersPack取Min(MinItemInnerSize), Max(MaxItemInnerSize), MEAS, sum(TTLCTNS), sum(TTLPrs), RTrim(LTrim(Edition)), group by MEAS,Edition
         * 2.混裝，從OrdersPack取Min(A.MinItemInnerSize), Max(A.MaxItemInnerSize),A.MEAS,A.CTNS, A.TTLPrs, RTrim(LTrim(A.Edition)),group ctns,meas,ttlprs,edition.groupby,refordersid,reflocaleid,localeid
         */
         public IEnumerable<Models.Views.PackPlanItem> Get(string ordersNo, int localeId, string edition)
        {
            // var order = Orders.Get().Where(i => i.OrderNo == ordersNo).FirstOrDefault();

            var ordersPL = OrdersPL.Get().Where(i => i.OrderNo == ordersNo && i.LocaleId == localeId && i.Edition == edition).FirstOrDefault();
            var ordersPack = OrdersPack.Get().Where(i => i.RefOrdersId == ordersPL.RefOrdersId & i.LocaleId == ordersPL.LocaleId && i.RefLocaleId == ordersPL.RefLocaleId && i.Edition == ordersPL.Edition).ToList();

            if (ordersPack != null && ordersPack.Count() > 0)
            {
                var ordersItem = OrdersItem.GetSizeMapping((int)ordersPL.RefOrdersId, (int)ordersPL.RefLocaleId).ToList();
                var ordersPLForUSA = OrdersPLForUSA.Get().Where(i => i.RefOrdersId == ordersPL.RefOrdersId & i.LocaleId == ordersPL.LocaleId && i.RefLocaleId == ordersPL.RefLocaleId && i.Edition == ordersPL.Edition).ToList();
                var ordersPLSpec = OrdersPLSpec.Get().Where(i => i.RefOrdersId == ordersPL.RefOrdersId & i.LocaleId == ordersPL.LocaleId && i.RefLocaleId == ordersPL.RefLocaleId && i.Edition == ordersPL.Edition).ToList();

                //step1:get GroupBy,ArticleSize,OrderSize ,ItemInnerSize,RefDisplaySize,PairOfCTN ,CTNS, NWOfCTN,GWOfCTN,MEASCBM,AdjQty
                var packPlanItems = (
                   from op in ordersPack
                   join oi in ordersItem on new { ArticleInnerSize = op.ItemInnerSize } equals new { ArticleInnerSize = oi.ArticleInnerSize }
                   orderby op.Id
                   select new Models.Views.PackPlanItem
                   {
                       Id = op.Id,
                       LocaleId = op.LocaleId,
                       RefLocaleId = op.RefLocaleId,
                       RefOrdersId = op.RefOrdersId,
                       Edition = op.Edition,

                       GroupBy = op.GroupBy,
                       ArticleSize = (oi.ArticleSize.ToString("0.0") + oi.ArticleSizeSuffix).Trim(),
                       OrderSize = (oi.OrderSize.ToString("0.0") + oi.OrderSizeSuffix).Trim(),
                       ItemInnerSize = op.ItemInnerSize,
                       RefDisplaySize = op.RefDisplaySize,
                       PairOfCTN = op.PairOfCTN,
                       CTNS = op.CTNS,
                       NWOfCTN = op.NWOfCTN,
                       GWOfCTN = op.GWOfCTN,
                       MEAS = op.MEAS,
                       CBM = op.CBM,
                       
                       AdjQty = op.AdjQty,
                       ModifyUserName = op.ModifyUserName,
                       LastUpdateTime = op.LastUpdateTime,
                   }
                )
                .OrderBy(i => i.GroupBy).ThenBy(i => i.ItemInnerSize)
                .ToList();

                //RefDisplaySizeBegin is string ,can't compare like decimal, join OrdersItem and get InnerSize
                var cartons = ordersPLSpec.Where(i => i.Type == 1).Select(i => new
                {
                    Type = i.Type,
                    RefDisplaySizeBegin = i.RefDisplaySizeBegin,
                    InnerSizeBegin = packPlanItems.Where(oi => oi.RefDisplaySize.Trim() == i.RefDisplaySizeBegin.Trim()).Select(oi => oi.ItemInnerSize).FirstOrDefault(),
                    RefDisplaySizeEnd = i.RefDisplaySizeEnd,
                    InnerSizeEnd = packPlanItems.Where(oi => oi.RefDisplaySize.Trim() == i.RefDisplaySizeEnd.Trim()).Select(oi => oi.ItemInnerSize).FirstOrDefault(),
                    Spec = i.Spec,
                    MEAS = i.MEAS
                }).ToList();
                var boxs = ordersPLSpec.Where(i => i.Type == 2).Select(i => new
                {
                    Type = i.Type,
                    RefDisplaySizeBegin = i.RefDisplaySizeBegin,
                    InnerSizeBegin = packPlanItems.Where(oi => oi.RefDisplaySize.Trim() == i.RefDisplaySizeBegin.Trim()).Select(oi => oi.ItemInnerSize).FirstOrDefault(),
                    RefDisplaySizeEnd = i.RefDisplaySizeEnd,
                    InnerSizeEnd = packPlanItems.Where(oi => oi.RefDisplaySize.Trim() == i.RefDisplaySizeEnd.Trim()).Select(oi => oi.ItemInnerSize).FirstOrDefault(),
                    Spec = i.Spec,
                    MEAS = i.MEAS
                }).ToList();

                //step2: MinCNo,MaxCNo,CTNSpec,CTNL,CTNW,CTNH,BOXSpec from PLSpec,PLForUSA
                var CNoFrom = ordersPL.CNoFrom;
                if (ordersPL.PackingType == 0)
                {   // solid
                    packPlanItems.ToList().ForEach(i =>
                    {
                        var maxNo = packPlanItems.Max(m => m.MaxCNo) <= 0 ? (CNoFrom - 1) : packPlanItems.Max(m => m.MaxCNo);
                        i.MinCNo = maxNo + 1;
                        i.MaxCNo = i.MinCNo + i.CTNS - 1;
                        //carton compare innersize & meas,box compare innersize only
                        i.CTNSpec = cartons.Where(c => c.InnerSizeBegin <= i.ItemInnerSize && c.InnerSizeEnd >= i.ItemInnerSize && c.MEAS == i.MEAS).Select(c => c.Spec).FirstOrDefault();
                        i.BOXSpec = boxs.Where(b => b.InnerSizeBegin <= i.ItemInnerSize && b.InnerSizeEnd >= i.ItemInnerSize).Select(b => b.Spec).FirstOrDefault();
                        i.CTNL = ordersPLForUSA.Where(u => Convert.ToInt32(i.MinCNo) == i.MinCNo && Convert.ToInt32(u.MaxCNo) == i.MaxCNo).Select(u => u.CTNL).FirstOrDefault();
                        i.CTNW = ordersPLForUSA.Where(u => Convert.ToInt32(i.MinCNo) == i.MinCNo && Convert.ToInt32(u.MaxCNo) == i.MaxCNo).Select(u => u.CTNW).FirstOrDefault();
                        i.CTNH = ordersPLForUSA.Where(u => Convert.ToInt32(i.MinCNo) == i.MinCNo && Convert.ToInt32(u.MaxCNo) == i.MaxCNo).Select(u => u.CTNH).FirstOrDefault();
                    });
                }
                else
                {   // Mix
                    var groupNos = packPlanItems.Select(i => new { i.GroupBy, i.MEAS }).Distinct().ToList();
                    groupNos.ForEach(g =>
                    {
                        var maxNo = packPlanItems.Max(m => m.MaxCNo) <= 0 ? (CNoFrom - 1) : packPlanItems.Max(m => m.MaxCNo);
                        // RefDisplaySize is string ,can not get Min,Max size like int

                        var beginSize = packPlanItems.Where(i => i.GroupBy == g.GroupBy && i.MEAS == g.MEAS).OrderBy(i => i.ItemInnerSize).Select(i => i.ItemInnerSize).FirstOrDefault();
                        var endSize = packPlanItems.Where(i => i.GroupBy == g.GroupBy && i.MEAS == g.MEAS).OrderByDescending(i => i.ItemInnerSize).Select(i => i.ItemInnerSize).FirstOrDefault();

                        packPlanItems.Where(i => i.GroupBy == g.GroupBy).ToList().ForEach(i =>
                        {
                            i.MinCNo = maxNo + 1;
                            i.MaxCNo = i.MinCNo + i.CTNS - 1;
                            
                            //carton compare innersize & meas,box compare innersize only
                            i.CTNSpec = cartons.Where(c => c.InnerSizeBegin <= beginSize && c.InnerSizeEnd >= endSize && c.MEAS == i.MEAS).Select(c => c.Spec).FirstOrDefault();
                            i.BOXSpec = boxs.Where(b => b.InnerSizeBegin <= beginSize && b.InnerSizeEnd >= endSize).Select(b => b.Spec).FirstOrDefault();
                            i.CTNL = ordersPLForUSA.Where(u => Convert.ToInt32(i.MinCNo) == i.MinCNo && Convert.ToInt32(u.MaxCNo) == i.MaxCNo).Select(u => u.CTNL).FirstOrDefault();
                            i.CTNW = ordersPLForUSA.Where(u => Convert.ToInt32(i.MinCNo) == i.MinCNo && Convert.ToInt32(u.MaxCNo) == i.MaxCNo).Select(u => u.CTNW).FirstOrDefault();
                            i.CTNH = ordersPLForUSA.Where(u => Convert.ToInt32(i.MinCNo) == i.MinCNo && Convert.ToInt32(u.MaxCNo) == i.MaxCNo).Select(u => u.CTNH).FirstOrDefault();

                            if(i.BOXSpec == null || i.BOXSpec =="") {
                                i.BOXSpec = boxs.Where(b => b.MEAS == i.MEAS && b.InnerSizeBegin <= i.ItemInnerSize && b.InnerSizeEnd >= i.ItemInnerSize).Select(b => b.Spec).FirstOrDefault();
                            }
                        });
                    });
                }
                return packPlanItems.OrderBy(i => i.GroupBy).ThenBy(i => i.ItemInnerSize);
            }
            else
            {
                return new List<Models.Views.PackPlanItem>() { };
            }
        }
        
        public void CreateRange(IEnumerable<Models.Views.PackPlanItem> packPlanItems, int packType)
        {
            // OrdersPack has Id compare issuse, need to foreach create by serial 
            // OrdersPack.CreateRange(OrdersPackBuildRange(packPlanItems));
            OrdersPackBuildRange(packPlanItems).ToList().ForEach(i => {
                OrdersPack.Create(i);
            });
            OrdersPLForUSA.CreateRange(OrdersPLForUSABuildRange(packPlanItems));
            OrdersPLSpec.CreateRange(OrdersPLSpecBuildRange(packPlanItems, packType));
        }
        public void RemoveRange(int ordersId, int localeId, string edition)
        {
            OrdersPack.RemoveRange(i => i.RefOrdersId == ordersId && i.LocaleId == localeId && i.Edition == edition);
            OrdersPLForUSA.RemoveRange(i => i.RefOrdersId == ordersId && i.LocaleId == localeId && i.Edition == edition);
            OrdersPLSpec.RemoveRange(i => i.RefOrdersId == ordersId && i.LocaleId == localeId && i.RefLocaleId == localeId && i.Edition == edition);
        }
        public IEnumerable<Models.Entities.OrdersPack> OrdersPackBuildRange(IEnumerable<Models.Views.PackPlanItem> packPlanItems)
        {
            return packPlanItems.Select(item => new Models.Entities.OrdersPack
            {
                LocaleId = item.LocaleId,
                RefLocaleId = item.RefLocaleId,
                RefOrdersId = item.RefOrdersId,
                Edition = item.Edition,
                ItemInnerSize = item.ItemInnerSize,
                RefDisplaySize = item.RefDisplaySize,
                PairOfCTN = item.PairOfCTN,
                CTNS = item.CTNS,
                GroupBy = item.GroupBy,
                NWOfCTN = item.NWOfCTN,
                GWOfCTN = item.GWOfCTN,
                MEAS = item.MEAS,
                CBM = item.CBM,
                AdjQty = item.AdjQty,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
                PLId = item.RefOrdersId+"_"+item.RefLocaleId+"_"+item.Edition
            }).OrderBy(i => i.GroupBy).ThenBy(i => i.ItemInnerSize);
        }
        public IEnumerable<Models.Entities.OrdersPLForUSA> OrdersPLForUSABuildRange(IEnumerable<Models.Views.PackPlanItem> packPlanItems)
        {
            var forUSA = packPlanItems.Select(i => new
            {
                i.MinCNo,
                i.MaxCNo,
                i.CTNL,
                i.CTNW,
                i.CTNH,
                i.Edition,
                i.RefOrdersId,
                i.RefLocaleId,
                i.LocaleId,
                i.ModifyUserName
            }).Distinct();
            return forUSA.Select(item => new Models.Entities.OrdersPLForUSA
            {
                LocaleId = item.LocaleId,
                RefLocaleId = (decimal)item.RefLocaleId,
                RefOrdersId = item.RefOrdersId,
                Edition = item.Edition,
                MinCNo = item.MinCNo.ToString(),
                MaxCNo = item.MaxCNo.ToString(),
                CTNL = item.CTNL,
                CTNH = item.CTNH,
                CTNW = item.CTNW,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
            });
        }
        public IEnumerable<Models.Entities.OrdersPLSpec> OrdersPLSpecBuildRange(IEnumerable<Models.Views.PackPlanItem> packPlanItems, int packType)
        {
            var spec = new List<Models.Entities.OrdersPLSpec>();
            if (packType == 0)
            {
                var cartons = packPlanItems
                    .GroupBy(i => new { i.MEAS, i.Edition, i.RefOrdersId, i.RefLocaleId, i.LocaleId, i.ModifyUserName, i.GroupBy })
                    .Select(i => new Models.Entities.OrdersPLSpec
                    {
                        Type = 1,
                        Spec =  i.OrderByDescending(g => g.ItemInnerSize).Select(g => g.CTNSpec) == null ? "" :
                                i.OrderByDescending(g => g.ItemInnerSize).Select(g => g.CTNSpec).FirstOrDefault(), //i.Key.CTNSpec,
                        Edition = i.Key.Edition,
                        LocaleId = i.Key.LocaleId,
                        RefLocaleId = i.Key.RefLocaleId,
                        RefOrdersId = i.Key.RefOrdersId,
                        MEAS = i.Key.MEAS,
                        RefDisplaySizeBegin = i.OrderBy(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault(),
                        RefDisplaySizeEnd = i.OrderByDescending(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault(),
                        Qty = i.Sum(g => g.CTNS),
                        ModifyUserName = i.Key.ModifyUserName,
                        LastUpdateTime = DateTime.Now,
                    })
                    .ToList();
                cartons.ForEach(i => {
                    i.Spec = i.Spec == null ? "" : i.Spec;
                });
                spec.AddRange(cartons);

                var boxs = packPlanItems
                  .GroupBy(i => new { i.BOXSpec, i.Edition, i.RefOrdersId, i.RefLocaleId, i.LocaleId, i.ModifyUserName, i.GroupBy })
                  .Select(i => new Models.Entities.OrdersPLSpec
                  {
                      Type = 2,
                      Spec = i.Key.BOXSpec == null ? "" : i.Key.BOXSpec,
                      Edition = i.Key.Edition,
                      LocaleId = i.Key.LocaleId,
                      RefLocaleId = i.Key.RefLocaleId,
                      RefOrdersId = i.Key.RefOrdersId,
                      MEAS = i.OrderByDescending(g => g.ItemInnerSize).Select(g => g.MEAS).FirstOrDefault(),
                      RefDisplaySizeBegin = i.OrderBy(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault(),
                      RefDisplaySizeEnd = i.OrderByDescending(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault(),
                      Qty = i.Sum(g => g.CTNS * g.PairOfCTN - g.AdjQty),
                      ModifyUserName = i.Key.ModifyUserName,
                      LastUpdateTime = DateTime.Now,
                  })
                  .ToList();
                spec.AddRange(boxs);
            }
            else
            {
                var cartons = packPlanItems
                    .Select(i => new
                    {
                        CTNSpec = i.CTNSpec,
                        Edition = i.Edition,
                        LocaleId = i.LocaleId,
                        RefLocaleId = i.RefLocaleId,
                        RefOrdersId = i.RefOrdersId,
                        MEAS = i.MEAS,
                        ModifyUserName = i.ModifyUserName,
                        GroupBy = i.GroupBy,
                        CTNS = i.CTNS,
                    })
                    .Distinct()
                    .GroupBy(i => new { i.CTNSpec, i.MEAS, i.Edition, i.RefOrdersId, i.RefLocaleId, i.LocaleId, i.ModifyUserName })
                    .Select(i => new Models.Entities.OrdersPLSpec
                    {
                        Type = 1,
                        Spec = i.Key.CTNSpec == null ? "" : i.Key.CTNSpec,
                        Edition = i.Key.Edition,
                        LocaleId = i.Key.LocaleId,
                        RefLocaleId = i.Key.RefLocaleId,
                        RefOrdersId = i.Key.RefOrdersId,
                        MEAS = i.Key.MEAS,
                        RefDisplaySizeBegin = packPlanItems.Where(g => g.CTNSpec == i.Key.CTNSpec && g.MEAS == i.Key.MEAS).Any() ?
                                              packPlanItems.Where(g => g.CTNSpec == i.Key.CTNSpec && g.MEAS == i.Key.MEAS).OrderBy(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault() : 
                                              "",
                        RefDisplaySizeEnd = packPlanItems.Where(g => g.CTNSpec == i.Key.CTNSpec && g.MEAS == i.Key.MEAS).Any() ? 
                                            packPlanItems.Where(g => g.CTNSpec == i.Key.CTNSpec && g.MEAS == i.Key.MEAS).OrderByDescending(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault() : 
                                            "",
                        Qty = i.Sum(g => g.CTNS),
                        ModifyUserName = i.Key.ModifyUserName,
                        LastUpdateTime = DateTime.Now,
                    })
                    .ToList();

                spec.AddRange(cartons);

                var boxs = packPlanItems
                  .GroupBy(i => new { i.BOXSpec, i.MEAS, i.Edition, i.RefOrdersId, i.RefLocaleId, i.LocaleId, i.ModifyUserName })
                  .Select(i => new Models.Entities.OrdersPLSpec
                  {
                      Type = 2,
                      Spec = i.Key.BOXSpec == null ? "" : i.Key.BOXSpec,
                      Edition = i.Key.Edition,
                      LocaleId = i.Key.LocaleId,
                      RefLocaleId = i.Key.RefLocaleId,
                      RefOrdersId = i.Key.RefOrdersId,
                      MEAS = i.Key.MEAS,
                      RefDisplaySizeBegin = i.OrderBy(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault(),
                      RefDisplaySizeEnd = i.OrderByDescending(g => g.ItemInnerSize).Select(g => g.RefDisplaySize).FirstOrDefault(),
                      Qty = i.Sum(g => g.CTNS * g.PairOfCTN - g.AdjQty),
                      ModifyUserName = i.Key.ModifyUserName,
                      LastUpdateTime = DateTime.Now,
                  })
                  .ToList();
                spec.AddRange(boxs);
            }
            return spec;
        }

    }
}