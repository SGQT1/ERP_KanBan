namespace ERP.Models.Views
{
    public class PackArticleItem
    {
        public decimal PackMappingId { get; set; }
        public decimal PackMappingItem1Id { get; set; }
        public decimal PackMappingItem2Id { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ArticleSize { get; set; }
        public string ArticleSizeSuffix { get; set; }
        public decimal ArticleInnerSize { get; set; }
        public decimal? PairsOfCTN { get; set; }
        public decimal GWOfCTN { get; set; }
        public decimal NWOfCTN { get; set; }
        public decimal MEAS { get; set; }
        public decimal GWOfCTNCLB { get; set; }
        public decimal MEASCLB { get; set; }
        public string CTNSpec { get; set; }
        public string CTNSpecCLB { get; set; }
        public string CTNL { get; set; }
        public string CTNW { get; set; }
        public string CTNH { get; set; }
        public string BOXSpec { get; set; }
        public string BOXSpecCLB { get; set; }
        public string BOXL { get; set; }
        public string BOXW { get; set; }
        public string BOXH { get; set; }
    }
}