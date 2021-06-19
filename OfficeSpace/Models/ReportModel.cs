using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class ExpiringLeaseFooterModel
    {
        public decimal TotalMonthlyRentalCost { get; set; }
        public decimal TotalSecurityDeposit { get; set; }

        public decimal TotalBuiltupArea { get; set; }
        public decimal TotalCarpetArea { get; set; }
        public decimal TotalRentalArea { get; set; }
        public static ExpiringLeaseFooterModel DefaultInstance
        {
            get
            {
                return new ExpiringLeaseFooterModel { TotalBuiltupArea = decimal.Zero, TotalCarpetArea = decimal.Zero, TotalRentalArea = decimal.Zero, TotalMonthlyRentalCost = decimal.Zero, TotalSecurityDeposit = decimal.Zero };
            }
        }


    }
    public class LeaseExpiringModel
    {
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Lease Closure Date")]
        public string LeaseClouserDate { get; set; }
        [Display(Name = "Company")]
        public string Company { get; set; }
        [Display(Name = "Total Monthly Rental Cost")]
        public decimal? TotalMonthlyRentalCost { get; set; }
        [Display(Name = "Security Deposit")]
        public decimal? SecurityDeposit { get; set; }
        [Display(Name = "Notice Period")]
        public int NoticePeriod { get; set; }
        [Display(Name = "Builtup Area")]
        public decimal? BuiltUpArea { get; set; }
        [Display(Name = "Carpet Area")]
        public decimal? CarpetArea { get; set; }
        [Display(Name = "Rental Area")]
        public decimal? RentalArea { get; set; }
        public int MonthsToExpire { get; set; }
        public static LeaseExpiringModel EmptyInstance
        {
            get
            {
                return new LeaseExpiringModel
                {
                    City = string.Empty,
                    BuiltUpArea = decimal.Zero,
                    CarpetArea = decimal.Zero,
                    Company = string.Empty,
                    LeaseClouserDate = string.Empty,
                    MonthsToExpire = 0,
                    NoticePeriod = 0,
                    RentalArea = decimal.Zero,
                    SecurityDeposit = decimal.Zero,
                    TotalMonthlyRentalCost = decimal.Zero
                };
            }
        }

    }
    public class ExpiringLeaseSearchModel
    {
        public List<string> Cities { get; set; }
        public string SelectedCity { get; set; }
        public List<ExpiringPeriod> ExpiringPeriods { get; set; }
        public string SelectedPeriod { get; set; }
        public static ExpiringLeaseSearchModel DefaultInstance
        {
            get
            {
                return new ExpiringLeaseSearchModel { Cities = new List<string>(), ExpiringPeriods = new List<ExpiringPeriod>(), SelectedCity = string.Empty, SelectedPeriod = string.Empty };
            }
        }
    }
    public class LeaseReportDetailsData
    {
        public List<LeaseExpiringModel> ExpiringLeaseList { get; set; }
        public ExpiringLeaseFooterModel Footer { get; set; }
    }
    public class LeaseReportData
    {
        public List<LeaseExpiringMasterModel> ExpiringLeaseList { get; set; }
        public ExpiringLeaseFooterModel Footer { get; set; }
    }
    public class LeaseExpiringReportViewModel
    {
        public LeaseExpiringModel HeaderModel { get { return LeaseExpiringModel.EmptyInstance; } }
        public List<LeaseExpiringModel> ExpiringLeaseList { get; set; }
        public ExpiringLeaseSearchModel SearchParams { get; set; }
        public ExpiringLeaseFooterModel Footer { get; set; }
        public static LeaseExpiringReportViewModel DefaultInstance
        {
            get
            {
                return new LeaseExpiringReportViewModel { ExpiringLeaseList = new List<LeaseExpiringModel>(), Footer = ExpiringLeaseFooterModel.DefaultInstance, SearchParams = ExpiringLeaseSearchModel.DefaultInstance };
            }
        }

    }
    public class LeaseExpiringReportViewDetailModel
    {
        public LeaseExpiringMasterModel HeaderModel { get { return LeaseExpiringMasterModel.EmptyInstance; } }
        public List<LeaseExpiringMasterModel> ExpiringLeaseList { get; set; }
        public ExpiringLeaseSearchModel SearchParams { get; set; }
        public ExpiringLeaseFooterModel Footer { get; set; }
        public static LeaseExpiringReportViewDetailModel DefaultInstance
        {
            get
            {
                return new LeaseExpiringReportViewDetailModel { ExpiringLeaseList = new List<LeaseExpiringMasterModel>(), Footer = ExpiringLeaseFooterModel.DefaultInstance, SearchParams = ExpiringLeaseSearchModel.DefaultInstance };
            }
        }

    }
    public class LeaseExpiringMasterModel
    {
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Lease Closure Date")]
        public decimal? TotalMonthlyRentalCost { get; set; }
        [Display(Name = "Security Deposit")]
        public decimal? SecurityDeposit { get; set; }
        [Display(Name = "Notice Period")]
        public int NoticePeriod { get; set; }
        [Display(Name = "Builtup Area")]
        public decimal? BuiltUpArea { get; set; }
        [Display(Name = "Carpet Area")]
        public decimal? CarpetArea { get; set; }
        [Display(Name = "Rental Area")]
        public decimal? RentalArea { get; set; }

        public List<LeaseExpiringDetailModel> Details { get; set; }
        public static LeaseExpiringMasterModel EmptyInstance
        {
            get
            {
                return new LeaseExpiringMasterModel
                {
                    City = string.Empty,
                    BuiltUpArea = decimal.Zero,
                    CarpetArea = decimal.Zero,
                    NoticePeriod = 0,
                    RentalArea = decimal.Zero,
                    SecurityDeposit = decimal.Zero,
                    TotalMonthlyRentalCost = decimal.Zero,
                    Details = new List<LeaseExpiringDetailModel>()
                };
            }
        }

    }
    public class LeaseExpiringDetailModel
    {
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Lease Closure Date")]
        public string LeaseClouserDate { get; set; }
        [Display(Name = "Company")]
        public string Company { get; set; }
        [Display(Name = "Total Monthly Rental Cost")]
        public decimal? TotalMonthlyRentalCost { get; set; }
        [Display(Name = "Security Deposit")]
        public decimal? SecurityDeposit { get; set; }
        [Display(Name = "Notice Period")]
        public int NoticePeriod { get; set; }
        [Display(Name = "Builtup Area")]
        public decimal? BuiltUpArea { get; set; }
        [Display(Name = "Carpet Area")]
        public decimal? CarpetArea { get; set; }
        [Display(Name = "Rental Area")]
        public decimal? RentalArea { get; set; }
        public int MonthsToExpire { get; set; }
        public static LeaseExpiringDetailModel EmptyInstance
        {
            get
            {
                return new LeaseExpiringDetailModel
                {
                    City = string.Empty,
                    BuiltUpArea = decimal.Zero,
                    CarpetArea = decimal.Zero,
                    Company = string.Empty,
                    LeaseClouserDate = string.Empty,
                    MonthsToExpire = 0,
                    NoticePeriod = 0,
                    RentalArea = decimal.Zero,
                    SecurityDeposit = decimal.Zero,
                    TotalMonthlyRentalCost = decimal.Zero
                };
            }
        }

    }
    public class ExpiringPeriod
    {
        public int ExpiringPeriodValue { get; set; }
        public string ExpiringPeriodText { get; set; }
    }
}