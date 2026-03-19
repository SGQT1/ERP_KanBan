using System.Linq;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class BOMRepPCLBKService : BusinessService
    {
        private Services.Entities.BOMRepPCLBKService BOMRepPCLBK { get; }

        public BOMRepPCLBKService(Services.Entities.BOMRepPCLBKService bomRepPCLBKService, UnitOfWork unitOfWork):base(unitOfWork)
        {
            this.BOMRepPCLBK = bomRepPCLBKService;
        }
        public IQueryable<ERP.Models.Views.View.BOMPCL> Get()
        {
            return BOMRepPCLBK.Get().Select(i => new ERP.Models.Views.View.BOMPCL
            {
                Id = i.Id,
                OrdersId = i.OrdersId,
                DisplaySize = i.DisplaySize,
                MappingSize = i.MappingSize,
                Qty = i.Qty,
                Other1Size = i.other1size,
                Other2Size = i.other2size,
                LocaleId = i.LocaleId,
                ShellDisplaySize = i.ShellDisplaySize,
                OrderSizeCountryCodeId = i.SizeCountryCodeId,
                ArticleSizeCountryCodeId = i.SizeCountryCodeId,
                KnifeDisplaySize = i.KnifeDisplaySize,
                OutsoleDisplaySize = i.OutsoleDisplaySize,
                LastDisplaySize = i.LastDisplaySize,
                ArticleInnerSize = i.ArticleInnerSize,
            });
        }
    }
}