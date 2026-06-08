// Models/CustomerCallReport.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace geoback.Models
{
    [Table("customer_call_reports")]
    public class CustomerCallReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("report_number")]
        public string ReportNumber { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "new";

        // Basic Information - Using TEXT for longer fields
        [Column("client_name")]
        public string ClientName { get; set; } = string.Empty;

        [Column("years_in_business", TypeName = "TEXT")]
        public string YearsInBusiness { get; set; } = string.Empty;

        [Column("nature_of_business", TypeName = "LONGTEXT")]
        public string NatureOfBusiness { get; set; } = string.Empty;

        [Column("location_of_business", TypeName = "TEXT")]
        public string LocationOfBusiness { get; set; } = string.Empty;

        [Column("branch", TypeName = "TEXT")]
        public string Branch { get; set; } = string.Empty;

        [Column("type_of_company", TypeName = "TEXT")]
        public string TypeOfCompany { get; set; } = string.Empty;

        [Column("bank_official", TypeName = "TEXT")]
        public string BankOfficial { get; set; } = string.Empty;

        // JSON Fields - Using LONGTEXT
        [Column("stakeholders", TypeName = "LONGTEXT")]
        public string StakeholdersJson { get; set; } = "[]";

        [Column("facilities", TypeName = "LONGTEXT")]
        public string FacilitiesJson { get; set; } = "[]";

        [Column("connected_exposures", TypeName = "LONGTEXT")]
        public string ConnectedExposuresJson { get; set; } = "[]";

        [Column("other_bank_facilities", TypeName = "LONGTEXT")]
        public string OtherBankFacilitiesJson { get; set; } = "[]";

        [Column("total_ncba_exposure", TypeName = "LONGTEXT")]
        public string TotalNCBAExposureJson { get; set; } = "{}";

        [Column("total_connected_exposure", TypeName = "LONGTEXT")]
        public string TotalConnectedExposureJson { get; set; } = "{}";

        [Column("total_other_banks_exposure", TypeName = "LONGTEXT")]
        public string TotalOtherBanksExposureJson { get; set; } = "{}";

        [Column("total_group_exposure", TypeName = "LONGTEXT")]
        public string TotalGroupExposureJson { get; set; } = "{}";

        [Column("security_held_for_connected", TypeName = "TEXT")]
        public string SecurityHeldForConnected { get; set; } = string.Empty;

        [Column("security_held_at_other_banks", TypeName = "TEXT")]
        public string SecurityHeldAtOtherBanks { get; set; } = string.Empty;

        [Column("sbl_insider_notes", TypeName = "TEXT")]
        public string SblInsiderNotes { get; set; } = string.Empty;

        // Security Details
        [Column("security_items", TypeName = "LONGTEXT")]
        public string SecurityItemsJson { get; set; } = "[]";

        [Column("total_security", TypeName = "LONGTEXT")]
        public string TotalSecurityJson { get; set; } = "{}";

        [Column("total_loan_exposure_stamped", TypeName = "TEXT")]
        public string TotalLoanExposureStamped { get; set; } = string.Empty;

        [Column("total_loan_exposure_extended", TypeName = "TEXT")]
        public string TotalLoanExposureExtended { get; set; } = string.Empty;

        [Column("security_coverage_stamped", TypeName = "TEXT")]
        public string SecurityCoverageStamped { get; set; } = string.Empty;

        [Column("security_coverage_extended", TypeName = "TEXT")]
        public string SecurityCoverageExtended { get; set; } = string.Empty;

        [Column("security_notes", TypeName = "LONGTEXT")]
        public string SecurityNotes { get; set; } = string.Empty;

        [Column("security_descriptions", TypeName = "LONGTEXT")]
        public string SecurityDescriptionsJson { get; set; } = "[]";

        [Column("securities", TypeName = "LONGTEXT")]
        public string SecuritiesJson { get; set; } = "[]";

        [Column("unsecured_amount", TypeName = "TEXT")]
        public string UnsecuredAmount { get; set; } = string.Empty;

        [Column("unsecured_parameters", TypeName = "LONGTEXT")]
        public string UnsecuredParametersJson { get; set; } = "[]";

        [Column("security_photos", TypeName = "LONGTEXT")]
        public string SecurityPhotosJson { get; set; } = "[]";

        [Column("security_photos_notes", TypeName = "TEXT")]
        public string SecurityPhotosNotes { get; set; } = string.Empty;

        // Executive Summary
        [Column("summary_of_requests", TypeName = "LONGTEXT")]
        public string SummaryOfRequestsJson { get; set; } = "[]";

        [Column("what_did_client_say", TypeName = "LONGTEXT")]
        public string WhatDidClientSay { get; set; } = string.Empty;

        [Column("what_did_client_say_photos", TypeName = "LONGTEXT")]
        public string WhatDidClientSayPhotosJson { get; set; } = "[]";

        [Column("amount_requested", TypeName = "TEXT")]
        public string AmountRequested { get; set; } = string.Empty;

        [Column("how_will_be_drawn", TypeName = "LONGTEXT")]
        public string HowWillBeDrawn { get; set; } = string.Empty;

        [Column("justification", TypeName = "LONGTEXT")]
        public string Justification { get; set; } = string.Empty;

        [Column("justification_photos", TypeName = "LONGTEXT")]
        public string JustificationPhotosJson { get; set; } = "[]";

        [Column("repayment_source", TypeName = "LONGTEXT")]
        public string RepaymentSource { get; set; } = string.Empty;

        [Column("certainty_of_repayment", TypeName = "LONGTEXT")]
        public string CertaintyOfRepayment { get; set; } = string.Empty;

        [Column("tenor_and_moratorium", TypeName = "TEXT")]
        public string TenorAndMoratorium { get; set; } = string.Empty;

        // Borrower's Business
        [Column("borrower_background", TypeName = "LONGTEXT")]
        public string BorrowerBackground { get; set; } = string.Empty;

        [Column("borrower_history", TypeName = "LONGTEXT")]
        public string BorrowerHistory { get; set; } = string.Empty;

        [Column("borrower_directors_profile", TypeName = "LONGTEXT")]
        public string BorrowerDirectorsProfile { get; set; } = string.Empty;

        [Column("borrower_employees", TypeName = "LONGTEXT")]
        public string BorrowerEmployees { get; set; } = string.Empty;

        [Column("borrower_main_bankers", TypeName = "TEXT")]
        public string BorrowerMainBankers { get; set; } = string.Empty;

        [Column("borrower_core_business", TypeName = "TEXT")]
        public string BorrowerCoreBusiness { get; set; } = string.Empty;

        [Column("borrower_products_services", TypeName = "TEXT")]
        public string BorrowerProductsServices { get; set; } = string.Empty;

        [Column("borrower_certifications", TypeName = "LONGTEXT")]
        public string BorrowerCertifications { get; set; } = string.Empty;

        [Column("borrower_modus_operandi", TypeName = "LONGTEXT")]
        public string BorrowerModusOperandi { get; set; } = string.Empty;

        [Column("borrower_main_suppliers", TypeName = "LONGTEXT")]
        public string BorrowerMainSuppliers { get; set; } = string.Empty;

        [Column("borrower_main_customers", TypeName = "LONGTEXT")]
        public string BorrowerMainCustomers { get; set; } = string.Empty;

        [Column("borrower_other_business", TypeName = "LONGTEXT")]
        public string BorrowerOtherBusiness { get; set; } = string.Empty;

        [Column("borrower_creditors_terms", TypeName = "LONGTEXT")]
        public string BorrowerCreditorsTerms { get; set; } = string.Empty;

        [Column("borrower_contracts_info", TypeName = "LONGTEXT")]
        public string BorrowerContractsInfo { get; set; } = string.Empty;

        [Column("borrower_related_business", TypeName = "LONGTEXT")]
        public string BorrowerRelatedBusiness { get; set; } = string.Empty;

        // CRB & Compliance
        [Column("crb_check_results", TypeName = "LONGTEXT")]
        public string CrbCheckResultsJson { get; set; } = "[]";

        [Column("policy_compliance", TypeName = "LONGTEXT")]
        public string PolicyComplianceJson { get; set; } = "[]";

        [Column("general_conditions", TypeName = "LONGTEXT")]
        public string GeneralConditionsJson { get; set; } = "[]";

        [Column("other_conditions", TypeName = "LONGTEXT")]
        public string OtherConditionsJson { get; set; } = "[]";

        [Column("covenants", TypeName = "LONGTEXT")]
        public string CovenantsJson { get; set; } = "[]";

        [Column("selected_product", TypeName = "TEXT")]
        public string SelectedProduct { get; set; } = string.Empty;

        // Account Performance
        [Column("bank_accounts", TypeName = "LONGTEXT")]
        public string BankAccountsJson { get; set; } = "[]";

        [Column("average_monthly_turnover", TypeName = "TEXT")]
        public string AverageMonthlyTurnover { get; set; } = string.Empty;

        [Column("average_balance", TypeName = "TEXT")]
        public string AverageBalance { get; set; } = string.Empty;

        [Column("account_performance_notes", TypeName = "LONGTEXT")]
        public string AccountPerformanceNotes { get; set; } = string.Empty;

        // Attachments
        [Column("attachments", TypeName = "LONGTEXT")]
        public string AttachmentsJson { get; set; } = "[]";

        // Photo Sections
        [Column("photo_sections", TypeName = "LONGTEXT")]
        public string PhotoSectionsJson { get; set; } = "[]";

        // Location Data
        [Column("pinned_location", TypeName = "LONGTEXT")]
        public string PinnedLocationJson { get; set; } = "{}";

        [Column("business_latitude")]
        public decimal? BusinessLatitude { get; set; }

        [Column("business_longitude")]
        public decimal? BusinessLongitude { get; set; }

        // Metadata
        [Column("rm_id")]
        public string RmId { get; set; } = string.Empty;

        [Column("rm_name")]
        public string RmName { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}