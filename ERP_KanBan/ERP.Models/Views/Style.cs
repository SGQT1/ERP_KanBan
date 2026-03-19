using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Style
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
        public string Photo1Photo { get; set; }
        public string Photo2NoteTw { get; set; }
        public string Photo2URL { get; set; }
        public string Photo2Photo { get; set; }
        public string Photo3NoteTW { get; set; }
        public string Photo3URL { get; set; }
        public string Photo3Photo { get; set; }
        public string Photo4NoteTW { get; set; }
        public string Photo4URL { get; set; }
        public string Photo4Photo { get; set; }
        public string Photo5NoteTW { get; set; }
        public string Photo5URL { get; set; }
        public string Photo5Photo { get; set; }
        public string Photo6NoteTW { get; set; }
        public string Photo6URL { get; set; }
        public string Photo6Photo { get; set; }
        public string Photo7NoteTW { get; set; }
        public string Photo7URL { get; set; }
        public string Photo7Photo { get; set; }
        public string Photo8NoteTW { get; set; }
        public string Photo8URL { get; set; }
        public string Photo8Photo { get; set; }
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
        public string FinishGoodsPhoto { get; set; }
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
        public decimal? CBDPrice { get; set; }
        public string SampleSize { get; set; }
        public string SampleSizeSuffix { get; set; }
        public decimal? SampleInnerSize { get; set; }
        // ArticleNo,ArticleName,BrandCodeId
        public string ArticleNo {get;set;}
        public string ArticleName {get;set;}
        public decimal BrandCodeId { get; set; }
        public string Brand { get; set; }
        public string StyleSizeCountry { get; set; }
        public string ShoeClass { get; set; }
        public string Category { get; set; }
        public string Knife {get; set;}
        public string Last {get; set;}
        public string Outsole { get; set; }
        public string Shell { get; set; }
        public int IsSpecial { get; set; }

        public string Customer { get; set; }
    }
    //Id
    //ArticleId   型體的Id(ref. Article.Id)
    //ColorCode 配色碼
    //StyleNo 鞋型編號
    //ProcessNoteTW 中文製造說明
    //ColorDesc 鞋型顏色描述
    //InsockLabel 鞋墊標內容
    //OutsoleColorDescTW 中文大底顏色描述
    //OutsoleColorDescEN 英文大底顏色描述
    //Photo1NoteTW 圖檔1的中文說明
    //Photo1URL 圖檔1的路徑
    //Photo2NoteTw 圖檔2的中文說明
    //Photo2URL 圖檔2的路徑
    //Photo3NoteTW 圖檔3的中文說明
    //Photo3URL 圖檔3的路徑
    //Photo4NoteTW 圖檔4的中文說明
    //Photo4URL 圖檔4的路徑
    //Photo5NoteTW 圖檔5的中文說明
    //Photo5URL 圖檔5的路徑檔
    //Photo6NoteTW 圖檔6的中文說明
    //Photo6URL 圖檔6的路徑
    //Photo7NoteTW 圖檔7的中文說明
    //Photo7URL 圖檔7的路徑
    //Photo8NoteTW 圖檔8的中文說明
    //Photo8URL 圖檔8的路徑
    //ModifyUserName 修改者
    //ProcessNoteEng 英文製造說明
    //Photo1NoteEng 圖檔1的英文說明
    //Photo2NoteEng 圖檔2的英文說明
    //Photo3NoteEng 圖檔3的英文說明
    //Photo4NoteEng 圖檔4的英文說明
    //Photo5NoteEng 圖檔5的英文說明
    //Photo6NoteEng 圖檔6的英文說明
    //Photo7NoteEng 圖檔7的英文說明
    //Photo8NoteEng 圖檔8的英文說明
    //LastUpdateTime
    //OutsoleId   大底Id(ref. Outsole.Id)
    //ShellId 鞋殼Id(ref. Shell.Id)
    //FinishGoodsPhotoURL 完成品圖檔路徑
    //LocaleId
    //CustomerId  客戶編號(ref.Customer.Id)(FK2)
    //SizeCountryCodeId   尺寸國家設定Id(ref. CodeItem.Id, CodeItem.CodeType= '35')(FK3)
    //ShoeClassCodeId 鞋種類別設定的Id(ref. CodeItem.Id, CodeItem.CodeType= '26')(FK4)
    //CategoryCodeId  Category類別設定的Id(ref. CodeItem.Id, CodeItem.CodeType= '41')(FK5)
    //doMRP   可否展開(0=否, 1=是)
    //KnifeId 斬刀Id(ref.Knife.Id)(FK6)
    //LastId  楦頭Id(ref. Last.Id)(FK7)
    //MoldNo  模治具編號
    //ProjectId   立案的識別碼(參考用)(ref. Project.Id)
    //Version 版本
    //msrepl_tran_version
    //CBDPrice    客戶端CBD報價每雙...20150824
    //SampleSize UK樣品號...20150824
    //SampleSizeSuffix UK樣品號後置字元(J)...20150824
    //SampleInnerSize UK樣品號內碼...20150824
}
