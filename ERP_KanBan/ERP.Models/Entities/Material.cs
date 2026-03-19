using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Material
    {
        public Material()
        {
            MaterialPOControl = new HashSet<MaterialPOControl>();
            MaterialQuot = new HashSet<MaterialQuot>();
            MaterialStock = new HashSet<MaterialStock>();
            MaterialVendorItem = new HashSet<MaterialVendorItem>();
            ProjectArticleItem = new HashSet<ProjectArticleItem>();
            SOMMaterial = new HashSet<SOM>();
            SOMMaterialNavigation = new HashSet<SOM>();
            StyleItem = new HashSet<StyleItem>();
            StyleSizeRunUsage = new HashSet<StyleSizeRunUsage>();
        }

        public decimal Id { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public decimal? MaterialCodeId { get; set; }
        public decimal? NameCodeId { get; set; }
        public decimal? ThicknessCodeId { get; set; }
        public decimal? TextureCodeId { get; set; }
        public decimal? ColorCodeId { get; set; }
        public decimal? CanvasCodeId { get; set; }
        public decimal? ProcessCodeId { get; set; }
        public decimal? SpecCodeId { get; set; }
        public decimal? VesicantCodeId { get; set; }
        public decimal? InColorCodeId { get; set; }
        public decimal? OutColorCodeId { get; set; }
        public decimal? UsedUnitCodeId { get; set; }
        public decimal? PriceUnitCodeId { get; set; }
        public decimal? UsedPriceRate { get; set; }
        public decimal? PurchasingUnitCodeId { get; set; }
        public decimal? UsedPurchasingRate { get; set; }
        public decimal? MinPurchasingQty { get; set; }
        public decimal? MinStockOutQty { get; set; }
        public decimal? WeightEachUnit { get; set; }
        public decimal? WeightUnitCodeId { get; set; }
        public decimal? UsedWeightRate { get; set; }
        public decimal? VolumeEachUnit { get; set; }
        public decimal? VolumeUnitCodeId { get; set; }
        public decimal? UsedVolumeRate { get; set; }
        public decimal? LastQuotationPrice { get; set; }
        public decimal? HighestPrice { get; set; }
        public decimal? BeforeLastPrice { get; set; }
        public decimal? LowestPrice { get; set; }
        public decimal? AcceptabilityOverRate { get; set; }
        public string ModifyUserName { get; set; }
        public string OtherDescTW { get; set; }
        public string Hardness { get; set; }
        public string OtherDescEng { get; set; }
        public string WeightEachYard { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int? SemiGoods { get; set; }
        public string OtherName { get; set; }
        public string AddOnDesc { get; set; }
        public decimal LocaleId { get; set; }
        public int SamplingMethod { get; set; }
        public string MaterialNo { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal? GroupId { get; set; }

        public virtual Company Locale { get; set; }
        public virtual ICollection<MaterialPOControl> MaterialPOControl { get; set; }
        public virtual ICollection<MaterialQuot> MaterialQuot { get; set; }
        public virtual ICollection<MaterialStock> MaterialStock { get; set; }
        public virtual ICollection<MaterialVendorItem> MaterialVendorItem { get; set; }
        public virtual ICollection<ProjectArticleItem> ProjectArticleItem { get; set; }
        public virtual ICollection<SOM> SOMMaterial { get; set; }
        public virtual ICollection<SOM> SOMMaterialNavigation { get; set; }
        public virtual ICollection<StyleItem> StyleItem { get; set; }
        public virtual ICollection<StyleSizeRunUsage> StyleSizeRunUsage { get; set; }
    }
}
