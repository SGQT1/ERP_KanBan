using System;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Business.Entities
{
    public class PackSpecService : BusinessService
    {
        private Services.Entities.PackSpecService PackSpec { get; set; }
        private Services.Entities.CodeItemService CodeItem { get; }

        public PackSpecService(
            Services.Entities.PackSpecService packSpecService,
            Services.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.PackSpec = packSpecService;
            this.CodeItem = codeItemService;
        }
        public IQueryable<PackSpec> Get()
        {
            var packSpec = (
                from p in PackSpec.Get()
                join c in CodeItem.Get() on new { BrandCodeId = (decimal)p.BrandCodeId, LocaleId = p.LocaleId } equals new { BrandCodeId = c.Id, LocaleId = c.LocaleId }
                select new PackSpec
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,
                    BrandCodeId = (decimal)p.BrandCodeId,
                    Type = p.Type,
                    Spec = p.Spec,
                    L = p.L,
                    W = p.W,
                    H = p.H,
                    TextureCodeId = (decimal)p.TextureCodeId,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,
                    Brand = p.Brand,
                    RefBrand = c.NameTW,
                }
            );
            return packSpec;
        }

        public ERP.Models.Views.PackSpec Get(decimal Id, decimal LocaleId)
        {
            return Get().Where(i => i.Id == Id && i.LocaleId == LocaleId).FirstOrDefault();
        }

        public ERP.Models.Views.PackSpec Create(PackSpec packSpec)
        {
            var item = PackSpec.Create(Build(packSpec));
            return Get(item.Id, item.LocaleId);
        }

        public ERP.Models.Views.PackSpec Update(PackSpec packSpec)
        {
            var item = PackSpec.Update(Build(packSpec));
            return Get(item.Id, item.LocaleId);
        }

        public void Remove(PackSpec packSpec)
        {
            PackSpec.Remove(Build(packSpec));
        }

        public void UpdateSpec()
        {
            var specs = Get().ToList();
            foreach (var spec in specs)
            {
                try
                {
                    if (spec.Spec.ToLower().Contains("mm"))
                    {
                        var specStr = spec.Spec.ToLower().Replace("mm", "").Trim();
                        var lwh = specStr.Split('*');
                        if (lwh.Length == 3)
                        {
                            spec.L = lwh[0];
                            spec.W = lwh[1];
                            spec.H = lwh[2];
                        }
                    }
                    else if (spec.Spec.ToLower().Contains("cm"))
                    {
                        var specStr = spec.Spec.ToLower().Replace("cm", "").Trim();
                        var lwh = specStr.Split('*');
                        if (lwh.Length == 3)
                        {
                            spec.L = Convert.ToInt32((Convert.ToDecimal(lwh[0]) * 10)).ToString();
                            spec.W = Convert.ToInt32((Convert.ToDecimal(lwh[1]) * 10)).ToString();
                            spec.H = Convert.ToInt32((Convert.ToDecimal(lwh[2]) * 10)).ToString();
                        }
                    }
                    // await Update(spec);
                    var item = PackSpec.Update(Build(spec));
                }
                catch
                {
                    
                }

            }
        }
        private ERP.Models.Entities.PackSpec Build(PackSpec item)
        {
            return new ERP.Models.Entities.PackSpec
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                BrandCodeId = item.BrandCodeId,
                Brand = item.Brand,
                Type = item.Type,
                Spec = item.Spec,
                L = item.L,
                W = item.W,
                H = item.H,
                TextureCodeId = item.TextureCodeId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = DateTime.Now,
            };
        }
    }
}