using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business.Entities
{
    public class NBPPMService : BusinessService
    {
        private Services.Entities.NBPPMService NBPPM { get; }

        public NBPPMService(
            Services.Entities.NBPPMService nbPPMService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            NBPPM = nbPPMService;
        }
        public IQueryable<Models.Views.NBPPM> Get()
        {
            return NBPPM.Get().Select(i => new Models.Views.NBPPM
            {
                Id = i.Id,
                MaterialCode = i.MaterialCode,
                CommodityType = i.CommodityType,
                Description = i.Description,
                UOM = i.UOM,
                ColorKey = i.ColorKey,
                NBColorName = i.NBColorName,
                ColorFamily = i.ColorFamily,
                VendorName = i.VendorName,
                VendorCode = i.VendorCode,
            });
        }

        public void CreateRange(IEnumerable<Models.Views.NBPPM> items)
        {
            NBPPM.CreateRange(StockOutBuildRange(items));
        }
        public void RemoveRange(List<decimal> items) {
            NBPPM.RemoveRange(i => items.Contains(i.Id));
        }
        private IEnumerable<Models.Entities.NBPPM> StockOutBuildRange(IEnumerable<Models.Views.NBPPM> items)
        {
            return items.Select(item => new Models.Entities.NBPPM
            {
                Id = item.Id,
                MaterialCode = item.MaterialCode,
                CommodityType = item.CommodityType,
                Description = item.Description,
                UOM = item.UOM,
                ColorKey = item.ColorKey,
                NBColorName = item.NBColorName,
                ColorFamily = item.ColorFamily,
                VendorName = item.VendorName,
                VendorCode = item.VendorCode,
            });
        }
        
    }
}