using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ERP.Models.Entities
{
    public partial class Staff
    {
        public decimal Id { get; set; }
        public decimal LocaleId { get; set; }
        public string EmpNo { get; set; }
        public decimal SalaryPayTypeId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public decimal CountryCodeId { get; set; }
        public string LicenseId { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal Interviewer { get; set; }
        public string PapaersNo { get; set; }
        public int Status { get; set; }
        public string PhotoURL { get; set; }
        public int Marriage { get; set; }
        public string MateName { get; set; }
        public decimal? MateCountryCodeId { get; set; }
        public DateTime? MateBirthday { get; set; }
        public decimal? MateCompanyId { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int Siblings { get; set; }
        public string RegistrationProvince { get; set; }
        public string RegistrationCounty { get; set; }
        public string RegistrationAddress { get; set; }
        public string RegistrationTelNo { get; set; }
        public string ContactProvince { get; set; }
        public string ContactCounty { get; set; }
        public string ContactAddress { get; set; }
        public string ContacctTelNo { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmergencyTelNo { get; set; }
        public string MobilePhoneNo { get; set; }
        public string PassportNo { get; set; }
        public string PassportEnglishName { get; set; }
        public DateTime? PassportExpiredDate { get; set; }
        public string PassportPhotoURL { get; set; }
        public string TRPNo { get; set; }
        public DateTime? TRPExpiredDate { get; set; }
        public string TRPPhotoURL { get; set; }
        public int? HighestEducation { get; set; }
        public int? Graduated { get; set; }
        public string HighestSchooling { get; set; }
        public string HighestMajor { get; set; }
        public string OtherSchooling { get; set; }
        public string OtherMajor { get; set; }
        public string ProfessionalLicense1 { get; set; }
        public string ProfessionalLicense2 { get; set; }
        public int? LanguageFluency { get; set; }
        public string LanguageOther { get; set; }
        public string ITProfession { get; set; }
        public decimal OrgUnitId { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public decimal InitUnitId { get; set; }
        public decimal InitTitleId { get; set; }
        public Guid msrepl_tran_version { get; set; }

        public virtual OrgUnit OrgUnit { get; set; }
    }
}
