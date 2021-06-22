using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class RequestDetails
    {
        public string Company { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
    }
    public class SearchApprovalRequestModel
    {


        public List<string> CompanyList { get; set; }
        public List<string> CityList { get; set; }
        public List<string> StatusList { get; set; }
        public string Status { get; set; }

        public string FromDate { get; set; }

        public string UserName { get; set; }
        public string ToDate { get; set; }
        public string City { get; set; }
        public string Company { get; set; }

      
        public static SearchApprovalRequestModel EmptyInstance
        {
            get
            {
                return new SearchApprovalRequestModel()
                {

                    Company = "All",
                    City = "All",
                    Status = "All",
                    FromDate = System.DateTime.Now.AddDays(-30).ToString("dd MMM yyyy"),
                    ToDate = System.DateTime.Now.ToString("dd MMM yyyy"),
                    CompanyList = new List<string>(),
                    CityList = new List<string>(),
                    StatusList = new List<string>()

                };
            }
        }
    }

    public class RequestViewModel
    {
        public SearchApprovalRequestModel SearchParam { get; set; }
        public List<RequestModel> ApprovalRequests { get; set; }
        public static RequestViewModel EmptyInstance
        {
            get
            {
                return new RequestViewModel()
                {
                    SearchParam=SearchApprovalRequestModel.EmptyInstance,
                    ApprovalRequests=new List<RequestModel>()
                };
            }
        }

    }




    public class RequestModel
    {
        public string SelectedStatus { get; set; }

        public List<RequestModel> FurnishedDataList { get; set; }
        public string DataType { get; set; }
        public string RequestID { get; set; }
        public string CreationDate { get; set; }
        public string BussinessType { get; set; }
        public string MenuSelection { get; set; }

        public List<string> CompanyList { get; set; }
        public List<string> CityList { get; set; }

        public List<string> StatusList { get; set; }

        public string Company { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }

        public string BuisnessType { get; set; }
        public string AllocationType { get; set; }
        [Required(ErrorMessage = "Please select the City from the list")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Property Type")]
        public string Fitouts { get; set; }
        [Required]
        [Display(Name = "Lease Start Date")]
        public string DateFromWhich { get; set; }
        [Required]
        public string ProposedLocation { get; set; }
        [Required]
        public string ProposedSignage { get; set; }
        [Required]
        public string ProposedNoOfPersons { get; set; }
        [Required]
        public string ProposedSuperBuiltUp { get; set; }
        [Required]
        public string ProposedBuiltUp { get; set; }
        [Required]
        public string ProposedCarpetArea { get; set; }
        [Required]
        public string ProposedRentalArea { get; set; }

        public string ProposedCostPerSquareFeet { get; set; }
        [Required]
        public string ProposedSecurityDeposit { get; set; }

        public string ProposedCarPark { get; set; }
        public string ProposedRemarks { get; set; }

        public string LoggedInUser { get; set; }

        public string CalcMonthlyCost { get; set; }

        public string ProposedCalcMonthlyCost { get; set; }

        public string LeaseStartDate { get; set; }

        public string ExistingLeaseStartDate { get; set; }

        public int ID { get; set; }

        public string SelectedMenu { get; set; }
        [Required]
        public string LeaseStartAmount { get; set; }

        public string RentalEscallation { get; set; }

        public string EscallationPeriod { get; set; }

        public string LeasePeriod { get; set; }

        public string AdvanceRental { get; set; }

        public string AmtHoldWithOwner { get; set; }

        public string NoticePeriod { get; set; }

        public string PresentMonthlyRenatlCost { get; set; }
        [Required]
        public string PresentMonthlyBilling { get; set; }

        public string RentalCostPer { get; set; }

        public string MonthlyMaintenenceCost { get; set; }

        public string AvgMaintenanceCost { get; set; }

        public string monthlyElectricCost { get; set; }

        public string MonthlyOtherCosts { get; set; }

        public string TotalMonthlyRenatlCost { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string OfficeName { get; set; }

        public string LeaseClosureDate { get; set; }

        //Below are the paarameters for Existing Details

        public string ExistingLocation { get; set; }
        public string ProposedDateofRenewal { get; set; }
        public string Signage { get; set; }

        public string NoOfPersons { get; set; }

        public string SuperBuiltUp { get; set; }

        public string BuiltUp { get; set; }

        public string CarpetArea { get; set; }

        public string RentalArea { get; set; }

        public string CostPerSquareFeet { get; set; }

        public string SecurityDeposit { get; set; }

        public string Remarks { get; set; }

        public string ExistingLeaseStartAmount { get; set; }

        public string ExistingRentalEscallation { get; set; }

        public string ExistingEscallationPeriod { get; set; }

        public string ExistingLeasePeriod { get; set; }

        public string ExistingLeaseClosureDate { get; set; }

        public string ExistingAdvanceRental { get; set; }

        public string ExistingAmtHoldWithOwner { get; set; }
        public string ExistingNoticePeriod { get; set; }

        public string ExistingPresentMonthlyRenatlCost { get; set; }

        public string ExistingPresentMonthlyBilling { get; set; }
        public string ExistingRentalCostPer { get; set; }

        public string ExistingMonthlyMaintenenceCost { get; set; }

        public string ExistingAvgMaintenanceCost { get; set; }
        public string ExistingmonthlyElectricCost { get; set; }
        public string ExistingMonthlyOtherCosts { get; set; }
        public string ExistingTotalMonthlyRenatlCost { get; set; }
        public string ExistingName { get; set; }
        public string ExistingEmail { get; set; }
        public string ExistingMobile { get; set; }
        public string ExistingOfficeName { get; set; }
        public string ExistingCarPark { get; set; }

        public string Status { get; set; }

        public string SelectedRequestId { get; set; }









        //public void GetCityList()
        //{
        //    CityList = new List<string>();

        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.CommandText = "SELECT City FROM CityMaster ORDER BY City";
        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                CityList.Add(reader[0].ToString());
        //            }
        //        }
        //        reader.Close();
        //    }

        //    //NavigationModel Nmodel = new NavigationModel();
        //    //Nmodel.GetCityList();

        //}

        //public int CreateNewRequest(string CreatedBy)
        //{
        //    int ID = 0;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
        //        {
        //            connection.Open();
        //            SqlCommand command = new SqlCommand();
        //            command.Connection = connection;
        //            command.CommandText = "udp_Store_Post_New_Navigation";
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@Company", Company);
        //            command.Parameters.AddWithValue("@MenuSelection", "New");
        //            command.Parameters.AddWithValue("@BussinessType", BuisnessType);
        //            command.Parameters.AddWithValue("@AllocationType", AllocationType);
        //            command.Parameters.AddWithValue("@City", City);
        //            command.Parameters.AddWithValue("@DateOfRequired", DateFromWhich == null ? string.Empty : DateFromWhich);
        //            command.Parameters.AddWithValue("@Fitouts", Fitouts);
        //            command.Parameters.AddWithValue("@ProposedLocation", ProposedLocation == null ? string.Empty : ProposedLocation);
        //            command.Parameters.AddWithValue("@ProposedSignage", ProposedSignage == null ? string.Empty : ProposedSignage);
        //            command.Parameters.AddWithValue("@ProposedEmployee", ProposedNoOfPersons == null ? string.Empty : ProposedNoOfPersons);
        //            command.Parameters.AddWithValue("@ProposedSuperBuiltUpArea", ProposedSuperBuiltUp == null ? string.Empty : ProposedSuperBuiltUp);
        //            command.Parameters.AddWithValue("@ProposedBuiltupArea", ProposedBuiltUp == null ? string.Empty : ProposedBuiltUp);
        //            command.Parameters.AddWithValue("@ProposedCarpetArea", ProposedCarpetArea == null ? string.Empty : ProposedCarpetArea);
        //            command.Parameters.AddWithValue("@ProposedRentalCost", ProposedCostPerSquareFeet == null ? string.Empty : ProposedCostPerSquareFeet);
        //            command.Parameters.AddWithValue("@ProposedSecurityDeposit", ProposedSecurityDeposit == null ? string.Empty : ProposedSecurityDeposit);
        //            command.Parameters.AddWithValue("@ProposedCarPark", ProposedCarPark);
        //            command.Parameters.AddWithValue("@Remark2", ProposedRemarks == null ? string.Empty : ProposedRemarks);
        //            command.Parameters.AddWithValue("@Status", "Initiated");
        //            command.Parameters.AddWithValue("@ProposedMonthlyCost", (Convert.ToInt32(ProposedRentalArea) * Convert.ToInt32(ProposedCostPerSquareFeet)));
        //            command.Parameters.AddWithValue("@LoggedInUser", CreatedBy == null ? string.Empty : CreatedBy);
        //            ID = int.Parse(command.ExecuteScalar().ToString());

        //        }
        //        return ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public string InitiateOfficeName { get; set; }
        public string InitiateLocation { get; set; }
        public string InitiateLeaseStartDate { get; set; }
        public string InitiateLeaseStartAmount { get; set; }
        public string InitiateRentalEscallation { get; set; }
        public string InitiateEscallationPeriod { get; set; }
        public string InitiateLeasePeriod { get; set; }
        public string InitiateLeaseClousureDate { get; set; }
        public string InitiateLeaseAdvanceRental { get; set; }
        public string InitiateAmtHoldWithOwner { get; set; }
        public string InitiateNoticePeriod { get; set; }
        public string InitiateSignage { get; set; }
        public string InitiateNoOfPerson { get; set; }
        public string InitiateSuperBuiltUp { get; set; }
        public string InitiateBuiltup { get; set; }
        public string InitiateCarpetArea { get; set; }

        public string InitiateRentalArea { get; set; }
        public string InitiateCalcMonthlyCost { get; set; }
        public string InitiateSecurityDeposit { get; set; }
        public string InitiateMonthlyRentalCost { get; set; }
        public string InitiateMonthlyBilling { get; set; }
        public string InitiateRentalCostPer { get; set; }
                  public string InitiateRentalCost { get; set; }
        public string InitiatemonthlyMaintenanceCost { get; set; }
        public string InitiateAvgMaintenanceCost { get; set; }
        public string InitiateMonthlyElectricCost { get; set; }
        public string InitiateMonthlyOtherCosts { get; set; }
        public string InitiateTotalMonthlyRenatlCost { get; set; }
        public string InitiateCarpark { get; set; }
        public string InitiateName { get; set; }
        public string InitiateEmail { get; set; }
        public string InitiateMobile { get; set; }
        public string InitiateRemarks { get; set; }

        public string InitiateStatus { get; set; }
       

        public string IdentifiedOfficeName { get; set; }
        public string IdentifiedLocation { get; set; }
        public string IdentifiedLeaseStartDate { get; set; }
        public string IdentifiedLeaseStartAmount { get; set; }
        public string IdentifiedRentalEscallation { get; set; }
        public string IdentifiedEscallationPeriod { get; set; }
        public string IdentifiedLeasePeriod { get; set; }
        public string IdentifiedLeaseClousureDate { get; set; }
        public string IdentifiedLeaseAdvanceRental { get; set; }
        public string IdentifiedAmtHoldWithOwner { get; set; }
        public string IdentifiedNoticePeriod { get; set; }
        public string IdentifiedSignage { get; set; }
        public string IdentifiedNoOfPerson { get; set; }
        public string IdentifiedSuperBuiltUp { get; set; }
        public string IdentifiedBuiltup { get; set; }
        public string IdentifiedCarpetArea { get; set; }

        public string IdentifiedRentalArea { get; set; }
        public string IdentifiedCalcMonthlyCost { get; set; }
        public string IdentifiedSecurityDeposit { get; set; }
        public string IdentifiedMonthlyRentalCost { get; set; }
        public string IdentifiedMonthlyBilling { get; set; }
        public string IdentifiedRentalCostPer { get; set; }

        public string SelectedIdentifyStatus { get; set; }

        public string IdentifiedRentalCost { get; set; }
        public string IdentifiedmonthlyMaintenanceCost { get; set; }
        public string IdentifiedAvgMaintenanceCost { get; set; }
        public string IdentifiedMonthlyElectricCost { get; set; }
        public string IdentifiedMonthlyOtherCosts { get; set; }
        public string IdentifiedTotalMonthlyRenatlCost { get; set; }
        public string IdentifiedCarpark { get; set; }
        public string IdentifiedName { get; set; }
        public string IdentifiedEmail { get; set; }
        public string IdentifiedMobile { get; set; }
        public string IdentifiedRemarks { get; set; }


    }
}