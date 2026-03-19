using System;
using System.Collections.Generic;

namespace ERP.Models.Views
{
    public class Material
    {
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
        public decimal? GroupId { get; set; }
        public string CategoryCode { get; set; }
        public string VolumeUnitCode { get; set; }

        public decimal RefLocaleId { get; set; }  // for cross copy
    }
    //Id
    //MaterialName    材料中文名稱(U2)
    //MaterialNameEng 材料英文名稱
    //CategoryCodeId 材料類別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '11')
    //MaterialCodeId 材質的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '32')
    //NameCodeId 品名的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '12')
    //ThicknessCodeId 厚度的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '13')
    //TextureCodeId 隔離(0=否, 1=是)
    //ColorCodeId 顏色的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '20')
    //CanvasCodeId 底布的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '15')
    //ProcessCodeId 加工的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '16')
    //SpecCodeId 規格的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '18')
    //VesicantCodeId 發泡別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '19')
    //InColorCodeId 裡層顏色別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '34')
    //OutColorCodeId 外層顏色別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '34')
    //UsedUnitCodeId 用量單位別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '21')
    //PriceUnitCodeId 計價單位別的設定Id(ref. CodeItem.Id, CodeItem.CodeType= '21')
    //UsedPriceRate 計價單位比率
    //PurchasingUnitCodeId 採購單位(ref. CodeItem.CodeId, CodeItem.CodeType= '21')
    //UsedPurchasingRate 採購單位比率
    //MinPurchasingQty 最少採購量
    //MinStockOutQty 最少領用量
    //WeightEachUnit 單位重量
    //WeightUnitCodeId 重量計量單位(ref. CodeItem.CodeId, CodeItem.CodeType= '21')
    //UsedWeightRate 重量單位比率
    //VolumeEachUnit 材積大小
    //VolumeUnitCodeId 材積大小計量單位(ref. CodeItem.CodeId, CodeItem.CodeType= '21')
    //UsedVolumeRate 材積大小單位比率
    //LastQuotationPrice 上次報價
    //HighestPrice 最高報價
    //BeforeLastPrice 上上次報價
    //LowestPrice 最低報價
    //AcceptabilityOverRate 超收率
    //ModifyUserName 修改者
    //OtherDescTW 大底配色或其他說明(中文)
    //Hardness 硬度
    //OtherDescEng 大底配色或其他說明(英文)
    //WeightEachYard 碼重
    //LastUpdateTime
    //SemiGoods   是否為貼合材料中的主料(0=不是, 1=是)
    //OtherName 主要品名(材質+品名類別)
    //AddOnDesc 附加說明(素材+底布+製程.....etc)
    //LocaleId(ref. Company.Id)
    //SamplingMethod 抽樣方法(0=全檢, 1=抽檢,2=免驗)
    //MaterialNo
    //Text1
    //Text2
    //msrepl_tran_version

    public class MaterialCache {
        public decimal Id { get; set; }
        public string MaterialName { get; set; }
        public string MaterialNameEng { get; set; }
        public decimal? CategoryCodeId { get; set; }
        public int? SemiGoods { get; set; }
        public decimal? TextureCodeId { get; set; }
        public decimal LocaleId { get; set; }
    }

    public class MaterialSimple {
        public decimal Id { get; set; }
        public string Material { get; set; }
        public string MaterialNameEng { get; set; }
        // public decimal? CategoryCodeId { get; set; }
        // public int? SemiGoods { get; set; }
        // public decimal? TextureCodeId { get; set; }
        public decimal LocaleId { get; set; }
    }
}
