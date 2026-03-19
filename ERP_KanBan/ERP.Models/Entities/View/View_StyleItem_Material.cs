using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class View_StyleItem_Material
    {
        public decimal Id { get; set; }
        public decimal ArticleId { get; set; }
        public string ColorCode { get; set; }
        public string StyleNo { get; set; }
        public string ProcessNoteTW { get; set; }
        public string ColorDesc { get; set; }
        public string InsockLabel { get; set; }
        public string OutsoleColorDescTW { get; set; }
        public string OutsoleColorDescEN { get; set; }
        public string Photo1NoteTW { get; set; }
        public string Photo1URL { get; set; }
        public string Photo2NoteTw { get; set; }
        public string Photo2URL { get; set; }
        public string Photo3NoteTW { get; set; }
        public string Photo3URL { get; set; }
        public string Photo4NoteTW { get; set; }
        public string Photo4URL { get; set; }
        public string Photo5NoteTW { get; set; }
        public string Photo5URL { get; set; }
        public string Photo6NoteTW { get; set; }
        public string Photo6URL { get; set; }
        public string Photo7NoteTW { get; set; }
        public string Photo7URL { get; set; }
        public string Photo8NoteTW { get; set; }
        public string Photo8URL { get; set; }
        public string ModifyUserName { get; set; }
        public string ProcessNoteEng { get; set; }
        public string Photo1NoteEng { get; set; }
        public string Photo2NoteEng { get; set; }
        public string Photo3NoteEng { get; set; }
        public string Photo4NoteEng { get; set; }
        public string Photo5NoteEng { get; set; }
        public string Photo6NoteEng { get; set; }
        public string Photo7NoteEng { get; set; }
        public string Photo8NoteEng { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? ShellId { get; set; }
        public string FinishGoodsPhotoURL { get; set; }
        public decimal LocaleId { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? SizeCountryCodeId { get; set; }
        public decimal? ShoeClassCodeId { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public int doMRP { get; set; }
        public decimal? KnifeId { get; set; }
        public decimal? LastId { get; set; }
        public string MoldNo { get; set; }
        public decimal? ProjectId { get; set; }
        public int Version { get; set; }
        public Guid msrepl_tran_version { get; set; }
        public decimal? CBDPrice { get; set; }
        public string SampleSize { get; set; }
        public string SampleSizeSuffix { get; set; }
        public decimal? SampleInnerSize { get; set; }
        public decimal ArticlePartId { get; set; }
        public decimal MaterialId { get; set; }
        public int EnableMaterial { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal? Expr1 { get; set; }
        public string MaterialNo { get; set; }
    }
}
