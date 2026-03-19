using System;

namespace ERP.Models.Views.Projects
{
    public class Project
    {
        public decimal Id { get; set; }
        public int? ProjectProfileId { get; set; }
        public decimal ProjectType { get; set; }
        public string ArticleNo { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public DateTime OpenDate { get; set; }
        public int? ProjectLastId { get; set; }
        public int? ProjectOutsoleId { get; set; }
        public string MoldNo { get; set; }
        public decimal? ShellId { get; set; }
        public decimal CustomerId { get; set; }
        public string Season { get; set; }
        public decimal HeelHeight { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingPlace { get; set; }
        public string MeetingAttendedMen { get; set; }
        public string SurfacePhotoURL { get; set; }
        public string BottomPhotoURL { get; set; }
        public string Notice { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string ModifyUserName { get; set; }
        public string ShoeName { get; set; }
        public string ProcessDesc { get; set; }
        public decimal? BrandCodeId { get; set; }
        public string FinishGoodsPhotoURL { get; set; }
        public decimal SizeCountryCodeId { get; set; }
        public int Status { get; set; }
        public decimal LocaleId { get; set; }
        public string Remark { get; set; }
        public string OtherPhotoURL { get; set; }
        public decimal? InnerHeight { get; set; }
        public decimal? OuterHeight { get; set; }
        public decimal? ShoeClassCodeId { get; set; }
        public string Patterner { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Percentage { get; set; }
        public string Structure { get; set; }
        public string TemplateNo { get; set; }
        public string ReferenceArticleNo { get; set; }
        public int BrandId { get; set; }

        public string ProjectTypeText { get; set; }
        public string ProjectLast { get; set; }
        public string ProjectOutsole { get; set; }
        public string Customer { get; set; }
        public string BrandCode { get; set; }
        public string SizeCountryCode { get; set; }
        public string ShoeClassCode { get; set; }
        public string Locale { get; set; }
        public string Brand { get; set; }
    }
}
