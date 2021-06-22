using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using OfficeSpace.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using OfficeSpace.Repository;

namespace OfficeSpace.Services
{
    public class NavigationRepositoryService : BaseRepository, INavigationRepositoryService
    {
        //public void Init(string roleName, string userName = null)
        //{
        //    //GetCompanyList(roleName, userName);
        //    //GetCityList();
        //    //GetBranchList(City);
        //}

        public NavigationRepositoryService(ILogService logService) : base(logService)
        {

        }

        public List<string> GetCompanyList(string roleName, string userName)
        {
            var result = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (roleName == "User")
                {
                    //command.CommandText = @"SELECT CompanyMaster.CompanyName FROM UserDetails JOIN CompanyMaster 
                    //                        ON CompanyMaster.CompanyName = UserDetails.CompanyName
                    //                        WHERE  RoleName = 'User' and UserName = '" + userName + "'";
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "'  and  RoleName = 'User') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                else
                {
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return result;
        }

        public List<string> GetCityList()
        {
            var result = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT City FROM CityMaster ORDER BY City";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return result;
        }

        public List<string> GetBranchList(string City)
        {
            var BranchList = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT distinct OfficeName FROM [FurnishedRentalDetails] ORDER BY OfficeName";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BranchList.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return BranchList;
        }

        public void GetExistingData(RequestModel Model)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "udp_Store_Fetch_ExistingOfficeData";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Company", Model.Company);
                command.Parameters.AddWithValue("@Region", Model.Region);
                command.Parameters.AddWithValue("@Branch", Model.Branch);
                //  command.CommandText = "SELECT * FROM FurnishedRentalDetails WHERE Company='" + Model.Company + "' and [OfficeName]='" + Model.Branch + "'";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.City = reader["City"].ToString();
                        Model.ProposedDateofRenewal = reader["LeaseClouserDate"].ToString();
                        Model.ExistingLocation = reader["Address"].ToString();
                        Model.ExistingLeaseStartAmount = reader["LeaseStartingAmount"].ToString();
                        Model.ExistingRentalEscallation = reader["RentalEscallation"].ToString();
                        Model.ExistingEscallationPeriod = reader["EscallationPeriod"].ToString();
                        Model.ExistingLeasePeriod = reader["LeasePeriod"].ToString();
                        Model.ExistingLeaseClosureDate = reader["LeaseClouserDate"].ToString();
                        Model.ExistingAdvanceRental = reader["AdvanceRental"].ToString();
                        Model.ExistingAmtHoldWithOwner = reader["TotalAmountHoldWithOwner"].ToString();
                        Model.ExistingNoticePeriod = reader["NoticePeriod"].ToString();
                        Model.Signage = reader["Signage"].ToString();
                        Model.NoOfPersons = reader["NoOfEmployee"].ToString();
                        Model.SuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                        Model.BuiltUp = reader["BuiltUpArea"].ToString();
                        Model.CarpetArea = reader["CarpetArea"].ToString();
                        Model.RentalArea = reader["RentalArea"].ToString();
                        Model.CostPerSquareFeet = reader["PresentRentalCost"].ToString();
                        Model.SecurityDeposit = reader["SecurityDeposit"].ToString();
                        Model.ExistingPresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                        Model.ExistingPresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                        Model.ExistingRentalCostPer = reader["RenatlCostPercentage"].ToString();
                        Model.ExistingMonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                        Model.ExistingAvgMaintenanceCost = reader["AvgMonthltMaintenanceCost"].ToString();
                        Model.ExistingmonthlyElectricCost = reader["MonthlyElectricityCost"].ToString();
                        Model.ExistingMonthlyOtherCosts = reader["MonthlyAllOtherCosts"].ToString();
                        Model.ExistingTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                        Model.ExistingCarPark = reader["NoOfCarParking"].ToString();
                        Model.ExistingName = reader["Name"].ToString();
                        Model.ExistingEmail = reader["Email"].ToString();
                        Model.ExistingMobile = reader["Mobile"].ToString();
                        Model.ExistingOfficeName = reader["OfficeName"].ToString();
                        Model.Remarks = reader["Remarks"].ToString();

                    }
                }
                reader.Close();
            }
        }
        public List<UserDataModel> GetUserRequests()
        {
            var UserDataList = new List<UserDataModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM NavigationDetails WHERE IsMerged = 0 and Status != 'Closed'";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserDataModel obj = new UserDataModel();
                        obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                        obj.Company = reader["Company"].ToString();
                        obj.SelectedMenu = reader["MenuSelection"].ToString();
                        obj.BuisnessType = reader["BussinessType"].ToString();
                        obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                        obj.City = reader["City"].ToString();
                        obj.Fitouts = reader["Fitouts"].ToString();
                        obj.NoOfPersons = int.Parse(reader["NumberOfPersons"].ToString());
                        obj.CostPerSquareFeet = reader["FinalPrice"].ToString();
                        obj.Signage = reader["Signage"].ToString();
                        obj.SuperArea = reader["SuperArea"].ToString();
                        obj.Remarks = reader["Legal"].ToString();
                        obj.CarPark = reader["CarPark"].ToString();
                        obj.AllocationType = reader["AllocationType"].ToString();
                        obj.IsMerged = bool.Parse(reader["IsMerged"].ToString());
                        obj.Status = reader["Status"].ToString();
                        obj.Location = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                        UserDataList.Add(obj);
                    }
                }
                reader.Close();
            }
            return UserDataList;
        }

        public int CreateNewRequest(RequestModel newRequest)
        {
            int ID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = "udp_Store_Post_New_Navigation";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    // command.Parameters.AddWithValue("@Company", newRequest.Company);
                    command.Parameters.AddWithValue("@Company", newRequest.Company);
                    command.Parameters.AddWithValue("@MenuSelection", "New");
                    command.Parameters.AddWithValue("@BussinessType", newRequest.BuisnessType);
                    command.Parameters.AddWithValue("@AllocationType", newRequest.AllocationType);
                    command.Parameters.AddWithValue("@City", newRequest.City);
                    command.Parameters.AddWithValue("@DateOfRequired", newRequest.DateFromWhich == null ? string.Empty : newRequest.DateFromWhich);
                    command.Parameters.AddWithValue("@Fitouts", newRequest.Fitouts);
                    command.Parameters.AddWithValue("@ProposedLocation", newRequest.ProposedLocation == null ? string.Empty : newRequest.ProposedLocation);
                    command.Parameters.AddWithValue("@ProposedSignage", newRequest.ProposedSignage == null ? string.Empty : newRequest.ProposedSignage);
                    command.Parameters.AddWithValue("@ProposedEmployee", newRequest.ProposedNoOfPersons == null ? string.Empty : newRequest.ProposedNoOfPersons);
                    command.Parameters.AddWithValue("@ProposedSuperBuiltUpArea", newRequest.ProposedSuperBuiltUp == null ? string.Empty : newRequest.ProposedSuperBuiltUp);
                    command.Parameters.AddWithValue("@ProposedBuiltupArea", newRequest.ProposedBuiltUp == null ? string.Empty : newRequest.ProposedBuiltUp);
                    command.Parameters.AddWithValue("@ProposedCarpetArea", newRequest.ProposedCarpetArea == null ? string.Empty : newRequest.ProposedCarpetArea);
                    command.Parameters.AddWithValue("@ProposedRentalArea", newRequest.ProposedRentalArea == null ? string.Empty : newRequest.ProposedRentalArea);
                    command.Parameters.AddWithValue("@ProposedRentalCost", newRequest.ProposedCostPerSquareFeet == null ? string.Empty : newRequest.ProposedCostPerSquareFeet);
                    command.Parameters.AddWithValue("@ProposedSecurityDeposit", newRequest.ProposedSecurityDeposit == null ? string.Empty : newRequest.ProposedSecurityDeposit);
                    command.Parameters.AddWithValue("@ProposedCarPark", newRequest.ProposedCarPark);

                    command.Parameters.AddWithValue("@Remark2", newRequest.ProposedRemarks == null ? string.Empty : newRequest.ProposedRemarks);
                    command.Parameters.AddWithValue("@Status", "Fresh");
                    command.Parameters.AddWithValue("@ProposedMonthlyCost", (Convert.ToInt32(newRequest.ProposedRentalArea) * Convert.ToInt32(newRequest.ProposedCostPerSquareFeet)));
                    command.Parameters.AddWithValue("@LoggedInUser", newRequest.LoggedInUser == null ? string.Empty : newRequest.LoggedInUser);


                    command.Parameters.AddWithValue("@LeaseStartAmount", newRequest.LeaseStartAmount == null ? string.Empty : newRequest.LeaseStartAmount);
                    command.Parameters.AddWithValue("@RentalEscallation", newRequest.RentalEscallation == null ? string.Empty : newRequest.RentalEscallation);
                    command.Parameters.AddWithValue("@EscallationPeriod", newRequest.EscallationPeriod == null ? string.Empty : newRequest.EscallationPeriod);
                    command.Parameters.AddWithValue("@LeasePeriod", newRequest.LeasePeriod == null ? string.Empty : newRequest.LeasePeriod);
                    command.Parameters.AddWithValue("@LeaseClosureDate", newRequest.LeaseClosureDate == null ? string.Empty : newRequest.LeaseClosureDate);
                    command.Parameters.AddWithValue("@AdvanceRental", newRequest.AdvanceRental == null ? string.Empty : newRequest.AdvanceRental);
                    command.Parameters.AddWithValue("@AmtHoldWithOwner", newRequest.AmtHoldWithOwner == null ? string.Empty : newRequest.AmtHoldWithOwner);
                    command.Parameters.AddWithValue("@NoticePeriod", newRequest.NoticePeriod == null ? string.Empty : newRequest.NoticePeriod);
                    command.Parameters.AddWithValue("@PresentMonthlyRenatlCost", newRequest.PresentMonthlyRenatlCost == null ? string.Empty : newRequest.PresentMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@PresentMonthlyBilling", newRequest.PresentMonthlyBilling == null ? string.Empty : newRequest.PresentMonthlyBilling);
                    command.Parameters.AddWithValue("@RentalCostPer", newRequest.RentalCostPer == null ? string.Empty : newRequest.RentalCostPer);
                    command.Parameters.AddWithValue("@MonthlyMaintenenceCost", newRequest.MonthlyMaintenenceCost == null ? string.Empty : newRequest.MonthlyMaintenenceCost);
                    command.Parameters.AddWithValue("@AvgMaintenanceCost", newRequest.AvgMaintenanceCost == null ? string.Empty : newRequest.AvgMaintenanceCost);
                    command.Parameters.AddWithValue("@monthlyElectricCost", newRequest.monthlyElectricCost == null ? string.Empty : newRequest.monthlyElectricCost);
                    command.Parameters.AddWithValue("@MonthlyOtherCosts", newRequest.MonthlyOtherCosts == null ? string.Empty : newRequest.MonthlyOtherCosts);

                    command.Parameters.AddWithValue("@TotalMonthlyRenatlCost", newRequest.TotalMonthlyRenatlCost == null ? string.Empty : newRequest.TotalMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@Name", newRequest.Name == null ? string.Empty : newRequest.Name);
                    command.Parameters.AddWithValue("@Email", newRequest.Email == null ? string.Empty : newRequest.Email);
                    command.Parameters.AddWithValue("@Mobile", newRequest.Mobile == null ? string.Empty : newRequest.Mobile);
                    command.Parameters.AddWithValue("@OfficeName", newRequest.OfficeName == null ? string.Empty : newRequest.OfficeName);


                    ID = int.Parse(command.ExecuteScalar().ToString());

                }
                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateRelocationRequest(RequestModel newRequest)
        {
            int ID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = "udp_Store_Post_RelocationRequest";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    // command.Parameters.AddWithValue("@Company", newRequest.Company);
                    command.Parameters.AddWithValue("@Company", newRequest.Company);
                    command.Parameters.AddWithValue("@MenuSelection", "Relocation");
                    command.Parameters.AddWithValue("@BussinessType", newRequest.BuisnessType);
                    command.Parameters.AddWithValue("@AllocationType", newRequest.AllocationType);
                    command.Parameters.AddWithValue("@City", newRequest.City);
                    command.Parameters.AddWithValue("@DateOfRequired", newRequest.DateFromWhich == null ? string.Empty : newRequest.DateFromWhich);
                    command.Parameters.AddWithValue("@Fitouts", newRequest.Fitouts);
                    command.Parameters.AddWithValue("@ProposedLocation", newRequest.ProposedLocation == null ? string.Empty : newRequest.ProposedLocation);
                    command.Parameters.AddWithValue("@ProposedSignage", newRequest.ProposedSignage == null ? string.Empty : newRequest.ProposedSignage);
                    command.Parameters.AddWithValue("@ProposedEmployee", newRequest.ProposedNoOfPersons == null ? string.Empty : newRequest.ProposedNoOfPersons);
                    command.Parameters.AddWithValue("@ProposedSuperBuiltUpArea", newRequest.ProposedSuperBuiltUp == null ? string.Empty : newRequest.ProposedSuperBuiltUp);
                    command.Parameters.AddWithValue("@ProposedBuiltupArea", newRequest.ProposedBuiltUp == null ? string.Empty : newRequest.ProposedBuiltUp);
                    command.Parameters.AddWithValue("@ProposedCarpetArea", newRequest.ProposedCarpetArea == null ? string.Empty : newRequest.ProposedCarpetArea);
                    command.Parameters.AddWithValue("@ProposedRentalArea", newRequest.ProposedRentalArea == null ? string.Empty : newRequest.ProposedRentalArea);
                    command.Parameters.AddWithValue("@ProposedRentalCost", newRequest.ProposedCostPerSquareFeet == null ? string.Empty : newRequest.ProposedCostPerSquareFeet);
                    command.Parameters.AddWithValue("@ProposedSecurityDeposit", newRequest.ProposedSecurityDeposit == null ? string.Empty : newRequest.ProposedSecurityDeposit);
                    command.Parameters.AddWithValue("@ProposedCarPark", newRequest.ProposedCarPark);
                    command.Parameters.AddWithValue("@Remark2", newRequest.ProposedRemarks == null ? string.Empty : newRequest.ProposedRemarks);
                    command.Parameters.AddWithValue("@Status", "Fresh");
                    command.Parameters.AddWithValue("@ProposedMonthlyCost", (Convert.ToInt32(newRequest.ProposedRentalArea) * Convert.ToInt32(newRequest.ProposedCostPerSquareFeet)));
                    command.Parameters.AddWithValue("@LoggedInUser", newRequest.LoggedInUser == null ? string.Empty : newRequest.LoggedInUser);
                    command.Parameters.AddWithValue("@LeaseStartAmount", newRequest.LeaseStartAmount == null ? string.Empty : newRequest.LeaseStartAmount);
                    command.Parameters.AddWithValue("@RentalEscallation", newRequest.RentalEscallation == null ? string.Empty : newRequest.RentalEscallation);
                    command.Parameters.AddWithValue("@EscallationPeriod", newRequest.EscallationPeriod == null ? string.Empty : newRequest.EscallationPeriod);
                    command.Parameters.AddWithValue("@LeasePeriod", newRequest.LeasePeriod == null ? string.Empty : newRequest.LeasePeriod);
                    command.Parameters.AddWithValue("@LeaseClosureDate", newRequest.LeaseClosureDate == null ? string.Empty : newRequest.LeaseClosureDate);
                    command.Parameters.AddWithValue("@AdvanceRental", newRequest.AdvanceRental == null ? string.Empty : newRequest.AdvanceRental);
                    command.Parameters.AddWithValue("@AmtHoldWithOwner", newRequest.AmtHoldWithOwner == null ? string.Empty : newRequest.AmtHoldWithOwner);
                    command.Parameters.AddWithValue("@NoticePeriod", newRequest.NoticePeriod == null ? string.Empty : newRequest.NoticePeriod);
                    command.Parameters.AddWithValue("@PresentMonthlyRenatlCost", newRequest.PresentMonthlyRenatlCost == null ? string.Empty : newRequest.PresentMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@PresentMonthlyBilling", newRequest.PresentMonthlyBilling == null ? string.Empty : newRequest.PresentMonthlyBilling);
                    command.Parameters.AddWithValue("@RentalCostPer", newRequest.RentalCostPer == null ? string.Empty : newRequest.RentalCostPer);
                    command.Parameters.AddWithValue("@MonthlyMaintenenceCost", newRequest.MonthlyMaintenenceCost == null ? string.Empty : newRequest.MonthlyMaintenenceCost);
                    command.Parameters.AddWithValue("@AvgMaintenanceCost", newRequest.AvgMaintenanceCost == null ? string.Empty : newRequest.AvgMaintenanceCost);
                    command.Parameters.AddWithValue("@monthlyElectricCost", newRequest.monthlyElectricCost == null ? string.Empty : newRequest.monthlyElectricCost);
                    command.Parameters.AddWithValue("@MonthlyOtherCosts", newRequest.MonthlyOtherCosts == null ? string.Empty : newRequest.MonthlyOtherCosts);
                    command.Parameters.AddWithValue("@TotalMonthlyRenatlCost", newRequest.TotalMonthlyRenatlCost == null ? string.Empty : newRequest.TotalMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@Name", newRequest.Name == null ? string.Empty : newRequest.Name);
                    command.Parameters.AddWithValue("@Email", newRequest.Email == null ? string.Empty : newRequest.Email);
                    command.Parameters.AddWithValue("@Mobile", newRequest.Mobile == null ? string.Empty : newRequest.Mobile);
                    command.Parameters.AddWithValue("@OfficeName", newRequest.OfficeName == null ? string.Empty : newRequest.OfficeName);
                    command.Parameters.AddWithValue("@Branch", newRequest.ExistingOfficeName);
                    command.Parameters.AddWithValue("@ExistingLocation", newRequest.ExistingLocation);




                    ID = int.Parse(command.ExecuteScalar().ToString());

                }
                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EmailRelocationRequests(RequestModel model, bool IsReqMerged, string name)
        {
            string httpPort = ConfigurationManager.AppSettings["HttpPort"];
            string approveButtonURL = httpPort + "api/email/approvedetails?ID=" + model.ID + "&Name=" + name;
            string disapproveButtonURL = httpPort + "api/email/disapprovedetails?ID=" + model.ID + "&Name=" + name;
            string ReviewButtonUrl = httpPort + "api/email/Reviewdetails?ID=" + model.ID + "&Name=" + name;
            //  GetOtherData(model);
            string emailBody = GetMailBodyRelocation(model);

            if (!IsReqMerged)
            {
                if (model.BuisnessType == "Branch" && model.AllocationType == "Rent")
                {
                    //Email is sent to President for approval/disapproval
                    string emailID = GetEmailId("President", model.Company);
                    emailBody = emailBody + "<br/><b>For any changes please visit the application with your username and password !!<b>";
                    SendEmailWithButton("Relocation Branch Office Requirement for Rent", emailBody, new List<string>() { emailID }, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);

                    //Information Email is sent to all other presidents
                    List<string> EmailIds = GetEmailIdList("President", model.Company);
                    emailBody = emailBody + "<br/><b>This email is just for your information.No Action required on this !!<b>";
                    SendEmail("Relocation Branch Office Requirement for Rent", emailBody, EmailIds, null, true);
                }
                else if (model.BuisnessType.ToUpper() == "Region" && model.AllocationType == "Rent")
                {
                    string emailID = GetEmailId("President", model.Company);
                    SendEmail("Relocation Region Office Requirement for Rent", emailBody, new List<string>() { emailID }, null, true);

                    List<string> EmailIds = GetEmailIdList("CEO");
                    SendEmailWithButton("Relocation Region Office Requirement for Rent", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);
                }
                else if ((model.BuisnessType.ToUpper() == "Branch" && model.AllocationType == "Buy") || (model.BuisnessType.ToUpper() == "Region" && model.AllocationType == "Buy"))
                {
                    //Email sent to the CEO of the company for approval/disapproval 
                    List<string> EmailIds = GetEmailIdList("CEO");
                    SendEmailWithButton("Relocation Office Space Requirement", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);
                }
            }
            else
            {
                List<string> EmailIds = GetEmailIdList("CEO");
                SendEmailWithButton("New Office Space Requirement", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);
            }

        }

        public void EmailNewRequests(RequestModel model, bool IsReqMerged, string name)
        {
            string httpPort = ConfigurationManager.AppSettings["HttpPort"];
            string approveButtonURL = httpPort + "api/email/approvedetails?ID=" + model.ID + "&Name=" + name;
            string disapproveButtonURL = httpPort + "api/email/disapprovedetails?ID=" + model.ID + "&Name=" + name;
            string ReviewButtonUrl = httpPort + "api/email/Reviewedetails?ID=" + model.ID + "&Name=" + name;
            //  GetOtherData(model);
            string emailBody = GetMailBody(model);

            if (!IsReqMerged)
            {
                if (model.BuisnessType == "Branch" && model.AllocationType == "Rent")
                {
                    //Email is sent to President for approval/disapproval
                    string emailID = GetEmailId("President", model.Company);
                    emailBody = emailBody + "<br/><b>For any changes please visit the application with your username and password !!<b>";
                    SendEmailWithButton("New Branch Office Requirement for Rent", emailBody, new List<string>() { emailID }, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);

                    //Information Email is sent to all other presidents
                    List<string> EmailIds = GetEmailIdList("President", model.Company);
                    emailBody = emailBody + "<br/><b>This email is just for your information.No Action required on this !!<b>";
                    SendEmail("New Branch Office Requirement for Rent", emailBody, EmailIds, null, true);
                }
                else if (model.BuisnessType.ToUpper() == "Region" && model.AllocationType == "Rent")
                {
                    string emailID = GetEmailId("President", model.Company);
                    SendEmail("New Region Office Requirement for Rent", emailBody, new List<string>() { emailID }, null, true);

                    List<string> EmailIds = GetEmailIdList("CEO");
                    SendEmailWithButton("New Region Office Requirement for Rent", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);
                }
                else if ((model.BuisnessType.ToUpper() == "Branch" && model.AllocationType == "Buy") || (model.BuisnessType.ToUpper() == "Region" && model.AllocationType == "Buy"))
                {
                    //Email sent to the CEO of the company for approval/disapproval 
                    List<string> EmailIds = GetEmailIdList("CEO");
                    SendEmailWithButton("New Office Space Requirement", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);
                }
            }
            else
            {
                List<string> EmailIds = GetEmailIdList("CEO");
                SendEmailWithButton("New Office Space Requirement", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL, ReviewButtonUrl);
            }

        }

        public void EmailInitiateRequests(RequestModel model, bool IsReqMerged, string name)
        {
            string httpPort = ConfigurationManager.AppSettings["HttpPort"];
            string AllocationToRHRButtonURL = httpPort + "api/email/approvedetails?ID=" + model.ID + "&Name=" + name;
            string AllocationToSISAMCButtonURL = httpPort + "api/email/disapprovedetails?ID=" + model.ID + "&Name=" + name;
            string emailBody = GetMailBodyInitiate(model);
            string emailID = GetEmailId("SISAMCPresident","SISAMC");
            //emailBody = emailBody + "<br/><b>For any changes please visit the application with your username and password !!<b>";
            SendEmailWithButtonInitiate("New Initiated Request By President", emailBody, new List<string>() { emailID }, true, AllocationToRHRButtonURL, AllocationToSISAMCButtonURL);
        }

        public void SendEmail(String subject, string body, List<string> recipients, List<string> ccEmails, bool isBodyHtml)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = ConfigurationManager.AppSettings["SMTPHost"];
                    client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EnquiryEmailAddress"], ConfigurationManager.AppSettings["EnquiryEmailPassword"]);
                    client.EnableSsl = true;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["EnquiryEmailAddress"]);
                    if (recipients.Count != 0)
                    {
                        mail.Subject = subject;

                        AlternateView alternativeView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                        mail.AlternateViews.Add(alternativeView);
                        mail.IsBodyHtml = isBodyHtml;


                        foreach (string email in recipients)
                        {
                            mail.To.Add(email);
                            if (mail.To.Count > 0)
                            {
                                client.Send(mail);
                            }
                            mail.To.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendEmailWithButton(String subject, string body, List<string> recipients, bool isBodyHtml, string approveButtonUrl, string disapproveButtonUrl,string ReviewButtonUrl)
        {
            StringBuilder addButtons = new StringBuilder();
            addButtons.AppendFormat("<br><br>");
            addButtons.AppendFormat("<button>&nbsp;<a href=\"" + approveButtonUrl + "\" style=\"color:#4CAF50;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Initiate Request</a></button>&nbsp;&nbsp;");
            addButtons.AppendFormat("<button>&nbsp;<a href=\"" + disapproveButtonUrl + "\" style=\"color:#f44336;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Reject Request</a></button>&nbsp;&nbsp;");
            addButtons.AppendFormat("<button>&nbsp;<a href=\"" + ReviewButtonUrl + "\" style=\"color:#4CAF50;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Review Request</a></button>");
            addButtons.AppendFormat("</body></html>");
            body = body.Replace("</body></html>", addButtons.ToString());

            SendEmail(subject, body, recipients, null, isBodyHtml);
        }

        public void SendEmailWithButtonInitiate(String subject, string body, List<string> recipients, bool isBodyHtml, string AllocationToRHRButtonURL, string AllocationToSISAMCButtonURL)
        {
            StringBuilder addButtons = new StringBuilder();
            addButtons.AppendFormat("<br><br>");
            addButtons.AppendFormat("<button>&nbsp;<a href=\"" + AllocationToRHRButtonURL + "\" style=\"color:#4CAF50;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Allocate to RHR</a></button>&nbsp;&nbsp;");
            addButtons.AppendFormat("<button>&nbsp;<a href=\"" + AllocationToSISAMCButtonURL + "\" style=\"color:#f44336;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Allocate to SIS AMC</a></button>&nbsp;&nbsp;");
           
            addButtons.AppendFormat("</body></html>");
            body = body.Replace("</body></html>", addButtons.ToString());

            SendEmail(subject, body, recipients, null, isBodyHtml);
        }


        private string GetMailBody(RequestModel model)
        {

            StringBuilder bodyText = new StringBuilder(string.Empty);
            bodyText.AppendFormat("<html><body>");
            bodyText.AppendFormat("<p>Dear Sir/Mam,</p>");
            bodyText.AppendFormat("<p>New request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("<table border = " + 1 + " cellpadding = " + 0 + " cellspacing = " + 0 + " width = " + 1000 + " >");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Company Name : " + model.Company + " </td><td width = " + 300 + ">Request Type : " + "New " + "</td><td width = " + 300 + ">Bussinss Type : " + model.BuisnessType + " </td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Allocation Type : " + model.AllocationType + " </td><td width = " + 300 + ">City : " + model.City + "</td><td width = " + 300 + ">Date from which property required : " + model.DateFromWhich + "</td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Property Type : " + model.Fitouts + "</td><td width = " + 300 + ">Creation Date  : " + System.DateTime.Today.ToShortDateString() + "</td><td width = " + 300 + ">Created By Name/Email/Phone : " + model.Name + "/" + model.Email + "/" + model.Mobile + "</td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#08F9F5'><td>OTHER DETAILS </td><td>PROPOSED </td><td>OBSERVATIONS OF SIS STORE  </td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Location : </td><td> " + model.ProposedLocation + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Start Amount (In INR) : </td><td> " + model.LeaseStartAmount + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Escallation : </td><td> " + model.RentalEscallation + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Escallation Period : </td><td> " + model.EscallationPeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Period : </td><td> " + model.LeasePeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Closure Date : </td><td> " + model.LeaseClosureDate + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Advance Rental (In INR) : </td><td> " + model.AdvanceRental + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Amount Hold with Owner (In INR) : </td><td> " + model.AmtHoldWithOwner + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Notice Period : </td><td> " + model.NoticePeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Road Name for Signage : </td><td> " + model.ProposedSignage + " </td><td></td></tr>");



            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Employee : </td><td> " + model.ProposedNoOfPersons + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Super Built Up Area (In Sq. Ft.) : </td><td> " + model.ProposedSuperBuiltUp + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Built Up Area (In Sq. Ft.) : </td><td> " + model.ProposedBuiltUp + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Carpet Area (In Sq. Ft.) : </td><td> " + model.ProposedCarpetArea + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Area (In Sq. Ft.) : </td><td> " + model.ProposedRentalArea + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental cost/square feet (In INR) : </td><td> " + model.ProposedCostPerSquareFeet + " </td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.ExistingMonthlyCost + " </td><td> " + model.ProposedMonthlyCost + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + (Convert.ToInt32(model.ProposedRentalArea) * Convert.ToInt32(model.ProposedCostPerSquareFeet)) + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Security Deposit (In INR) : </td><td> " + model.ProposedSecurityDeposit + " </td><td></td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Present Monthly Rental Cost (In INR) : </td><td> " + model.PresentMonthlyRenatlCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Present Monthly Billing (In INR) : </td><td> " + model.PresentMonthlyBilling + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Cost Percentage (In INR) : </td><td> " + model.RentalCostPer + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly Maintenance Cost (In INR) : </td><td> " + model.MonthlyMaintenenceCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Average Maintenance Cost (In INR) : </td><td> " + model.AvgMaintenanceCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly Electric Cost (In INR) : </td><td> " + model.monthlyElectricCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly All Other Costs (In INR) : </td><td> " + model.MonthlyOtherCosts + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.TotalMonthlyRenatlCost + " </td><td></td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Car Park : </td><td> " + model.ProposedCarPark + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Office Name : </td><td> " + model.OfficeName + " </td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Requested By : </td><td> " + model.RequestedBy + " </td><td> " + model.ProposedRequestedBy + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Remarks :  </td><td> " + model.ProposedRemarks + " </td><td></td></tr></table>");
            //bodyText.AppendFormat("<p>New request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("</body></html>");

            return bodyText.ToString();
        }

        public List<string> GetEmailIdList(string designation, string companyName = null)
        {
            List<string> companyEmailIds = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (!string.IsNullOrEmpty(companyName))
                {
                    command.CommandText = string.Format("SELECT EmailID FROM DesignationNEmailID WHERE Designation = '{0}' and CompanyName != '{1}'", designation, companyName);
                }
                else
                {
                    command.CommandText = string.Format("SELECT EmailID FROM DesignationNEmailID WHERE Designation = '{0}'", designation);
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        companyEmailIds.Add(reader[0].ToString());
                    }
                }
                reader.Close();

            }

            return companyEmailIds;
        }

        public string GetEmailId(string designation, string companyName)
        {
            string emailID = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT EmailID FROM DesignationNEmailID WHERE Designation = '{0}' and CompanyName = '{1}'", designation, companyName);
                emailID = command.ExecuteScalar().ToString();
            }

            return emailID;
        }

        public void ApprovedEmail(string name, int ID)
        {
            RequestModel model = GetData(ID, name);
            string emailBody = GetMailBody(model);
            //if approved email sent to karan + gaosain + GMD + procurement    
            List<string> EmailIds = GetEmailIdList("Procurement");
            SendEmail("New office space requirement is approved", emailBody, EmailIds, null, true);
        }

        public void Disapproved(string name, int ID)
        {
            RequestModel model = GetData(ID, name);

            string emailBody = GetMailBody(model);
            //if disapproved then information email sent to all other presidents 
            List<string> EmailIds = GetEmailIdList("President");
            SendEmail("New office space requirement is disapproved", emailBody, EmailIds, null, true);

            //Request will be closed
            CloseRequest(ID, name);
        }

        private void CloseRequest(int ID, string name)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    if (name == "MR")
                    {
                        command.CommandText = @"UPDATE MergeRequests SET Status = 'Disapproved',IsClosed = 1 WHERE MergeAutoID =" + ID;
                    }
                    else
                    {
                        command.CommandText = @"UPDATE NavigationDetailsNew SET Status = 'Disapproved',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string>  GetStatus( string Username)
        {
            var result = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_GetStatus";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", Username);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return result;
        }

        private RequestModel GetData(int ID, string name)
        {
            RequestModel obj = new RequestModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    string updateQuery = string.Empty;

                    if (name == "MR")
                    {
                        command.CommandText = @"SELECT Company,MenuSelection,BussinessType,SecurityDeposit,City,Location,Fitouts,NumberOfPersons,FinalPrice,Signage,SuperArea,Legal,CarPark,'BUY' as AllocationType
                                                FROM MergedRequests WHERE MergeAutoID =" + ID;
                        updateQuery = @"UPDATE MergedRequests SET Status = 'Approved' WHERE MergeAutoID =" + ID;
                    }
                    else
                    {
                        command.CommandText = "udp_Store_Fetch_RequestDetails";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", ID);

                        if (name == "NDI")
                        {
                            updateQuery = @"UPDATE NavigationDetailsNew SET Status = 'Initiated',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                        }
                        if (name == "NDR")
                        {
                            updateQuery = @"UPDATE NavigationDetailsNew SET Status = 'Reviewed',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                        }
                        else
                        {
                            updateQuery = @"UPDATE NavigationDetailsNew SET Status = 'Approved',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                        }
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["ExistingLocation"].ToString();
                            obj.ProposedLocation = reader["ProposedLocation"].ToString();


                            obj.ExistingLeaseStartAmount = reader["ExistingLeaseStartAmount"].ToString();
                            obj.LeaseStartAmount = reader["ProposedLeaseStartAmount"].ToString();
                            obj.ExistingRentalEscallation = reader["ExistingRentalEscallation"].ToString();
                            obj.RentalEscallation = reader["ProposedRentalEscallation"].ToString();
                            obj.ExistingEscallationPeriod = reader["ExistingEscallationPeriod"].ToString();
                            obj.EscallationPeriod = reader["ProposedEscallationPeriod"].ToString();
                            obj.ExistingLeasePeriod = reader["ExistingLeasePeriod"].ToString();
                            obj.LeasePeriod = reader["ProposedLeasePeriod"].ToString();
                            // obj.ExistingLeaseClosureDate = reader["[LeaseRenewalDate]"].ToString();
                            obj.LeaseClosureDate = reader["LeaseRenewalDate"].ToString();
                            obj.ExistingAdvanceRental = reader["ExistingAdvanceRental"].ToString();
                            obj.AdvanceRental = reader["ProposedAdvanceRental"].ToString();
                            obj.ExistingAmtHoldWithOwner = reader["ExistingAmountHoldWithOwner"].ToString();
                            obj.AmtHoldWithOwner = reader["ProposedAmountHoldWithOwner"].ToString();
                            obj.ExistingNoticePeriod = reader["ExistingNoticePeriod"].ToString();
                            obj.NoticePeriod = reader["ProposedNoticePeriod"].ToString();


                            obj.Signage = reader["ExistingSignage"].ToString();
                            obj.ProposedSignage = reader["ProposedSignage"].ToString();
                            obj.NoOfPersons = reader["ExistingEmployee"].ToString();
                            obj.ProposedNoOfPersons = reader["ProposedEmployee"].ToString();
                            obj.SuperBuiltUp = reader["ExistingSuperBuiltUpArea"].ToString();
                            obj.ProposedSuperBuiltUp = reader["ProposedSuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["ExistingBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["ProposedBuiltupArea"].ToString();
                            obj.CarpetArea = reader["ExistingCarpetArea"].ToString();
                            obj.ProposedCarpetArea = reader["ProposedCarpetArea"].ToString();
                            obj.RentalArea = reader["ExistingRentalArea"].ToString();
                            obj.ProposedRentalArea = reader["ProposedRentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["ExistingRentalCost"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["ProposedRentalCost"].ToString();
                            obj.SecurityDeposit = reader["ExistingSecurityDeposit"].ToString();
                            obj.ProposedSecurityDeposit = reader["ProposedSecurityDeposit"].ToString();

                            obj.ExistingPresentMonthlyRenatlCost = reader["ExistingPresentMonthlyRentalCost"].ToString();
                            obj.PresentMonthlyRenatlCost = reader["ProposedPresentMonthlyRentalCost"].ToString();
                            obj.ExistingPresentMonthlyBilling = reader["ExistingPresentMonthlyBilling"].ToString();
                            obj.PresentMonthlyBilling = reader["ProposedPresentMonthlyBilling"].ToString();
                            obj.ExistingRentalCostPer = reader["ExistingRentalCostPercentage"].ToString();
                            obj.RentalCostPer = reader["ProposedRentalCostPercentage"].ToString();
                            obj.ExistingMonthlyMaintenenceCost = reader["ExistingMonthlyMaintenanceCost"].ToString();
                            obj.MonthlyMaintenenceCost = reader["ProposedMonthlyMaintenanceCost"].ToString();
                            obj.ExistingAvgMaintenanceCost = reader["[ExistingAvgMaintenanceCost]"].ToString();
                            obj.AvgMaintenanceCost = reader["ProposedAvgMaintenanceCost"].ToString();
                            obj.ExistingmonthlyElectricCost = reader["ExistingMonthlyElectricCost"].ToString();
                            obj.monthlyElectricCost = reader["ProposedMonthlyElectricCost"].ToString();
                            obj.ExistingMonthlyOtherCosts = reader["ExistingPresentMonthlyAllCost"].ToString();
                            obj.MonthlyOtherCosts = reader["ProposedPresentMonthlyAllCost"].ToString();
                            obj.ExistingTotalMonthlyRenatlCost = reader["ExistingTotalMonthlyRentalCost"].ToString();
                            obj.TotalMonthlyRenatlCost = reader["ProposedTotalMonthlyRentalCost"].ToString();
                            obj.ExistingCarPark = reader["ExistingCarPark"].ToString();
                            obj.ProposedCarPark = reader["ProposedCarPark"].ToString();



                            obj.ExistingName = reader["ExistingName"].ToString();
                            obj.Name = reader["ProposedName"].ToString();
                            obj.Remarks = reader["Remark1"].ToString();
                            obj.ProposedRemarks = reader["Remark2"].ToString();


                            obj.ExistingEmail = reader["ExistingEmail"].ToString();
                            obj.Email = reader["ProposedEmail"].ToString();
                            obj.ExistingMobile = reader["ExistingMobile"].ToString();
                            obj.Mobile = reader["ProposedMobile"].ToString();
                            obj.ExistingOfficeName = reader["ExistingOfficeName"].ToString();
                            obj.OfficeName = reader["ProposedOfficeName"].ToString();

                        }
                    }
                    reader.Close();

                    command.CommandText = updateQuery;
                    command.ExecuteNonQuery();
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMailBodyRelocation(RequestModel model)
        {

            StringBuilder bodyText = new StringBuilder(string.Empty);
            bodyText.AppendFormat("<html><body>");
            bodyText.AppendFormat("<p>Dear Sir/Mam,</p>");
            bodyText.AppendFormat("<p>Relocation request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("<table border = " + 1 + " cellpadding = " + 0 + " cellspacing = " + 0 + " width = " + 1000 + " >");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Company Name : " + model.Company + " </td><td width = " + 300 + ">Request Type : " + "Relocation " + "</td><td width = " + 300 + ">Bussinss Type : " + model.BuisnessType + " </td><td width = " + 300 + ">Allocation Type : " + model.AllocationType + "</td> </tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">City : " + model.City + "</td><td width = " + 300 + ">Date from which property required : " + model.DateFromWhich + "</td><td width = " + 300 + ">Property Type : " + model.Fitouts + "</td><td width = " + 300 + "></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Creation Date  : " + System.DateTime.Today.ToShortDateString() + "</td><td width = " + 300 + ">Created By Name : " + model.Name + "</td><td width = " + 300 + ">Created By Email : " + model.Email + "</td><td width = " + 300 + ">Created By Phone : " + model.Mobile + "</td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#08F9F5'><td>OTHER DETAILS </td><td>EXISTING </td><td>PROPOSED </td><td>OBSERVATIONS OF SIS STORE  </td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Location : </td><td> " + model.ExistingLocation + " </td><td> " + model.ProposedLocation + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Start Amount (In INR) : </td><td> " + model.ExistingLeaseStartAmount + " </td><td> " + model.LeaseStartAmount + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Escallation : </td><td> " + model.ExistingRentalEscallation + " </td><td> " + model.RentalEscallation + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Escallation Period : </td><td> " + model.ExistingEscallationPeriod + " </td><td> " + model.EscallationPeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Period : </td><td> " + model.ExistingLeasePeriod + " </td><td> " + model.LeasePeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Closure Date : </td><td> " + model.ExistingLeaseClosureDate + " </td><td> " + model.LeaseClosureDate + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Advance Rental (In INR) : </td><td> " + model.ExistingAdvanceRental + " </td><td> " + model.AdvanceRental + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Amount Hold with Owner (In INR) : </td><td> " + model.ExistingAmtHoldWithOwner + " </td><td> " + model.AmtHoldWithOwner + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Notice Period : </td><td> " + model.ExistingNoticePeriod + " </td><td> " + model.NoticePeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Road Name for Signage : </td><td> " + model.Signage + " </td><td> " + model.ProposedSignage + " </td><td></td></tr>");



            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Employee : </td><td> " + model.NoOfPersons + " </td><td> " + model.ProposedNoOfPersons + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Super Built Up Area (In Sq. Ft.) : </td><td> " + model.SuperBuiltUp + " </td><td> " + model.ProposedSuperBuiltUp + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Built Up Area (In Sq. Ft.) : </td><td> " + model.BuiltUp + " </td><td> " + model.ProposedBuiltUp + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Carpet Area (In Sq. Ft.) : </td><td> " + model.CarpetArea + " </td><td> " + model.ProposedCarpetArea + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Area (In Sq. Ft.) : </td><td> " + model.RentalArea + " </td><td> " + model.ProposedRentalArea + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental cost/square feet (In INR) : </td><td> " + model.CostPerSquareFeet + " </td><td> " + model.ProposedCostPerSquareFeet + " </td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.ExistingMonthlyCost + " </td><td> " + model.ProposedMonthlyCost + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + (Convert.ToInt32(model.RentalArea) * Convert.ToInt32(model.CostPerSquareFeet)) + " </td><td> " + (Convert.ToInt32(model.ProposedRentalArea) * Convert.ToInt32(model.ProposedCostPerSquareFeet)) + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Security Deposit (In INR) : </td><td> " + model.SecurityDeposit + " </td><td> " + model.ProposedSecurityDeposit + " </td><td></td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Present Monthly Rental Cost (In INR) : </td><td> " + model.ExistingPresentMonthlyRenatlCost + " </td><td> " + model.PresentMonthlyRenatlCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Present Monthly Billing (In INR) : </td><td> " + model.ExistingPresentMonthlyBilling + " </td><td> " + model.PresentMonthlyBilling + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Cost Percentage (In INR) : </td><td> " + model.ExistingRentalCostPer + " </td><td> " + model.RentalCostPer + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly Maintenance Cost (In INR) : </td><td> " + model.ExistingMonthlyMaintenenceCost + " </td><td> " + model.MonthlyMaintenenceCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Average Maintenance Cost (In INR) : </td><td> " + model.ExistingAvgMaintenanceCost + " </td><td> " + model.AvgMaintenanceCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly Electric Cost (In INR) : </td><td> " + model.ExistingmonthlyElectricCost + " </td><td> " + model.monthlyElectricCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly All Other Costs (In INR) : </td><td> " + model.ExistingMonthlyOtherCosts + " </td><td> " + model.MonthlyOtherCosts + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.ExistingTotalMonthlyRenatlCost + " </td><td> " + model.TotalMonthlyRenatlCost + " </td><td></td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Car Park : </td><td> " + model.ExistingCarPark + " </td><td> " + model.ProposedCarPark + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Office Name : </td><td> " + model.ExistingOfficeName + " </td><td> " + model.OfficeName + " </td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Requested By : </td><td> " + model.RequestedBy + " </td><td> " + model.ProposedRequestedBy + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Remarks :  </td><td> " + model.Remarks + " </td><td> " + model.ProposedRemarks + " </td><td></td></tr></table>");
            //bodyText.AppendFormat("<p>New request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("</body></html>");

            return bodyText.ToString();
        }

        private string GetMailBodyInitiate(RequestModel model)
        {

            StringBuilder bodyText = new StringBuilder(string.Empty);
            bodyText.AppendFormat("<html><body>");
            bodyText.AppendFormat("<p>Dear Sir,</p>");
            bodyText.AppendFormat("<p>New request has been initiated by the President. Please find the details below:</p>");
            bodyText.AppendFormat("<table border = " + 1 + " cellpadding = " + 0 + " cellspacing = " + 0 + " width = " + 1000 + " >");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Company Name : " + model.Company + " </td><td width = " + 300 + ">Request Type : " + "Relocation " + "</td><td width = " + 300 + ">Bussinss Type : " + model.BuisnessType + " </td><td width = " + 300 + ">Allocation Type : " + model.AllocationType + "</td> </tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">City : " + model.City + "</td><td width = " + 300 + ">Date from which property required : " + model.DateFromWhich + "</td><td width = " + 300 + ">Property Type : " + model.Fitouts + "</td><td width = " + 300 + "></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Creation Date  : " + System.DateTime.Today.ToShortDateString() + "</td><td width = " + 300 + ">Created By Name : " + model.Name + "</td><td width = " + 300 + ">Created By Email : " + model.Email + "</td><td width = " + 300 + ">Created By Phone : " + model.Mobile + "</td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#08F9F5'><td>OTHER DETAILS </td><td>EXISTING </td><td>PROPOSED </td><td>OBSERVATIONS OF SIS STORE  </td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Location : </td><td> " + model.ExistingLocation + " </td><td> " + model.ProposedLocation + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Start Amount (In INR) : </td><td> " + model.ExistingLeaseStartAmount + " </td><td> " + model.LeaseStartAmount + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Escallation : </td><td> " + model.ExistingRentalEscallation + " </td><td> " + model.RentalEscallation + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Escallation Period : </td><td> " + model.ExistingEscallationPeriod + " </td><td> " + model.EscallationPeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Period : </td><td> " + model.ExistingLeasePeriod + " </td><td> " + model.LeasePeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Lease Closure Date : </td><td> " + model.ExistingLeaseClosureDate + " </td><td> " + model.LeaseClosureDate + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Advance Rental (In INR) : </td><td> " + model.ExistingAdvanceRental + " </td><td> " + model.AdvanceRental + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Amount Hold with Owner (In INR) : </td><td> " + model.ExistingAmtHoldWithOwner + " </td><td> " + model.AmtHoldWithOwner + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Notice Period : </td><td> " + model.ExistingNoticePeriod + " </td><td> " + model.NoticePeriod + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Road Name for Signage : </td><td> " + model.Signage + " </td><td> " + model.ProposedSignage + " </td><td></td></tr>");



            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Employee : </td><td> " + model.NoOfPersons + " </td><td> " + model.ProposedNoOfPersons + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Super Built Up Area (In Sq. Ft.) : </td><td> " + model.SuperBuiltUp + " </td><td> " + model.ProposedSuperBuiltUp + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Built Up Area (In Sq. Ft.) : </td><td> " + model.BuiltUp + " </td><td> " + model.ProposedBuiltUp + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Carpet Area (In Sq. Ft.) : </td><td> " + model.CarpetArea + " </td><td> " + model.ProposedCarpetArea + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Area (In Sq. Ft.) : </td><td> " + model.RentalArea + " </td><td> " + model.ProposedRentalArea + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental cost/square feet (In INR) : </td><td> " + model.CostPerSquareFeet + " </td><td> " + model.ProposedCostPerSquareFeet + " </td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.ExistingMonthlyCost + " </td><td> " + model.ProposedMonthlyCost + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + (Convert.ToInt32(model.RentalArea) * Convert.ToInt32(model.CostPerSquareFeet)) + " </td><td> " + (Convert.ToInt32(model.ProposedRentalArea) * Convert.ToInt32(model.ProposedCostPerSquareFeet)) + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Security Deposit (In INR) : </td><td> " + model.SecurityDeposit + " </td><td> " + model.ProposedSecurityDeposit + " </td><td></td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Present Monthly Rental Cost (In INR) : </td><td> " + model.ExistingPresentMonthlyRenatlCost + " </td><td> " + model.PresentMonthlyRenatlCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Present Monthly Billing (In INR) : </td><td> " + model.ExistingPresentMonthlyBilling + " </td><td> " + model.PresentMonthlyBilling + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Cost Percentage (In INR) : </td><td> " + model.ExistingRentalCostPer + " </td><td> " + model.RentalCostPer + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly Maintenance Cost (In INR) : </td><td> " + model.ExistingMonthlyMaintenenceCost + " </td><td> " + model.MonthlyMaintenenceCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Average Maintenance Cost (In INR) : </td><td> " + model.ExistingAvgMaintenanceCost + " </td><td> " + model.AvgMaintenanceCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly Electric Cost (In INR) : </td><td> " + model.ExistingmonthlyElectricCost + " </td><td> " + model.monthlyElectricCost + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Monthly All Other Costs (In INR) : </td><td> " + model.ExistingMonthlyOtherCosts + " </td><td> " + model.MonthlyOtherCosts + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.ExistingTotalMonthlyRenatlCost + " </td><td> " + model.TotalMonthlyRenatlCost + " </td><td></td></tr>");

            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Car Park : </td><td> " + model.ExistingCarPark + " </td><td> " + model.ProposedCarPark + " </td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Office Name : </td><td> " + model.ExistingOfficeName + " </td><td> " + model.OfficeName + " </td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Requested By : </td><td> " + model.RequestedBy + " </td><td> " + model.ProposedRequestedBy + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Remarks :  </td><td> " + model.Remarks + " </td><td> " + model.ProposedRemarks + " </td><td></td></tr></table>");
            //bodyText.AppendFormat("<p>New request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("</body></html>");

            return bodyText.ToString();
        }


        public void ApproveDetails(int ID, string Name)
        {
            RequestModel model = GetData(ID, Name);
            string emailBody = GetMailBody(model);
            //if approved email sent to karan + gaosain + GMD + procurement    
            List<string> EmailIds = GetEmailIdList("Procurement");
            SendEmail("New office space requirement is approved", emailBody, EmailIds, null, true);
        }

        public void DisapproveDetails(int ID, string Name)
        {
            throw new NotImplementedException();
        }

        public List<RequestModel> GetApprovalRequests(SearchApprovalRequestModel searchParam)
        {
            var result = new List<RequestModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_Fetch_GetApprovalRequests";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Company", searchParam.Company);
                command.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(searchParam.FromDate));
                command.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(searchParam.ToDate));
                command.Parameters.AddWithValue("@City", searchParam.City);
                command.Parameters.AddWithValue("@Status", searchParam.Status);
                command.Parameters.AddWithValue("@UserName", searchParam.UserName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RequestModel obj = new RequestModel();
                        obj.ID = Convert.ToInt32(reader["ID"].ToString());
                        obj.RequestID = reader["RequestId"].ToString();
                        obj.CreationDate = reader["CreationDate"].ToString();
                        obj.Company = reader["Company"].ToString();
                        obj.City = reader["City"].ToString();
                        obj.BussinessType = reader["BussinessType"].ToString();
                        obj.MenuSelection = reader["MenuSelection"].ToString();
                        obj.Status = reader["Status"].ToString();
                        obj.OfficeName = reader["ProposedOfficeName"].ToString();
                        obj.ProposedLocation = reader["ProposedLocation"].ToString();

                        result.Add(obj);
                    }

                }

                reader.Close();
                return result;
            }

        }

        public List<RequestModel> FreshRequests(int ID)
        {
            var result = new List<RequestModel>();
            //RequestModel request = new RequestModel();
            //request.FurnishedDataList = new List<RequestModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_Fetch_FreshRequests";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RequestModel obj = new RequestModel();
                        if (reader["RequestType"].ToString() == "Existing")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.Signage = reader["Signage"].ToString();
                            obj.NoOfPersons = reader["Employee"].ToString();
                            obj.SuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["BuiltUpArea"].ToString();
                            obj.CarpetArea = reader["CarpetArea"].ToString();
                            obj.RentalArea = reader["RentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ExistingCarPark = reader["CarPark"].ToString();
                            obj.Remarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.CalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.ExistingLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.ExistingLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.ExistingRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.ExistingEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.ExistingLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.ExistingAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.ExistingAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.ExistingNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.ExistingPresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.ExistingPresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.ExistingRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.ExistingMonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.ExistingAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.ExistingmonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.ExistingMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.ExistingTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.ExistingName = reader["Name"].ToString();
                            obj.ExistingEmail = reader["Email"].ToString();
                            obj.ExistingMobile = reader["Mobile"].ToString();
                            obj.ExistingOfficeName = reader["OfficeName"].ToString();




                        }
                        else
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ProposedLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.ProposedSignage = reader["Signage"].ToString();
                            obj.ProposedNoOfPersons = reader["Employee"].ToString();
                            obj.ProposedSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["BuiltUpArea"].ToString();
                            obj.ProposedCarpetArea = reader["CarpetArea"].ToString();
                            obj.ProposedRentalArea = reader["RentalArea"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.ProposedSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ProposedCarPark = reader["CarPark"].ToString();
                            obj.ProposedRemarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.ProposedCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.LeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.LeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.RentalEscallation = reader["RentalEscallation"].ToString();
                            obj.EscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.LeasePeriod = reader["LeasePeriod"].ToString();
                            obj.AdvanceRental = reader["AdvanceRental"].ToString();
                            obj.AmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.NoticePeriod = reader["NoticePeriod"].ToString();
                            obj.PresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.PresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.RentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.MonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.AvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.monthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.MonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.TotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.Name = reader["Name"].ToString();
                            obj.Email = reader["Email"].ToString();
                            obj.Mobile = reader["Mobile"].ToString();
                            obj.OfficeName = reader["OfficeName"].ToString();

                        }

                        result.Add(obj);
                    }
                }
                reader.Close();
                return result;
            }
        }

        public List<RequestModel> InitiatedRequests(int ID)
        {
            var result = new List<RequestModel>();
            //RequestModel request = new RequestModel();
            //request.FurnishedDataList = new List<RequestModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_Fetch_InitaiteRequests";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RequestModel obj = new RequestModel();
                        if (reader["RequestType"].ToString() == "Existing")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.Signage = reader["Signage"].ToString();
                            obj.NoOfPersons = reader["Employee"].ToString();
                            obj.SuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["BuiltUpArea"].ToString();
                            obj.CarpetArea = reader["CarpetArea"].ToString();
                            obj.RentalArea = reader["RentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ExistingCarPark = reader["CarPark"].ToString();
                            obj.Remarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.CalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.ExistingLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.ExistingLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.ExistingRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.ExistingEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.ExistingLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.ExistingAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.ExistingAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.ExistingNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.ExistingPresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.ExistingPresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.ExistingRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.ExistingMonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.ExistingAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.ExistingmonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.ExistingMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.ExistingTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.ExistingName = reader["Name"].ToString();
                            obj.ExistingEmail = reader["Email"].ToString();
                            obj.ExistingMobile = reader["Mobile"].ToString();
                            obj.ExistingOfficeName = reader["OfficeName"].ToString();




                        }
                        else if (reader["RequestType"].ToString() == "Proposed")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ProposedLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.ProposedSignage = reader["Signage"].ToString();
                            obj.ProposedNoOfPersons = reader["Employee"].ToString();
                            obj.ProposedSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["BuiltUpArea"].ToString();
                            obj.ProposedCarpetArea = reader["CarpetArea"].ToString();
                            obj.ProposedRentalArea = reader["RentalArea"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.ProposedSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ProposedCarPark = reader["CarPark"].ToString();
                            obj.ProposedRemarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.ProposedCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.LeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.LeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.RentalEscallation = reader["RentalEscallation"].ToString();
                            obj.EscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.LeasePeriod = reader["LeasePeriod"].ToString();
                            obj.AdvanceRental = reader["AdvanceRental"].ToString();
                            obj.AmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.NoticePeriod = reader["NoticePeriod"].ToString();
                            obj.PresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.PresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.RentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.MonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.AvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.monthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.MonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.TotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.Name = reader["Name"].ToString();
                            obj.Email = reader["Email"].ToString();
                            obj.Mobile = reader["Mobile"].ToString();
                            obj.OfficeName = reader["OfficeName"].ToString();


                        }
                        else
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.InitiateLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.InitiateSignage = reader["Signage"].ToString();
                            obj.InitiateNoOfPerson = reader["Employee"].ToString();
                            obj.InitiateSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.InitiateBuiltup = reader["BuiltUpArea"].ToString();
                            obj.InitiateCarpetArea = reader["CarpetArea"].ToString();
                            obj.InitiateRentalArea = reader["RentalArea"].ToString();
                            obj.InitiateRentalCost = reader["RentalCost"].ToString();
                            obj.InitiateSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.InitiateCarpark = reader["CarPark"].ToString();
                            obj.InitiateRemarks = reader["Remark"].ToString();
                            obj.InitiateStatus = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.InitiateCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.InitiateLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.InitiateLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.InitiateRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.InitiateEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.InitiateLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.InitiateLeaseAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.InitiateAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.InitiateNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.InitiateMonthlyRentalCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.InitiateMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.InitiateRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.InitiatemonthlyMaintenanceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.InitiateAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.InitiateMonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.InitiateMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.InitiateTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.InitiateName = reader["Name"].ToString();
                            obj.InitiateEmail = reader["Email"].ToString();
                            obj.InitiateMobile = reader["Mobile"].ToString();
                            obj.InitiateOfficeName = reader["OfficeName"].ToString();


                        }

                        result.Add(obj);
                    }
                }
                reader.Close();
                return result;
            }
        }

        public List<RequestModel> IdentifiedRequests(int ID)
        {
            var result = new List<RequestModel>();
            //RequestModel request = new RequestModel();
            //request.FurnishedDataList = new List<RequestModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_Fetch_IdentifiedRequests";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RequestModel obj = new RequestModel();
                        if (reader["RequestType"].ToString() == "Existing")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.Signage = reader["Signage"].ToString();
                            obj.NoOfPersons = reader["Employee"].ToString();
                            obj.SuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["BuiltUpArea"].ToString();
                            obj.CarpetArea = reader["CarpetArea"].ToString();
                            obj.RentalArea = reader["RentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ExistingCarPark = reader["CarPark"].ToString();
                            obj.Remarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.CalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.ExistingLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.ExistingLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.ExistingRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.ExistingEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.ExistingLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.ExistingAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.ExistingAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.ExistingNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.ExistingPresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.ExistingPresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.ExistingRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.ExistingMonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.ExistingAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.ExistingmonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.ExistingMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.ExistingTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.ExistingName = reader["Name"].ToString();
                            obj.ExistingEmail = reader["Email"].ToString();
                            obj.ExistingMobile = reader["Mobile"].ToString();
                            obj.ExistingOfficeName = reader["OfficeName"].ToString();




                        }
                        else if (reader["RequestType"].ToString() == "Proposed")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ProposedLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.ProposedSignage = reader["Signage"].ToString();
                            obj.ProposedNoOfPersons = reader["Employee"].ToString();
                            obj.ProposedSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["BuiltUpArea"].ToString();
                            obj.ProposedCarpetArea = reader["CarpetArea"].ToString();
                            obj.ProposedRentalArea = reader["RentalArea"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.ProposedSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ProposedCarPark = reader["CarPark"].ToString();
                            obj.ProposedRemarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.ProposedCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.LeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.LeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.RentalEscallation = reader["RentalEscallation"].ToString();
                            obj.EscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.LeasePeriod = reader["LeasePeriod"].ToString();
                            obj.AdvanceRental = reader["AdvanceRental"].ToString();
                            obj.AmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.NoticePeriod = reader["NoticePeriod"].ToString();
                            obj.PresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.PresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.RentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.MonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.AvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.monthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.MonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.TotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.Name = reader["Name"].ToString();
                            obj.Email = reader["Email"].ToString();
                            obj.Mobile = reader["Mobile"].ToString();
                            obj.OfficeName = reader["OfficeName"].ToString();


                        }
                        else
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.InitiateLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.InitiateSignage = reader["Signage"].ToString();
                            obj.InitiateNoOfPerson = reader["Employee"].ToString();
                            obj.InitiateSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.InitiateBuiltup = reader["BuiltUpArea"].ToString();
                            obj.InitiateCarpetArea = reader["CarpetArea"].ToString();
                            obj.InitiateRentalArea = reader["RentalArea"].ToString();
                            obj.InitiateRentalCost = reader["RentalCost"].ToString();
                            obj.InitiateSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.InitiateCarpark = reader["CarPark"].ToString();
                            obj.InitiateRemarks = reader["Remark"].ToString();
                            obj.InitiateStatus = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.InitiateCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.InitiateLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.InitiateLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.InitiateRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.InitiateEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.InitiateLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.InitiateLeaseAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.InitiateAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.InitiateNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.InitiateMonthlyRentalCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.InitiateMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.InitiateRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.InitiatemonthlyMaintenanceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.InitiateAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.InitiateMonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.InitiateMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.InitiateTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.InitiateName = reader["Name"].ToString();
                            obj.InitiateEmail = reader["Email"].ToString();
                            obj.InitiateMobile = reader["Mobile"].ToString();
                            obj.InitiateOfficeName = reader["OfficeName"].ToString();


                        }

                        result.Add(obj);
                    }
                }
                reader.Close();
                return result;
            }
        }

        public List<RequestModel> ApprovedRequests(int ID)
        {
            var result = new List<RequestModel>();
            //RequestModel request = new RequestModel();
            //request.FurnishedDataList = new List<RequestModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_Fetch_ApprovedRequests";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RequestModel obj = new RequestModel();
                        if (reader["RequestType"].ToString() == "Existing")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.Signage = reader["Signage"].ToString();
                            obj.NoOfPersons = reader["Employee"].ToString();
                            obj.SuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["BuiltUpArea"].ToString();
                            obj.CarpetArea = reader["CarpetArea"].ToString();
                            obj.RentalArea = reader["RentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ExistingCarPark = reader["CarPark"].ToString();
                            obj.Remarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.CalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.ExistingLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.ExistingLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.ExistingRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.ExistingEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.ExistingLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.ExistingAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.ExistingAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.ExistingNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.ExistingPresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.ExistingPresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.ExistingRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.ExistingMonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.ExistingAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.ExistingmonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.ExistingMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.ExistingTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.ExistingName = reader["Name"].ToString();
                            obj.ExistingEmail = reader["Email"].ToString();
                            obj.ExistingMobile = reader["Mobile"].ToString();
                            obj.ExistingOfficeName = reader["OfficeName"].ToString();




                        }
                        else if (reader["RequestType"].ToString() == "Proposed")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ProposedLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.ProposedSignage = reader["Signage"].ToString();
                            obj.ProposedNoOfPersons = reader["Employee"].ToString();
                            obj.ProposedSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["BuiltUpArea"].ToString();
                            obj.ProposedCarpetArea = reader["CarpetArea"].ToString();
                            obj.ProposedRentalArea = reader["RentalArea"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.ProposedSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ProposedCarPark = reader["CarPark"].ToString();
                            obj.ProposedRemarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.ProposedCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.LeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.LeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.RentalEscallation = reader["RentalEscallation"].ToString();
                            obj.EscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.LeasePeriod = reader["LeasePeriod"].ToString();
                            obj.AdvanceRental = reader["AdvanceRental"].ToString();
                            obj.AmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.NoticePeriod = reader["NoticePeriod"].ToString();
                            obj.PresentMonthlyRenatlCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.PresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.RentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.MonthlyMaintenenceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.AvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.monthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.MonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.TotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.Name = reader["Name"].ToString();
                            obj.Email = reader["Email"].ToString();
                            obj.Mobile = reader["Mobile"].ToString();
                            obj.OfficeName = reader["OfficeName"].ToString();


                        }
                        else if (reader["RequestType"].ToString() == "Initiated")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.InitiateLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.InitiateSignage = reader["Signage"].ToString();
                            obj.InitiateNoOfPerson = reader["Employee"].ToString();
                            obj.InitiateSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.InitiateBuiltup = reader["BuiltUpArea"].ToString();
                            obj.InitiateCarpetArea = reader["CarpetArea"].ToString();
                            obj.InitiateRentalArea = reader["RentalArea"].ToString();
                            obj.InitiateRentalCost = reader["RentalCost"].ToString();
                            obj.InitiateSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.InitiateCarpark = reader["CarPark"].ToString();
                            obj.InitiateRemarks = reader["Remark"].ToString();
                            obj.InitiateStatus = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.InitiateCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.InitiateLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.InitiateLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.InitiateRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.InitiateEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.InitiateLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.InitiateLeaseAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.InitiateAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.InitiateNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.InitiateMonthlyRentalCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.InitiateMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.InitiateRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.InitiatemonthlyMaintenanceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.InitiateAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.InitiateMonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.InitiateMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.InitiateTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.InitiateName = reader["Name"].ToString();
                            obj.InitiateEmail = reader["Email"].ToString();
                            obj.InitiateMobile = reader["Mobile"].ToString();
                            obj.InitiateOfficeName = reader["OfficeName"].ToString();


                        }
                        else
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.IdentifiedLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.IdentifiedSignage = reader["Signage"].ToString();
                            obj.IdentifiedNoOfPerson = reader["Employee"].ToString();
                            obj.IdentifiedSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.IdentifiedBuiltup = reader["BuiltUpArea"].ToString();
                            obj.IdentifiedCarpetArea = reader["CarpetArea"].ToString();
                            obj.IdentifiedRentalArea = reader["RentalArea"].ToString();
                            obj.IdentifiedRentalCost = reader["RentalCost"].ToString();
                            obj.IdentifiedSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.IdentifiedCarpark = reader["CarPark"].ToString();
                            obj.IdentifiedRemarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.IdentifiedCalcMonthlyCost = reader["MonthlyCost"].ToString();

                            obj.IdentifiedLeaseStartDate = reader["LeaseStartDate"].ToString();
                            obj.IdentifiedLeaseStartAmount = reader["LeaseStartAmount"].ToString();
                            obj.IdentifiedRentalEscallation = reader["RentalEscallation"].ToString();
                            obj.IdentifiedEscallationPeriod = reader["EscallationPeriod"].ToString();
                            obj.IdentifiedLeasePeriod = reader["LeasePeriod"].ToString();
                            obj.IdentifiedLeaseAdvanceRental = reader["AdvanceRental"].ToString();
                            obj.IdentifiedAmtHoldWithOwner = reader["AmountHoldWithOwner"].ToString();
                            obj.IdentifiedNoticePeriod = reader["NoticePeriod"].ToString();
                            obj.IdentifiedMonthlyRentalCost = reader["PresentMonthlyRentalCost"].ToString();
                            obj.IdentifiedMonthlyBilling = reader["PresentMonthlyBilling"].ToString();
                            obj.IdentifiedRentalCostPer = reader["RentalCostPercentage"].ToString();
                            obj.IdentifiedmonthlyMaintenanceCost = reader["MonthlyMaintenanceCost"].ToString();
                            obj.IdentifiedAvgMaintenanceCost = reader["AvgMaintenanceCost"].ToString();
                            obj.IdentifiedMonthlyElectricCost = reader["MonthlyElectricCost"].ToString();
                            obj.IdentifiedMonthlyOtherCosts = reader["PresentMonthlyAllCost"].ToString();
                            obj.IdentifiedTotalMonthlyRenatlCost = reader["TotalMonthlyRentalCost"].ToString();
                            obj.IdentifiedName = reader["Name"].ToString();
                            obj.IdentifiedEmail = reader["Email"].ToString();
                            obj.IdentifiedMobile = reader["Mobile"].ToString();
                            obj.IdentifiedOfficeName = reader["OfficeName"].ToString();


                        }

                        result.Add(obj);
                    }
                }
                reader.Close();
                return result;
            }
        }

        public int UpdateFreshStatus(int ID, string UserName, string Status)
        {
            try
            {
                int SaveStatus = 0;
              
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "udp_Store_Update_FreshRequestStatus";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@LoggedInUser", UserName);
              
                    SaveStatus = int.Parse(command.ExecuteScalar().ToString());

                }
                return SaveStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFreshRequests(RequestModel Model)
        {
            try
            {
              //  int SaveStatus = 0;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    //  FurnishedDataList obj in Model.FurnishedDataList            

                    //foreach (RequestModel obj in Model.FurnishedDataList)
                    //{
                    //    if (obj.DataType == "Proposed")
                    //    {
                            command.CommandText = "udp_Store_Update_FreshRequestData";
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ID", Convert.ToInt32(Model.ID));
                            command.Parameters.AddWithValue("@OfficeName", Model.OfficeName);
                            command.Parameters.AddWithValue("@OfficeLocation", Model.ProposedLocation);
                            command.Parameters.AddWithValue("@LeaseStartDate",Convert.ToDateTime( Model.LeaseStartDate));
                            command.Parameters.AddWithValue("@RentalAmount", Model.LeaseStartAmount);
                            command.Parameters.AddWithValue("@RentalEscallation", Model.RentalEscallation);
                            command.Parameters.AddWithValue("@EscallationPeriod", Model.EscallationPeriod);
                            command.Parameters.AddWithValue("@LeasePeriod", Model.LeasePeriod);
                            //command.Parameters.AddWithValue("@LeaseClousureDate", Convert.ToDateTime(Model.LeaseClosureDate));
                            command.Parameters.AddWithValue("@AdvanceRental", Model.AdvanceRental);
                            command.Parameters.AddWithValue("@AmtHoldWithOwner", Model.AmtHoldWithOwner);
                            command.Parameters.AddWithValue("@NoticePeriod", Model.NoticePeriod);
                            command.Parameters.AddWithValue("@Signage", Model.ProposedSignage);
                            command.Parameters.AddWithValue("@NoOfEmp", Model.ProposedNoOfPersons);
                            command.Parameters.AddWithValue("@SuperBuiltUp", Model.ProposedSuperBuiltUp);
                            command.Parameters.AddWithValue("@BuiltUp", Model.ProposedBuiltUp);
                            command.Parameters.AddWithValue("@CarpetArea", Model.ProposedCarpetArea);
                            command.Parameters.AddWithValue("@RentalArea", Model.ProposedRentalArea);
                            command.Parameters.AddWithValue("@RentalCost", Model.ProposedCalcMonthlyCost);
                            command.Parameters.AddWithValue("@SecurityDeposit", Model.ProposedSecurityDeposit);
                            command.Parameters.AddWithValue("@MonthlyRentalCost", Model.PresentMonthlyRenatlCost);
                            command.Parameters.AddWithValue("@MonthlyBilling", Model.PresentMonthlyBilling);
                            command.Parameters.AddWithValue("@RentalcostPer", Model.RentalCostPer);
                            command.Parameters.AddWithValue("@MonthlyMaintenace", Model.MonthlyMaintenenceCost);
                            command.Parameters.AddWithValue("@AvgMaintenance", Model.AvgMaintenanceCost);
                            command.Parameters.AddWithValue("@ElectricCost", Model.monthlyElectricCost);
                            command.Parameters.AddWithValue("@OtherCosts", Model.MonthlyOtherCosts);
                            command.Parameters.AddWithValue("@TotalRentalCost", Model.TotalMonthlyRenatlCost);
                            command.Parameters.AddWithValue("@CarPark", Model.ProposedCarPark);
                            command.Parameters.AddWithValue("@Name", Model.Name);
                            command.Parameters.AddWithValue("@Email", Model.Email);
                            command.Parameters.AddWithValue("@Mobile", Model.Mobile);
                            command.Parameters.AddWithValue("@Remarks", Model.ProposedRemarks);
                            command.Parameters.AddWithValue("@Username", Model.LoggedInUser);

                            command.ExecuteNonQuery();
                           // SaveStatus = int.Parse(command.ExecuteScalar().ToString());
                            command.Parameters.Clear();
                    //    }
                    // //   return SaveStatus;

                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SubmitInitiateRequests(RequestModel Model)
        {
            try
            {
                //  int SaveStatus = 0;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    //  FurnishedDataList obj in Model.FurnishedDataList            

                    //foreach (RequestModel obj in Model.FurnishedDataList)
                    //{
                    //    if (obj.DataType == "Proposed")
                    //    {
                    command.CommandText = "udp_Store_Insert_InitiateRequestData";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(Model.ID));
                    command.Parameters.AddWithValue("@OfficeName", Model.InitiateOfficeName);
                    command.Parameters.AddWithValue("@OfficeLocation", Model.InitiateLocation);
                    command.Parameters.AddWithValue("@LeaseStartDate", Convert.ToDateTime(Model.InitiateLeaseStartDate));
                    command.Parameters.AddWithValue("@RentalAmount", Model.InitiateLeaseStartAmount);
                    command.Parameters.AddWithValue("@RentalEscallation", Model.InitiateRentalEscallation);
                    command.Parameters.AddWithValue("@EscallationPeriod", Model.InitiateEscallationPeriod);
                    command.Parameters.AddWithValue("@LeasePeriod", Model.InitiateLeasePeriod);
                    //command.Parameters.AddWithValue("@LeaseClousureDate", Convert.ToDateTime(Model.LeaseClosureDate));
                    command.Parameters.AddWithValue("@AdvanceRental", Model.InitiateLeaseAdvanceRental);
                    command.Parameters.AddWithValue("@AmtHoldWithOwner", Model.InitiateAmtHoldWithOwner);
                    command.Parameters.AddWithValue("@NoticePeriod", Model.InitiateNoticePeriod);
                    command.Parameters.AddWithValue("@Signage", Model.InitiateSignage);
                    command.Parameters.AddWithValue("@NoOfEmp", Model.InitiateNoOfPerson);
                    command.Parameters.AddWithValue("@SuperBuiltUp", Model.InitiateSuperBuiltUp);
                    command.Parameters.AddWithValue("@BuiltUp", Model.InitiateBuiltup);
                    command.Parameters.AddWithValue("@CarpetArea", Model.InitiateCarpetArea);
                    command.Parameters.AddWithValue("@RentalArea", Model.InitiateRentalArea);
                    command.Parameters.AddWithValue("@RentalCost",  Model.InitiateRentalCost);
                    command.Parameters.AddWithValue("@SecurityDeposit", Model.InitiateSecurityDeposit);
                    command.Parameters.AddWithValue("@MonthlyRentalCost", Model.InitiateMonthlyRentalCost);
                    command.Parameters.AddWithValue("@MonthlyBilling", Model.InitiateMonthlyBilling);
                    command.Parameters.AddWithValue("@RentalcostPer", Model.InitiateRentalCostPer);
                    command.Parameters.AddWithValue("@MonthlyMaintenace", Model.InitiatemonthlyMaintenanceCost);
                    command.Parameters.AddWithValue("@AvgMaintenance", Model.InitiateAvgMaintenanceCost);
                    command.Parameters.AddWithValue("@ElectricCost", Model.InitiateMonthlyElectricCost);
                    command.Parameters.AddWithValue("@OtherCosts", Model.InitiateMonthlyOtherCosts);
                    command.Parameters.AddWithValue("@TotalRentalCost", Model.InitiateTotalMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@CarPark", Model.InitiateCarpark);
                    command.Parameters.AddWithValue("@Name", Model.InitiateName);
                    command.Parameters.AddWithValue("@Email", Model.InitiateEmail);
                    command.Parameters.AddWithValue("@Mobile", Model.InitiateMobile);
                    command.Parameters.AddWithValue("@Remarks", Model.InitiateRemarks);
                    command.Parameters.AddWithValue("@Username", Model.LoggedInUser);

                    command.ExecuteNonQuery();
                    // SaveStatus = int.Parse(command.ExecuteScalar().ToString());
                    command.Parameters.Clear();
                    //    }
                    // //   return SaveStatus;

                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateIdentifyStatus(int ID, string UserName, string Status)
        {
            try
            {
                int SaveStatus = 0;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "udp_Store_Update_IdentifyRequestStatus";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@LoggedInUser", UserName);
                    SaveStatus = int.Parse(command.ExecuteScalar().ToString());

                }
                return SaveStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateIdentifyRequests(RequestModel Model)
        {
            try
            {
                //  int SaveStatus = 0;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    //  FurnishedDataList obj in Model.FurnishedDataList            

                    //foreach (RequestModel obj in Model.FurnishedDataList)
                    //{
                    //    if (obj.DataType == "Proposed")
                    //    {
                    command.CommandText = "udp_Store_Update_IdentifyRequestData";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(Model.ID));
                    command.Parameters.AddWithValue("@OfficeName", Model.InitiateOfficeName);
                    command.Parameters.AddWithValue("@OfficeLocation", Model.InitiateLocation);
                    command.Parameters.AddWithValue("@LeaseStartDate", Convert.ToDateTime(Model.InitiateLeaseStartDate));
                    command.Parameters.AddWithValue("@RentalAmount", Model.InitiateLeaseStartAmount);
                    command.Parameters.AddWithValue("@RentalEscallation", Model.InitiateRentalEscallation);
                    command.Parameters.AddWithValue("@EscallationPeriod", Model.InitiateEscallationPeriod);
                    command.Parameters.AddWithValue("@LeasePeriod", Model.InitiateLeasePeriod);
                    //command.Parameters.AddWithValue("@LeaseClousureDate", Convert.ToDateTime(Model.LeaseClosureDate));
                    command.Parameters.AddWithValue("@AdvanceRental", Model.InitiateLeaseAdvanceRental);
                    command.Parameters.AddWithValue("@AmtHoldWithOwner", Model.InitiateAmtHoldWithOwner);
                    command.Parameters.AddWithValue("@NoticePeriod", Model.InitiateNoticePeriod);
                    command.Parameters.AddWithValue("@Signage", Model.InitiateSignage);
                    command.Parameters.AddWithValue("@NoOfEmp", Model.InitiateNoOfPerson);
                    command.Parameters.AddWithValue("@SuperBuiltUp", Model.InitiateSuperBuiltUp);
                    command.Parameters.AddWithValue("@BuiltUp", Model.InitiateBuiltup);
                    command.Parameters.AddWithValue("@CarpetArea", Model.InitiateCarpetArea);
                    command.Parameters.AddWithValue("@RentalArea", Model.InitiateRentalArea);
                    command.Parameters.AddWithValue("@RentalCost", Model.InitiateRentalCost);
                    command.Parameters.AddWithValue("@SecurityDeposit", Model.InitiateSecurityDeposit);
                    command.Parameters.AddWithValue("@MonthlyRentalCost", Model.InitiateMonthlyRentalCost);
                    command.Parameters.AddWithValue("@MonthlyBilling", Model.InitiateMonthlyBilling);
                    command.Parameters.AddWithValue("@RentalcostPer", Model.InitiateRentalCostPer);
                    command.Parameters.AddWithValue("@MonthlyMaintenace", Model.InitiatemonthlyMaintenanceCost);
                    command.Parameters.AddWithValue("@AvgMaintenance", Model.InitiateAvgMaintenanceCost);
                    command.Parameters.AddWithValue("@ElectricCost", Model.InitiateMonthlyElectricCost);
                    command.Parameters.AddWithValue("@OtherCosts", Model.InitiateMonthlyOtherCosts);
                    command.Parameters.AddWithValue("@TotalRentalCost", Model.InitiateTotalMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@CarPark", Model.InitiateCarpark);
                    command.Parameters.AddWithValue("@Name", Model.InitiateName);
                    command.Parameters.AddWithValue("@Email", Model.InitiateEmail);
                    command.Parameters.AddWithValue("@Mobile", Model.InitiateMobile);
                    command.Parameters.AddWithValue("@Remarks", Model.InitiateRemarks);
                    command.Parameters.AddWithValue("@Username", Model.LoggedInUser);

                    command.ExecuteNonQuery();

                  
                    // SaveStatus = int.Parse(command.ExecuteScalar().ToString());
                    command.Parameters.Clear();
                    //    }
                    // //   return SaveStatus;

                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SubmitClosureRequests(RequestModel Model)
        {
            try
            {
                //  int SaveStatus = 0;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    //  FurnishedDataList obj in Model.FurnishedDataList            

                    //foreach (RequestModel obj in Model.FurnishedDataList)
                    //{
                    //    if (obj.DataType == "Proposed")
                    //    {
                    command.CommandText = "udp_Store_Insert_clousureRequestData";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(Model.ID));
                    command.Parameters.AddWithValue("@OfficeName", Model.IdentifiedOfficeName);
                    command.Parameters.AddWithValue("@OfficeLocation", Model.IdentifiedLocation);
                    command.Parameters.AddWithValue("@LeaseStartDate", Convert.ToDateTime(Model.IdentifiedLeaseStartDate));
                    command.Parameters.AddWithValue("@RentalAmount", Model.IdentifiedLeaseStartAmount);
                    command.Parameters.AddWithValue("@RentalEscallation", Model.IdentifiedRentalEscallation);
                    command.Parameters.AddWithValue("@EscallationPeriod", Model.IdentifiedEscallationPeriod);
                    command.Parameters.AddWithValue("@LeasePeriod", Model.IdentifiedLeasePeriod);
                    //command.Parameters.AddWithValue("@LeaseClousureDate", Convert.ToDateTime(Model.LeaseClosureDate));
                    command.Parameters.AddWithValue("@AdvanceRental", Model.IdentifiedLeaseAdvanceRental);
                    command.Parameters.AddWithValue("@AmtHoldWithOwner", Model.IdentifiedAmtHoldWithOwner);
                    command.Parameters.AddWithValue("@NoticePeriod", Model.IdentifiedNoticePeriod);
                    command.Parameters.AddWithValue("@Signage", Model.IdentifiedSignage);
                    command.Parameters.AddWithValue("@NoOfEmp", Model.IdentifiedNoOfPerson);
                    command.Parameters.AddWithValue("@SuperBuiltUp", Model.IdentifiedSuperBuiltUp);
                    command.Parameters.AddWithValue("@BuiltUp", Model.IdentifiedBuiltup);
                    command.Parameters.AddWithValue("@CarpetArea", Model.IdentifiedCarpetArea);
                    command.Parameters.AddWithValue("@RentalArea", Model.IdentifiedRentalArea);
                    command.Parameters.AddWithValue("@RentalCost", Model.IdentifiedRentalCost);
                    command.Parameters.AddWithValue("@SecurityDeposit", Model.IdentifiedSecurityDeposit);
                    command.Parameters.AddWithValue("@MonthlyRentalCost", Model.IdentifiedMonthlyRentalCost);
                    command.Parameters.AddWithValue("@MonthlyBilling", Model.IdentifiedMonthlyBilling);
                    command.Parameters.AddWithValue("@RentalcostPer", Model.IdentifiedRentalCostPer);
                    command.Parameters.AddWithValue("@MonthlyMaintenace", Model.IdentifiedmonthlyMaintenanceCost);
                    command.Parameters.AddWithValue("@AvgMaintenance", Model.IdentifiedAvgMaintenanceCost);
                    command.Parameters.AddWithValue("@ElectricCost", Model.IdentifiedMonthlyElectricCost);
                    command.Parameters.AddWithValue("@OtherCosts", Model.IdentifiedMonthlyOtherCosts);
                    command.Parameters.AddWithValue("@TotalRentalCost", Model.IdentifiedTotalMonthlyRenatlCost);
                    command.Parameters.AddWithValue("@CarPark", Model.IdentifiedCarpark);
                    command.Parameters.AddWithValue("@Name", Model.IdentifiedName);
                    command.Parameters.AddWithValue("@Email", Model.IdentifiedEmail);
                    command.Parameters.AddWithValue("@Mobile", Model.IdentifiedMobile);
                    command.Parameters.AddWithValue("@Remarks", Model.IdentifiedRemarks);
                    command.Parameters.AddWithValue("@Username", Model.LoggedInUser);

                    command.ExecuteNonQuery();
                    // SaveStatus = int.Parse(command.ExecuteScalar().ToString());
                    command.Parameters.Clear();
                    //    }
                    // //   return SaveStatus;

                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UploadLeaseDocument(int ID,string FileName)
        {
            try
            {
              
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "udp_Store_UpdateLeaseDocument";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(ID));
                    command.Parameters.AddWithValue("@FileName", FileName);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                 }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}