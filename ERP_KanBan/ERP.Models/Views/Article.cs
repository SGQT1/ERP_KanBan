using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Article
    {
        public decimal Id { get; set; }
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public int SizeRange { get; set; }
        //public string OtherRangeDesc { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public decimal LocaleId { get; set; }
        public decimal ProjectType { get; set; }
        public decimal BrandCodeId { get; set; }
        public decimal? OutsoleId { get; set; }
        public decimal? ShellId { get; set; }
        public decimal? KnifeId { get; set; }
        public decimal? LastId { get; set; }
        public decimal? DayCapacity { get; set; }
        public int? LastTurnover { get; set; }
        public string Gender { get; set; }
        public int? forCBD { get; set; }
        public int IsAlternate { get; set; }

        public string OutsoleNo { get; set; }
        public string LastNo { get; set; }
        public string KnifeNo { get; set; }
        public string ShellNo { get; set; }
        public string Brand { get; set; }
        public int? IsUse { get; set; }
    }

    //Id PK
    //ArticleNo 型體編號
    //ArticleName 鞋名
    //SizeRange 分段(0=不分,1=斬分,2=大底,3=楦頭,4-鞋殼,5=其它1,6=其它2)
    //OtherRangeDesc 分段5&6時用來說明
    //ModifyUserName  修改者
    //LastUpdateTime
    //LocaleId 歸屬分公司Id(ref. Company.Id)(PK)
    //ProjectType 立案類別立案類別(ref. CodeItem.Id, CodeType= '45')(FK1)
    //BrandCodeId 品牌別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '25')(FK8)
    //OutsoleId   大底Id(ref. Outsole.Id)
    //ShellId 鞋殼Id(ref. Shell.Id)
    //KnifeId 斬刀Id(ref.Knife.Id)(FK6)
    //LastId  楦頭Id(ref. Last.Id)(FK7)
    //DayCapacity 日產能(雙)
    //LastTurnover 楦頭週轉數
    //msrepl_tran_version
    //Gender  性別(Men’s, Women’s, Unisex)...20131106
    //forCBD 用於CBD否(0=否,1=是)...20150824
}
