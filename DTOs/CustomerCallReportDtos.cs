// DTOs/CustomerCallReportDtos.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace geoback.DTOs
{
    // Stakeholder DTO
    public class StakeholderDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("designation")]
        public string Designation { get; set; } = "Shareholder & Director";
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("shareholding")]
        public string Shareholding { get; set; } = string.Empty;
    }

    // Facility Item DTO
    public class FacilityItemDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("facilityType")]
        public string FacilityType { get; set; } = string.Empty;
        
        [JsonPropertyName("existing")]
        public string Existing { get; set; } = string.Empty;
        
        [JsonPropertyName("proposed")]
        public string Proposed { get; set; } = string.Empty;
        
        [JsonPropertyName("instalment")]
        public string Instalment { get; set; } = string.Empty;
        
        [JsonPropertyName("tenor")]
        public string Tenor { get; set; } = string.Empty;
        
        [JsonPropertyName("purpose")]
        public string Purpose { get; set; } = string.Empty;
    }

    // Total Exposure DTO
    public class TotalExposureDto
    {
        [JsonPropertyName("existing")]
        public string Existing { get; set; } = "0";
        
        [JsonPropertyName("proposed")]
        public string Proposed { get; set; } = "0";
        
        [JsonPropertyName("instalment")]
        public string Instalment { get; set; } = "0";
    }

    // Security Item DTO
    public class SecurityItemDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("securityType")]
        public string SecurityType { get; set; } = string.Empty;
        
        [JsonPropertyName("currentMarketValue")]
        public string CurrentMarketValue { get; set; } = string.Empty;
        
        [JsonPropertyName("discountRatio")]
        public string DiscountRatio { get; set; } = string.Empty;
        
        [JsonPropertyName("forcedSaleValue")]
        public string ForcedSaleValue { get; set; } = string.Empty;
        
        [JsonPropertyName("stampedValue")]
        public string StampedValue { get; set; } = string.Empty;
        
        [JsonPropertyName("extendedValue")]
        public string ExtendedValue { get; set; } = string.Empty;
    }

    // Total Security DTO
    public class TotalSecurityDto
    {
        [JsonPropertyName("currentMarketValue")]
        public string CurrentMarketValue { get; set; } = "0";
        
        [JsonPropertyName("forcedSaleValue")]
        public string ForcedSaleValue { get; set; } = "0";
        
        [JsonPropertyName("stampedValue")]
        public string StampedValue { get; set; } = "0";
        
        [JsonPropertyName("extendedValue")]
        public string ExtendedValue { get; set; } = "0";
    }

    // Security Description DTO
    public class SecurityDescriptionDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("propertyNo")]
        public string PropertyNo { get; set; } = string.Empty;
        
        [JsonPropertyName("ino")]
        public string Ino { get; set; } = string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("typeOfUser")]
        public string TypeOfUser { get; set; } = string.Empty;
        
        [JsonPropertyName("valuation")]
        public string Valuation { get; set; } = string.Empty;
        
        [JsonPropertyName("remainingLeasehold")]
        public string RemainingLeasehold { get; set; } = string.Empty;
    }

    // CRB Check Item DTO
    public class CRBCheckItemDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("idNumber")]
        public string IdNumber { get; set; } = string.Empty;
        
        [JsonPropertyName("checkDate")]
        public string CheckDate { get; set; } = string.Empty;
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    // Policy Compliance Item DTO
    public class PolicyComplianceItemDto
    {
        [JsonPropertyName("requirement")]
        public string Requirement { get; set; } = string.Empty;
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        
        [JsonPropertyName("justification")]
        public string Justification { get; set; } = string.Empty;
    }

    // Condition Item DTO
    public class ConditionItemDto
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        
        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
    }

    // Bank Account Stats DTO
    public class BankAccountStatsDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("bankName")]
        public string BankName { get; set; } = string.Empty;
        
        [JsonPropertyName("accountName")]
        public string AccountName { get; set; } = string.Empty;
        
        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; } = string.Empty;
        
        [JsonPropertyName("month")]
        public string Month { get; set; } = string.Empty;
        
        [JsonPropertyName("turnover")]
        public string Turnover { get; set; } = string.Empty;
        
        [JsonPropertyName("averageBalance")]
        public string AverageBalance { get; set; } = string.Empty;
        
        [JsonPropertyName("best")]
        public string Best { get; set; } = string.Empty;
        
        [JsonPropertyName("worst")]
        public string Worst { get; set; } = string.Empty;
    }

    // Photo Section DTO
    public class PhotoSectionDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        
        [JsonPropertyName("photos")]
        public List<string> Photos { get; set; } = new List<string>();
    }

    // Pinned Location DTO
    public class PinnedLocationDto
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        
        [JsonPropertyName("accuracy")]
        public double? Accuracy { get; set; }
        
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        
        [JsonPropertyName("lrNo")]
        public string LrNo { get; set; } = string.Empty;
        
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    // Borrower Business DTO
    public class BorrowerBusinessDto
    {
        [JsonPropertyName("background")]
        public string Background { get; set; } = string.Empty;
        
        [JsonPropertyName("history")]
        public string History { get; set; } = string.Empty;
        
        [JsonPropertyName("directorsProfile")]
        public string DirectorsProfile { get; set; } = string.Empty;
        
        [JsonPropertyName("employees")]
        public string Employees { get; set; } = string.Empty;
        
        [JsonPropertyName("mainBankers")]
        public string MainBankers { get; set; } = string.Empty;
        
        [JsonPropertyName("coreBusiness")]
        public string CoreBusiness { get; set; } = string.Empty;
        
        [JsonPropertyName("productsServices")]
        public string ProductsServices { get; set; } = string.Empty;
        
        [JsonPropertyName("businessCertifications")]
        public string BusinessCertifications { get; set; } = string.Empty;
        
        [JsonPropertyName("modusOperandi")]
        public string ModusOperandi { get; set; } = string.Empty;
        
        [JsonPropertyName("mainSuppliers")]
        public string MainSuppliers { get; set; } = string.Empty;
        
        [JsonPropertyName("mainCustomers")]
        public string MainCustomers { get; set; } = string.Empty;
        
        [JsonPropertyName("otherBusiness")]
        public string OtherBusiness { get; set; } = string.Empty;
        
        [JsonPropertyName("creditorsTerms")]
        public string CreditorsTerms { get; set; } = string.Empty;
        
        [JsonPropertyName("contractsInfo")]
        public string ContractsInfo { get; set; } = string.Empty;
        
        [JsonPropertyName("relatedBusiness")]
        public string RelatedBusiness { get; set; } = string.Empty;
    }

    // Main Customer Call Report Form DTO
    public class CustomerCallReportFormDto
    {
        // Basic Information
        [JsonPropertyName("clientName")]
        public string ClientName { get; set; } = string.Empty;
        
        [JsonPropertyName("yearsInBusiness")]
        public string YearsInBusiness { get; set; } = string.Empty;
        
        [JsonPropertyName("natureOfBusiness")]
        public string NatureOfBusiness { get; set; } = string.Empty;
        
        [JsonPropertyName("locationOfBusiness")]
        public string LocationOfBusiness { get; set; } = string.Empty;
        
        [JsonPropertyName("branch")]
        public string Branch { get; set; } = string.Empty;
        
        [JsonPropertyName("typeOfCompany")]
        public string TypeOfCompany { get; set; } = string.Empty;
        
        [JsonPropertyName("bankOfficial")]
        public string BankOfficial { get; set; } = string.Empty;
        
        // Shareholders & Directors
        [JsonPropertyName("stakeholders")]
        public List<StakeholderDto> Stakeholders { get; set; } = new List<StakeholderDto>();
        
        // Facility Details
        [JsonPropertyName("facilities")]
        public List<FacilityItemDto> Facilities { get; set; } = new List<FacilityItemDto>();
        
        [JsonPropertyName("connectedExposures")]
        public List<FacilityItemDto> ConnectedExposures { get; set; } = new List<FacilityItemDto>();
        
        [JsonPropertyName("otherBankFacilities")]
        public List<FacilityItemDto> OtherBankFacilities { get; set; } = new List<FacilityItemDto>();
        
        [JsonPropertyName("totalNCBAExposure")]
        public TotalExposureDto TotalNCBAExposure { get; set; } = new TotalExposureDto();
        
        [JsonPropertyName("totalConnectedExposure")]
        public TotalExposureDto TotalConnectedExposure { get; set; } = new TotalExposureDto();
        
        [JsonPropertyName("totalOtherBanksExposure")]
        public TotalExposureDto TotalOtherBanksExposure { get; set; } = new TotalExposureDto();
        
        [JsonPropertyName("totalGroupExposure")]
        public TotalExposureDto TotalGroupExposure { get; set; } = new TotalExposureDto();
        
        [JsonPropertyName("securityHeldForConnected")]
        public string SecurityHeldForConnected { get; set; } = string.Empty;
        
        [JsonPropertyName("securityHeldAtOtherBanks")]
        public string SecurityHeldAtOtherBanks { get; set; } = string.Empty;
        
        [JsonPropertyName("sblInsiderNotes")]
        public string SblInsiderNotes { get; set; } = string.Empty;
        
        // Security Details
        [JsonPropertyName("securityItems")]
        public List<SecurityItemDto> SecurityItems { get; set; } = new List<SecurityItemDto>();
        
        [JsonPropertyName("totalSecurity")]
        public TotalSecurityDto TotalSecurity { get; set; } = new TotalSecurityDto();
        
        [JsonPropertyName("totalLoanExposure")]
        public TotalExposureDto TotalLoanExposure { get; set; } = new TotalExposureDto();
        
        [JsonPropertyName("securityCoverage")]
        public TotalExposureDto SecurityCoverage { get; set; } = new TotalExposureDto();
        
        [JsonPropertyName("securityNotes")]
        public string SecurityNotes { get; set; } = string.Empty;
        
        [JsonPropertyName("securityDescriptions")]
        public List<SecurityDescriptionDto> SecurityDescriptions { get; set; } = new List<SecurityDescriptionDto>();
        
        [JsonPropertyName("securities")]
        public List<string> Securities { get; set; } = new List<string>();
        
        [JsonPropertyName("unsecuredAmount")]
        public string UnsecuredAmount { get; set; } = string.Empty;
        
        [JsonPropertyName("unsecuredParameters")]
        public List<string> UnsecuredParameters { get; set; } = new List<string>();
        
        [JsonPropertyName("securityPhotos")]
        public List<string> SecurityPhotos { get; set; } = new List<string>();
        
        [JsonPropertyName("securityPhotosNotes")]
        public string SecurityPhotosNotes { get; set; } = string.Empty;
        
        // Executive Summary
        [JsonPropertyName("summaryOfRequests")]
        public List<string> SummaryOfRequests { get; set; } = new List<string>();
        
        [JsonPropertyName("whatDidClientSay")]
        public string WhatDidClientSay { get; set; } = string.Empty;
        
        [JsonPropertyName("whatDidClientSayPhotos")]
        public List<string> WhatDidClientSayPhotos { get; set; } = new List<string>();
        
        [JsonPropertyName("amountRequested")]
        public string AmountRequested { get; set; } = string.Empty;
        
        [JsonPropertyName("howWillBeDrawn")]
        public string HowWillBeDrawn { get; set; } = string.Empty;
        
        [JsonPropertyName("justification")]
        public string Justification { get; set; } = string.Empty;
        
        [JsonPropertyName("justificationPhotos")]
        public List<string> JustificationPhotos { get; set; } = new List<string>();
        
        [JsonPropertyName("repaymentSource")]
        public string RepaymentSource { get; set; } = string.Empty;
        
        [JsonPropertyName("certaintyOfRepayment")]
        public string CertaintyOfRepayment { get; set; } = string.Empty;
        
        [JsonPropertyName("tenorAndMoratorium")]
        public string TenorAndMoratorium { get; set; } = string.Empty;
        
        // Borrower's Business
        [JsonPropertyName("borrowerBusiness")]
        public BorrowerBusinessDto BorrowerBusiness { get; set; } = new BorrowerBusinessDto();
        
        // CRB & Compliance
        [JsonPropertyName("crbCheckResults")]
        public List<CRBCheckItemDto> CrbCheckResults { get; set; } = new List<CRBCheckItemDto>();
        
        [JsonPropertyName("policyCompliance")]
        public List<PolicyComplianceItemDto> PolicyCompliance { get; set; } = new List<PolicyComplianceItemDto>();
        
        [JsonPropertyName("generalConditions")]
        public List<ConditionItemDto> GeneralConditions { get; set; } = new List<ConditionItemDto>();
        
        [JsonPropertyName("otherConditions")]
        public List<ConditionItemDto> OtherConditions { get; set; } = new List<ConditionItemDto>();
        
        [JsonPropertyName("covenants")]
        public List<ConditionItemDto> Covenants { get; set; } = new List<ConditionItemDto>();
        
        [JsonPropertyName("selectedProduct")]
        public string SelectedProduct { get; set; } = string.Empty;
        
        // Account Performance
        [JsonPropertyName("bankAccounts")]
        public List<BankAccountStatsDto> BankAccounts { get; set; } = new List<BankAccountStatsDto>();
        
        [JsonPropertyName("averageMonthlyTurnover")]
        public string AverageMonthlyTurnover { get; set; } = string.Empty;
        
        [JsonPropertyName("averageBalance")]
        public string AverageBalance { get; set; } = string.Empty;
        
        [JsonPropertyName("accountPerformanceNotes")]
        public string AccountPerformanceNotes { get; set; } = string.Empty;
        
        // Attachments
        [JsonPropertyName("attachments")]
        public List<string> Attachments { get; set; } = new List<string>();
        
        // Photo Sections
        [JsonPropertyName("photoSections")]
        public List<PhotoSectionDto> PhotoSections { get; set; } = new List<PhotoSectionDto>();
        
        // Location Data
        [JsonPropertyName("pinnedLocation")]
        public PinnedLocationDto? PinnedLocation { get; set; }
        
        [JsonPropertyName("businessLatitude")]
        public double? BusinessLatitude { get; set; }
        
        [JsonPropertyName("businessLongitude")]
        public double? BusinessLongitude { get; set; }
        
        // Metadata
        [JsonPropertyName("status")]
        public string Status { get; set; } = "new";
        
        [JsonPropertyName("reportNumber")]
        public string? ReportNumber { get; set; }
        
        [JsonPropertyName("rmId")]
        public string? RmId { get; set; }
        
        [JsonPropertyName("rmName")]
        public string? RmName { get; set; }
    }

    // Create DTO
    public class CreateCustomerCallReportDto
    {
        [JsonPropertyName("formData")]
        public CustomerCallReportFormDto FormData { get; set; } = new CustomerCallReportFormDto();
        
        [JsonPropertyName("rmId")]
        public string RmId { get; set; } = string.Empty;
        
        [JsonPropertyName("rmName")]
        public string RmName { get; set; } = string.Empty;
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = "new";
    }

    // Update DTO
    public class UpdateCustomerCallReportDto
    {
        [JsonPropertyName("formData")]
        public CustomerCallReportFormDto FormData { get; set; } = new CustomerCallReportFormDto();
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = "new";
    }
}