using System.Linq;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class OrdersItemSizeRunService : BusinessService
    {
        private Services.Entities.ViewOrdersItemSizeRunService OrdersItemSizeRun { get; }

        public OrdersItemSizeRunService(Services.Entities.ViewOrdersItemSizeRunService rrdersItemSizeRunService, UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.OrdersItemSizeRun = rrdersItemSizeRunService;
        }
        public IQueryable<ERP.Models.Views.View.OrdersItemSizeRun> Get()
        {
            return OrdersItemSizeRun.Get().Select(i => new ERP.Models.Views.View.OrdersItemSizeRun
            {
                LocaleId = i.LocaleId,
                OrdersId = i.OrdersId,
                OrderNo = i.OrderNo,
                ArticleInnerSize = i.ArticleInnerSize,
                ArticleSize = i.ArticleSize,
                ArticleSizeSuffix = i.ArticleSizeSuffix,
                DisplaySize = i.DisplaySize,
                Qty = i.Qty,
                KnifeSize = i.KnifeSize,
                KnifeSizeSuffix = i.KnifeSizeSuffix,
                KnifeInnerSize = i.KnifeInnerSize,
                KnifeDisplaySize = i.KnifeDisplaySize,
                OutsoleSize = i.OutsoleSize,
                OutsoleSizeSuffix = i.OutsoleSizeSuffix,
                OutsoleInnerSize = i.OutsoleInnerSize,
                OutsoleDisplaySize = i.OutsoleDisplaySize,
                LastSize = i.LastSize,
                LastSizeSuffix = i.LastSizeSuffix,
                LastInnerSize = i.LastInnerSize,
                LastDisplaySize = i.LastDisplaySize,
                ShellSize = i.ShellSize,
                ShellSizeSuffix = i.ShellSizeSuffix,
                ShellInnerSize = i.ShellInnerSize,
                ShellDisplaySize = i.ShellDisplaySize,
                Other1Size = i.Other1Size,
                Other1SizeSuffix = i.Other1SizeSuffix,
                Other1InnerSize = i.Other1InnerSize,
                Other1Desc = i.Other1Desc,
                Other2Size = i.Other2Size,
                Other2SizeSuffix = i.Other2SizeSuffix,
                Other2InnerSize = i.Other2InnerSize,
                Other2SizeDesc = i.Other2SizeDesc,
                SizeCountryNameTw = i.SizeCountryNameTw,
                MappingSizeCountryNameTw = i.MappingSizeCountryNameTw,
                MappingShoeSize = i.MappingShoeSize,
                MixedQty1 = i.MixedQty1,
                MixedQty2 = i.MixedQty2,
                MixedQty3 = i.MixedQty3,
                MixedQty4 = i.MixedQty4,
                MixedQty5 = i.MixedQty5,

            });
        }
    }
}