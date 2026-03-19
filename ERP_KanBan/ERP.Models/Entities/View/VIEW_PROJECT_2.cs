using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class VIEW_PROJECT_2
    {
        public decimal Id { get; set; }
        public decimal ProjectType { get; set; }
        public string ArticleNo { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public DateTime OpenDate { get; set; }
        public decimal? LastId { get; set; }
        public decimal? OutsoleId { get; set; }
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
        public Guid msrepl_tran_version { get; set; }
        public string ProjectTypeNameTw { get; set; }
        public string LastNo { get; set; }
        public string OutsoleNo { get; set; }
        public string ShellNo { get; set; }
        public string BrandNameTw { get; set; }
        public string SizeCountryNameTw { get; set; }
    }
}
