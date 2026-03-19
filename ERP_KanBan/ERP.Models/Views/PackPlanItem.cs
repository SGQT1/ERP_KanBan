using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    /*
     * replace PackPlanItem
     */
    public class PackPlanItem
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }       //資料所屬廠別
        public decimal? RefLocaleId { get; set; }   //訂單資料所屬廠別
        public decimal RefOrdersId { get; set; }    //訂單ID
        public string Edition { get; set; }         //PL 版本
        public int PackingType {get;set;}
        public int GroupBy { get; set; }            //是不是在同一個箱子裡(＋順序)
        public string ArticleSize {get;set;}
        public string OrderSize { get;set; }
        public decimal ItemInnerSize { get; set; }  //比對用的Size
        public string RefDisplaySize { get; set; }  //顯示用的size
        public decimal PairOfCTN { get; set; }      //該size在carton的數量
        public int CTNS { get; set; }               //該裝法有幾個箱子
        public int MinCNo { get; set; }
        public int MaxCNo { get; set; }
        public decimal NWOfCTN { get; set; }        //淨重
        public decimal GWOfCTN { get; set; }        //毛重
        public decimal MEAS { get; set; }           
        public decimal CBM { get; set; }
        public string CTNSpec { get; set; }
        public string CTNL { get; set; }
        public string CTNW { get; set; }
        public string CTNH { get; set; }
        public string BOXSpec { get; set; }
        public string BOXL { get; set; }
        public string BOXW { get; set; }
        public string BOXH { get; set; }
        public decimal AdjQty { get; set; }      
        public string ModifyUserName { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Guid MsreplTranVersion { get; set; }
    }
}
