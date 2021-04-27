using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeSpace.Models;

using System.ComponentModel;


namespace OfficeSpace.Controllers
{
    public class FurnishedRentalsController : Controller
    {
        // GET: FurnishedRentals


        public ActionResult Index()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                string roleName = Session["CurrentUserRole"].ToString();

                ViewBag.pageSize = 15;
                FurnishedRentalModel model = new FurnishedRentalModel();
                BAL bal = new BAL();
                var companies = new List<Company>();
                if (roleName == "Admin")
                {
                    companies = bal.GetCompanyList(roleName);
                }
                else
                {
                    companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
                }
                model.Companies = (from company in companies
                                   select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
                model.SelectedCompanyIds = companies.Select(comp => comp.CompayName).ToArray();

                model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.Join(",", model.SelectedCompanyIds.Select(com => com)));
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult Index(FurnishedRentalModel model)
        {
            try
            {

                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                string roleName = Session["CurrentUserRole"].ToString();

                ViewBag.pageSize = 15;
                BAL bal = new BAL();
                var companies = new List<Company>();
                if (roleName == "Admin")
                {
                    companies = bal.GetCompanyList(roleName);
                }
                else
                {
                    companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
                }
                model.Companies = (from company in companies
                                   select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
                model.BranchDataList = new List<FurnishedOffice>();
                if (model.SelectedCompanyIds != null && model.SelectedCompanyIds.Count() > 0)
                {
                    model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.Join(",", model.SelectedCompanyIds.Select(com => com)));
                }
                return View(model);



            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Downloadfile(string companyName, string Flag)
        {
            FurnishedRentalModel model = new FurnishedRentalModel();
            //model.Company = companyName;
            //model.Flag = Flag;
            string roleName = Session["CurrentUserRole"].ToString();
            BAL bal = new BAL();
            var companies = new List<Company>();
            if (roleName == "Admin")
            {
                companies = bal.GetCompanyList(roleName);
            }
            else
            {
                companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
            }
            model.Companies = (from company in companies
                               select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
            model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.IsNullOrEmpty(model.SelectedCompanyId) ? string.Join(",", companies.Select(com => com.CompayName)) : model.SelectedCompanyId);
            string userDateFormat = "dd/MM/yyyy";

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ExistingRentalExcelModel));
            Dictionary<string, string> columnDisplayName = new Dictionary<string, string>();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                columnDisplayName.Add(property.Name, property.DisplayName);
            }

            List<ExistingRentalExcelModel> ExistingRentalDataModel = new List<ExistingRentalExcelModel>();
            foreach (FurnishedOffice obj in model.BranchDataList)
            {
                ExistingRentalExcelModel DMObjExisting = new ExistingRentalExcelModel();
                DMObjExisting.SNI = obj.ID;
                DMObjExisting.Date = obj.Date;
                DMObjExisting.City = obj.City;
                DMObjExisting.Company = obj.Company;
                DMObjExisting.BORO = obj.OfficeType;
                DMObjExisting.RegionalOffice = obj.RegionalOffice;
                DMObjExisting.OfficeName = obj.OfficeName;
                DMObjExisting.OfficeAddress = obj.Address;
                DMObjExisting.PropertyType = obj.PropertyType;
                DMObjExisting.LeaseStartDate = obj.LeaseStartDate;
                DMObjExisting.LeaseStartRentalAmount = obj.LeaseStartingAmount;
                DMObjExisting.RentalEscallation = obj.RentalEscallation;
                DMObjExisting.EscallationPeriod = obj.EscallationPeriod;
                DMObjExisting.LeasePeriod = obj.LeasePeriod;
                DMObjExisting.LeaseClouserDate = obj.LeaseClouserDate;
                DMObjExisting.Security_Deposit = obj.SecurityDeposit;
                DMObjExisting.AdvanceRental = obj.AdvanceRental;
                DMObjExisting.TotalAmountHoldWithOwner = obj.TotalAmountHoldWithOwner;
                DMObjExisting.FitoutsNew = obj.FitOuts;
                DMObjExisting.Car_Park = obj.NoOfCarParking;
                DMObjExisting.NoticePeriod = obj.NoticePeriod;
                DMObjExisting.SignageRoads = obj.Signage;
                DMObjExisting.NoOfEmployee = obj.NoOfEmployee;
                DMObjExisting.SuperBuilt_UpArea = obj.SuperBuiltUpArea;
                DMObjExisting.BuiltUp_area = obj.BuiltUpArea;
                DMObjExisting.Carpet_Area = obj.CarpetArea;
                DMObjExisting.Rental_Area = obj.RentalArea;
                DMObjExisting.PresentRentalCost = obj.PresentRentalCost;
                DMObjExisting.PresentMonthlyRentalCost = obj.PresentMonthlyRentalCost;
                DMObjExisting.PresentMonthlyBilling = obj.PresentMonthlyBilling;
                DMObjExisting.RenatlCostPercentage = obj.RenatlCostPercentage;
                DMObjExisting.MonthlyMaintenanceCost = obj.MonthlyMaintenanceCost;
                DMObjExisting.AvgMonthltMaintenanceCost = obj.AvgMonthltMaintenanceCost;
                DMObjExisting.MonthlyElectricityCost = obj.MonthlyElectricityCost;
                DMObjExisting.MonthlyAllOtherCosts = obj.MonthlyAllOtherCosts;
                DMObjExisting.TotalMonthlyRentalCost = obj.TotalMonthlyRentalCost;
                DMObjExisting.Created_By_Name = obj.Name;
                DMObjExisting.Created_By_Email = obj.Email;
                DMObjExisting.Created_By_Phone = obj.Mobile;
                DMObjExisting.Remarks = obj.Remarks;

                ExistingRentalDataModel.Add(DMObjExisting);

        }


        byte[] filecontent = ExcelExportExtension.ExportToExcel(ExistingRentalDataModel, "FurnishedRentals", null, false, userDateFormat, columnDisplayName);
            return File(filecontent, ExcelExportExtension.ExcelContentType, $"ExistingRentalDetails_{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx");
    }
}
}