using OfficeSpace.BussinessService;
using OfficeSpace.Extension;
using OfficeSpace.Models;
using OfficeSpace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeSpace.Controllers
{
    public class RequestController : BaseController
    {
        private readonly IRequestBusinessService _RequestBussinessService;
        private readonly IMasterBussinessService _MasterBussinessService;

        public object ViewBox { get; private set; }

        public RequestController(IRequestBusinessService RequestBussinessService, IMasterBussinessService MasterBussinessService, ILogService logService) : base(logService)
        {
            _RequestBussinessService = RequestBussinessService;
            _MasterBussinessService = MasterBussinessService;
        }
        public ActionResult test()
        {
            return  View();
        }
        public ActionResult Index()
        {
            var request = (RequestModel)ViewData["RequestDetails"];
            ViewData["RequestDetails"] = request;
            if (request.Branch == "New")
            {
                return View("NewRequest");
            }
            else
            {
                return View("RellocateRequest");
            }
        }

        public ActionResult InitiatedRequests(int RequestId)
        {
            Session["InitiateRequestID"] = RequestId;
            RequestModel request = new RequestModel();
            ViewBag.pageSize = 10;
            request.FurnishedDataList = _RequestBussinessService.InitiatedRequests(RequestId);
            return View(request);
        }

        public ActionResult IdentifiedRequests(int RequestId)
        {
            Session["IdentifyRequestID"] = RequestId;
            RequestModel request = new RequestModel();
            ViewBag.pageSize = 10;
            request.FurnishedDataList = _RequestBussinessService.IdentifiedRequests(RequestId);
            return View(request);
        }

        public ActionResult ApprovedRequests(int RequestId)
        {
            Session["ApprovedRequestID"] = RequestId;
            RequestModel request = new RequestModel();
            ViewBag.pageSize = 10;
            request.FurnishedDataList = _RequestBussinessService.ApprovedRequests(RequestId);
            return View(request);
        }

        public ActionResult FreshRequests(int RequestId)
        {
            Session["FreshRequestID"] = RequestId;
            RequestModel request = new RequestModel();
            ViewBag.pageSize = 10;           
            request.FurnishedDataList= _RequestBussinessService.FreshRequests(RequestId);
            return View(request);
        }

        [HttpPost]    
        public ActionResult SearchFreshRequests(RequestModel Model,string Submit)
        {
            switch (Submit)
            {
                case "Initiated":
                int GetStatus= _RequestBussinessService.UpdateFreshStatus(Convert.ToInt32(Session["FreshRequestID"].ToString()), Session["CurrentUserName"].ToString(), "Initiated");
                  
                    Model.ID = Convert.ToInt32(Session["FreshRequestID"].ToString());
                    Model.LoggedInUser = Session["CurrentUserName"].ToString();
                    Model.OfficeName = Request.Form["FurnishedDataList[1].OfficeName"].ToString();
                    Model.ProposedLocation = Request.Form["FurnishedDataList[1].ProposedLocation"].ToString();
                    Model.LeaseStartDate = Request.Form["FurnishedDataList[1].LeaseStartDate"].ToString();
                    Model.LeaseStartAmount = Request.Form["FurnishedDataList[1].LeaseStartAmount"].ToString();
                    Model.RentalEscallation = Request.Form["FurnishedDataList[1].RentalEscallation"].ToString();
                    Model.EscallationPeriod = Request.Form["FurnishedDataList[1].EscallationPeriod"].ToString();
                    Model.LeasePeriod = Request.Form["FurnishedDataList[1].LeasePeriod"].ToString();
                    //Model.LeaseClosureDate = Request.Form["FurnishedDataList[1].LeaseClosureDate"].ToString();
                    Model.AdvanceRental = Request.Form["FurnishedDataList[1].AdvanceRental"].ToString();
                    Model.AmtHoldWithOwner = Request.Form["FurnishedDataList[1].AmtHoldWithOwner"].ToString();
                    Model.NoticePeriod = Request.Form["FurnishedDataList[1].NoticePeriod"].ToString();
                    Model.ProposedSignage = Request.Form["FurnishedDataList[1].ProposedSignage"].ToString();
                    Model.ProposedNoOfPersons = Request.Form["FurnishedDataList[1].ProposedNoOfPersons"].ToString();
                    Model.ProposedSuperBuiltUp = Request.Form["FurnishedDataList[1].ProposedSuperBuiltUp"].ToString();
                    Model.ProposedBuiltUp = Request.Form["FurnishedDataList[1].ProposedBuiltUp"].ToString();
                    Model.ProposedCarpetArea = Request.Form["FurnishedDataList[1].ProposedCarpetArea"].ToString();
                    Model.ProposedRentalArea = Request.Form["FurnishedDataList[1].ProposedRentalArea"].ToString();
                    Model.ProposedCalcMonthlyCost = Request.Form["FurnishedDataList[1].ProposedCalcMonthlyCost"].ToString();
                    Model.ProposedSecurityDeposit = Request.Form["FurnishedDataList[1].ProposedSecurityDeposit"].ToString();
                    Model.PresentMonthlyRenatlCost = Request.Form["FurnishedDataList[1].PresentMonthlyRenatlCost"].ToString();
                    Model.PresentMonthlyBilling = Request.Form["FurnishedDataList[1].PresentMonthlyBilling"].ToString();
                    Model.RentalCostPer = Request.Form["FurnishedDataList[1].RentalCostPer"].ToString();
                    Model.MonthlyMaintenenceCost = Request.Form["FurnishedDataList[1].MonthlyMaintenenceCost"].ToString();
                    Model.AvgMaintenanceCost = Request.Form["FurnishedDataList[1].AvgMaintenanceCost"].ToString();
                    Model.monthlyElectricCost = Request.Form["FurnishedDataList[1].monthlyElectricCost"].ToString();
                    Model.MonthlyOtherCosts = Request.Form["FurnishedDataList[1].MonthlyOtherCosts"].ToString();
                    Model.TotalMonthlyRenatlCost = Request.Form["FurnishedDataList[1].TotalMonthlyRenatlCost"].ToString();
                    Model.ProposedCarPark = Request.Form["FurnishedDataList[1].ProposedCarPark"].ToString();
                    Model.Name = Request.Form["FurnishedDataList[1].Name"].ToString();
                    Model.Email = Request.Form["FurnishedDataList[1].Email"].ToString();
                    Model.Mobile = Request.Form["FurnishedDataList[1].Mobile"].ToString();
                    Model.ProposedRemarks = Request.Form["FurnishedDataList[1].ProposedRemarks"].ToString();

                    Model.ExistingOfficeName = Model.ExistingOfficeName;
                    //Model.ExistingLocation = Request.Form["FurnishedDataList[1].ExistingLocation"].ToString();
                    //Model.ExistingLeaseStartDate = Request.Form["FurnishedDataList[1].ExistingLeaseStartDate"].ToString();
                    //Model.ExistingLeaseStartAmount = Request.Form["FurnishedDataList[1].ExistingLeaseStartAmount"].ToString();
                    //Model.ExistingRentalEscallation = Request.Form["FurnishedDataList[1].ExistingRentalEscallation"].ToString();
                    //Model.ExistingEscallationPeriod = Request.Form["FurnishedDataList[1].ExistingEscallationPeriod"].ToString();
                    //Model.ExistingLeasePeriod = Request.Form["FurnishedDataList[1].ExistingLeasePeriod"].ToString();
                    ////Model.LeaseClosureDate = Request.Form["FurnishedDataList[1].LeaseClosureDate"].ToString();
                    //Model.ExistingAdvanceRental = Request.Form["FurnishedDataList[1].ExistingAdvanceRental"].ToString();
                    //Model.ExistingAmtHoldWithOwner = Request.Form["FurnishedDataList[1].ExistingAmtHoldWithOwner"].ToString();
                    //Model.ExistingNoticePeriod = Request.Form["FurnishedDataList[1].ExistingNoticePeriod"].ToString();
                    //Model.Signage = Request.Form["FurnishedDataList[1].Signage"].ToString();
                    //Model.NoOfPersons = Request.Form["FurnishedDataList[1].NoOfPersons"].ToString();

                    //Model.SuperBuiltUp = Request.Form["FurnishedDataList[1].SuperBuiltUp"].ToString();
                    //Model.BuiltUp = Request.Form["FurnishedDataList[1].BuiltUp"].ToString();
                    //Model.CarpetArea = Request.Form["FurnishedDataList[1].CarpetArea"].ToString();
                    //Model.RentalArea = Request.Form["FurnishedDataList[1].RentalArea"].ToString();
                    //Model.CalcMonthlyCost = Request.Form["FurnishedDataList[1].CalcMonthlyCost"].ToString();
                    //Model.SecurityDeposit = Request.Form["FurnishedDataList[1].SecurityDeposit"].ToString();
                    //Model.ExistingPresentMonthlyRenatlCost = Request.Form["FurnishedDataList[1].ExistingPresentMonthlyRenatlCost"].ToString();
                    //Model.ExistingPresentMonthlyBilling = Request.Form["FurnishedDataList[1].ExistingPresentMonthlyBilling"].ToString();
                    //Model.ExistingRentalCostPer = Request.Form["FurnishedDataList[1].ExistingRentalCostPer"].ToString();
                    //Model.ExistingMonthlyMaintenenceCost = Request.Form["FurnishedDataList[1].ExistingMonthlyMaintenenceCost"].ToString();
                    //Model.ExistingAvgMaintenanceCost = Request.Form["FurnishedDataList[1].ExistingAvgMaintenanceCost"].ToString();
                    //Model.ExistingmonthlyElectricCost = Request.Form["FurnishedDataList[1].ExistingmonthlyElectricCost"].ToString();
                    //Model.ExistingMonthlyOtherCosts = Request.Form["FurnishedDataList[1].ExistingMonthlyOtherCosts"].ToString();
                    //Model.ExistingTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[1].ExistingTotalMonthlyRenatlCost"].ToString();
                    //Model.ExistingCarPark = Request.Form["FurnishedDataList[1].ExistingCarPark"].ToString();
                    //Model.ExistingName = Request.Form["FurnishedDataList[1].ExistingName"].ToString();
                    //Model.ExistingEmail = Request.Form["FurnishedDataList[1].ExistingEmail"].ToString();
                    //Model.ExistingMobile = Request.Form["FurnishedDataList[1].ExistingMobile"].ToString();
                    //Model.Remarks = Request.Form["FurnishedDataList[1].Remarks"].ToString();

                    ViewBag.MessageInfo = new MessageInfo { Message = "Request has been Initiated Sucessfully !!", HasError = false };
                    _RequestBussinessService.EmailInitiateRequests(Model, false, "ND");
                    return RedirectToAction("Dashboard");
                    break;
                case "Rejected":
                    int GetStatus1 = _RequestBussinessService.UpdateFreshStatus(Convert.ToInt32(Session["FreshRequestID"].ToString()), Session["CurrentUserName"].ToString(), "Rejected");
                    return RedirectToAction("Dashboard");
                    break;
                case "Reviewed":
                    int GetStatus2 = _RequestBussinessService.UpdateFreshStatus(Convert.ToInt32(Session["FreshRequestID"].ToString()), Session["CurrentUserName"].ToString(), "Reviewed");
                    return RedirectToAction("Dashboard");
                    break;
                case "UpdateData":
                    Model.ID = Convert.ToInt32(Session["FreshRequestID"].ToString());
                    Model.LoggedInUser = Session["CurrentUserName"].ToString();
                   Model.OfficeName= Request.Form["FurnishedDataList[1].OfficeName"].ToString();
                    Model.ProposedLocation = Request.Form["FurnishedDataList[1].ProposedLocation"].ToString();
                    Model.LeaseStartDate = Request.Form["FurnishedDataList[1].LeaseStartDate"].ToString();
                    Model.LeaseStartAmount = Request.Form["FurnishedDataList[1].LeaseStartAmount"].ToString();
                    Model.RentalEscallation = Request.Form["FurnishedDataList[1].RentalEscallation"].ToString();
                    Model.EscallationPeriod = Request.Form["FurnishedDataList[1].EscallationPeriod"].ToString();
                    Model.LeasePeriod = Request.Form["FurnishedDataList[1].LeasePeriod"].ToString();
                    //Model.LeaseClosureDate = Request.Form["FurnishedDataList[1].LeaseClosureDate"].ToString();
                    Model.AdvanceRental = Request.Form["FurnishedDataList[1].AdvanceRental"].ToString();
                    Model.AmtHoldWithOwner = Request.Form["FurnishedDataList[1].AmtHoldWithOwner"].ToString();
                    Model.NoticePeriod = Request.Form["FurnishedDataList[1].NoticePeriod"].ToString();
                    Model.ProposedSignage = Request.Form["FurnishedDataList[1].ProposedSignage"].ToString();
                    Model.ProposedNoOfPersons = Request.Form["FurnishedDataList[1].ProposedNoOfPersons"].ToString();
                    Model.ProposedSuperBuiltUp = Request.Form["FurnishedDataList[1].ProposedSuperBuiltUp"].ToString();
                    Model.ProposedBuiltUp = Request.Form["FurnishedDataList[1].ProposedBuiltUp"].ToString();
                    Model.ProposedCarpetArea = Request.Form["FurnishedDataList[1].ProposedCarpetArea"].ToString();
                    Model.ProposedRentalArea = Request.Form["FurnishedDataList[1].ProposedRentalArea"].ToString();
                    Model.ProposedCalcMonthlyCost = Request.Form["FurnishedDataList[1].ProposedCalcMonthlyCost"].ToString();
                    Model.ProposedSecurityDeposit = Request.Form["FurnishedDataList[1].ProposedSecurityDeposit"].ToString();
                    Model.PresentMonthlyRenatlCost = Request.Form["FurnishedDataList[1].PresentMonthlyRenatlCost"].ToString();
                    Model.PresentMonthlyBilling = Request.Form["FurnishedDataList[1].PresentMonthlyBilling"].ToString();
                    Model.RentalCostPer = Request.Form["FurnishedDataList[1].RentalCostPer"].ToString();
                    Model.MonthlyMaintenenceCost = Request.Form["FurnishedDataList[1].MonthlyMaintenenceCost"].ToString();
                    Model.AvgMaintenanceCost = Request.Form["FurnishedDataList[1].AvgMaintenanceCost"].ToString();
                    Model.monthlyElectricCost = Request.Form["FurnishedDataList[1].monthlyElectricCost"].ToString();
                    Model.MonthlyOtherCosts = Request.Form["FurnishedDataList[1].MonthlyOtherCosts"].ToString();
                    Model.TotalMonthlyRenatlCost = Request.Form["FurnishedDataList[1].TotalMonthlyRenatlCost"].ToString();
                    Model.ProposedCarPark = Request.Form["FurnishedDataList[1].ProposedCarPark"].ToString();
                    Model.Name = Request.Form["FurnishedDataList[1].Name"].ToString();
                    Model.Email = Request.Form["FurnishedDataList[1].Email"].ToString();
                    Model.Mobile = Request.Form["FurnishedDataList[1].Mobile"].ToString();
                    Model.ProposedRemarks = Request.Form["FurnishedDataList[1].ProposedRemarks"].ToString();
                    _RequestBussinessService.UpdateFreshRequests(Model);
                    return RedirectToAction("FreshRequests", "Request", new { @RequestId = Convert.ToInt32(Session["FreshRequestID"].ToString()) });
                  //  return RedirectToAction("FreshRequests", Convert.ToInt32(Session["FreshRequestID"].ToString()));
                    break;
                case "Back":
                    return RedirectToAction("Dashboard");
                    break;
            }
            return RedirectToAction("FreshRequests");

        }

        [HttpPost]
        public ActionResult SearchInitiateRequests(RequestModel Model, string Submit)
        {
            switch (Submit)
            {
                
                case "SubmitData":
                    Model.ID = Convert.ToInt32(Session["InitiateRequestID"].ToString());
                    Model.LoggedInUser = Session["CurrentUserName"].ToString();
                    Model.InitiateOfficeName = Request.Form["FurnishedDataList[2].InitiateOfficeName"].ToString();
                    Model.InitiateLocation = Request.Form["FurnishedDataList[2].InitiateLocation"].ToString();
                    Model.InitiateLeaseStartDate = Request.Form["FurnishedDataList[2].InitiateLeaseStartDate"].ToString();
                    Model.InitiateLeaseStartAmount = Request.Form["FurnishedDataList[2].InitiateLeaseStartAmount"].ToString();
                    Model.InitiateRentalEscallation = Request.Form["FurnishedDataList[2].InitiateRentalEscallation"].ToString();
                    Model.InitiateEscallationPeriod = Request.Form["FurnishedDataList[2].InitiateEscallationPeriod"].ToString();
                    Model.InitiateLeasePeriod = Request.Form["FurnishedDataList[2].InitiateLeasePeriod"].ToString();
                    //Model.LeaseClosureDate = Request.Form["FurnishedDataList[2].LeaseClosureDate"].ToString();
                    Model.InitiateLeaseAdvanceRental = Request.Form["FurnishedDataList[2].InitiateLeaseAdvanceRental"].ToString();
                    Model.InitiateAmtHoldWithOwner = Request.Form["FurnishedDataList[2].InitiateAmtHoldWithOwner"].ToString();
                    Model.InitiateNoticePeriod = Request.Form["FurnishedDataList[2].InitiateNoticePeriod"].ToString();
                    Model.InitiateSignage = Request.Form["FurnishedDataList[2].InitiateSignage"].ToString();
                    Model.InitiateNoOfPerson = Request.Form["FurnishedDataList[2].InitiateNoOfPerson"].ToString();
                    Model.InitiateSuperBuiltUp = Request.Form["FurnishedDataList[2].InitiateSuperBuiltUp"].ToString();
                    Model.InitiateBuiltup = Request.Form["FurnishedDataList[2].InitiateBuiltup"].ToString();
                    Model.InitiateCarpetArea = Request.Form["FurnishedDataList[2].InitiateCarpetArea"].ToString();
                    Model.InitiateRentalArea = Request.Form["FurnishedDataList[2].InitiateRentalArea"].ToString();
                    Model.InitiateRentalCost = Request.Form["FurnishedDataList[2].InitiateRentalCost"].ToString();
                    Model.InitiateSecurityDeposit = Request.Form["FurnishedDataList[2].InitiateSecurityDeposit"].ToString();
             //       Model.InitiateTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[2].PresentMonthlyRenatlCost"].ToString();
                    Model.InitiateMonthlyRentalCost = Request.Form["FurnishedDataList[2].InitiateMonthlyRentalCost"].ToString();
                    Model.InitiateMonthlyBilling = Request.Form["FurnishedDataList[2].InitiateMonthlyBilling"].ToString();
                    Model.InitiateRentalCostPer = Request.Form["FurnishedDataList[2].InitiateRentalCostPer"].ToString();
                    Model.InitiatemonthlyMaintenanceCost = Request.Form["FurnishedDataList[2].InitiatemonthlyMaintenanceCost"].ToString();
                    Model.InitiateAvgMaintenanceCost = Request.Form["FurnishedDataList[2].InitiateAvgMaintenanceCost"].ToString();
                    Model.InitiateMonthlyElectricCost = Request.Form["FurnishedDataList[2].InitiateMonthlyElectricCost"].ToString();
                    Model.InitiateMonthlyOtherCosts = Request.Form["FurnishedDataList[2].InitiateMonthlyOtherCosts"].ToString();
                    Model.InitiateTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[2].InitiateTotalMonthlyRenatlCost"].ToString();
                    Model.InitiateCarpark = Request.Form["FurnishedDataList[2].InitiateCarpark"].ToString();
                    Model.InitiateName = Request.Form["FurnishedDataList[2].InitiateName"].ToString();
                    Model.InitiateEmail = Request.Form["FurnishedDataList[2].InitiateEmail"].ToString();
                    Model.InitiateMobile = Request.Form["FurnishedDataList[2].InitiateMobile"].ToString();
                    Model.InitiateRemarks = Request.Form["FurnishedDataList[2].InitiateRemarks"].ToString();

                    _RequestBussinessService.SubmitInitiateRequests(Model);
                    return RedirectToAction("Dashboard");
                    //  return RedirectToAction("FreshRequests", Convert.ToInt32(Session["FreshRequestID"].ToString()));
                    break;
                case "Back":
                    return RedirectToAction("Dashboard");
                    break;
            }
            return RedirectToAction("InitiatedRequests");

        }


        [HttpPost]
        public ActionResult SearchApprovedRequests(RequestModel Model, string Submit)
        {
            switch (Submit)
            {

                case "StatusUpdate":
                    Model.ID = Convert.ToInt32(Session["ApprovedRequestID"].ToString());
                    Model.LoggedInUser = Session["CurrentUserName"].ToString();
                    Model.IdentifiedOfficeName = Request.Form["FurnishedDataList[3].IdentifiedOfficeName"].ToString();
                    Model.IdentifiedLocation = Request.Form["FurnishedDataList[3].IdentifiedLocation"].ToString();
                    Model.IdentifiedLeaseStartDate = Request.Form["FurnishedDataList[3].IdentifiedLeaseStartDate"].ToString();
                    Model.IdentifiedLeaseStartAmount = Request.Form["FurnishedDataList[3].IdentifiedLeaseStartAmount"].ToString();
                    Model.IdentifiedRentalEscallation = Request.Form["FurnishedDataList[3].IdentifiedRentalEscallation"].ToString();
                    Model.IdentifiedEscallationPeriod = Request.Form["FurnishedDataList[3].IdentifiedEscallationPeriod"].ToString();
                    Model.IdentifiedLeasePeriod = Request.Form["FurnishedDataList[3].IdentifiedLeasePeriod"].ToString();
                    //Model.LeaseClosureDate = Request.Form["FurnishedDataList[3].LeaseClosureDate"].ToString();
                    Model.IdentifiedLeaseAdvanceRental = Request.Form["FurnishedDataList[3].IdentifiedLeaseAdvanceRental"].ToString();
                    Model.IdentifiedAmtHoldWithOwner = Request.Form["FurnishedDataList[3].IdentifiedAmtHoldWithOwner"].ToString();
                    Model.IdentifiedNoticePeriod = Request.Form["FurnishedDataList[3].IdentifiedNoticePeriod"].ToString();
                    Model.IdentifiedSignage = Request.Form["FurnishedDataList[3].IdentifiedSignage"].ToString();
                    Model.IdentifiedNoOfPerson = Request.Form["FurnishedDataList[3].IdentifiedNoOfPerson"].ToString();
                    Model.IdentifiedSuperBuiltUp = Request.Form["FurnishedDataList[3].IdentifiedSuperBuiltUp"].ToString();
                    Model.IdentifiedBuiltup = Request.Form["FurnishedDataList[3].IdentifiedBuiltup"].ToString();
                    Model.IdentifiedCarpetArea = Request.Form["FurnishedDataList[3].IdentifiedCarpetArea"].ToString();
                    Model.IdentifiedRentalArea = Request.Form["FurnishedDataList[3].IdentifiedRentalArea"].ToString();
                    Model.IdentifiedRentalCost = Request.Form["FurnishedDataList[3].IdentifiedRentalCost"].ToString();
                    Model.IdentifiedSecurityDeposit = Request.Form["FurnishedDataList[3].IdentifiedSecurityDeposit"].ToString();
                    //       Model.IdentifiedTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[3].PresentMonthlyRenatlCost"].ToString();
                    Model.IdentifiedMonthlyRentalCost = Request.Form["FurnishedDataList[3].IdentifiedMonthlyRentalCost"].ToString();
                    Model.IdentifiedMonthlyBilling = Request.Form["FurnishedDataList[3].IdentifiedMonthlyBilling"].ToString();
                    Model.IdentifiedRentalCostPer = Request.Form["FurnishedDataList[3].IdentifiedRentalCostPer"].ToString();
                    Model.IdentifiedmonthlyMaintenanceCost = Request.Form["FurnishedDataList[3].IdentifiedmonthlyMaintenanceCost"].ToString();
                    Model.IdentifiedAvgMaintenanceCost = Request.Form["FurnishedDataList[3].IdentifiedAvgMaintenanceCost"].ToString();
                    Model.IdentifiedMonthlyElectricCost = Request.Form["FurnishedDataList[3].IdentifiedMonthlyElectricCost"].ToString();
                    Model.IdentifiedMonthlyOtherCosts = Request.Form["FurnishedDataList[3].IdentifiedMonthlyOtherCosts"].ToString();
                    Model.IdentifiedTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[3].IdentifiedTotalMonthlyRenatlCost"].ToString();
                    Model.IdentifiedCarpark = Request.Form["FurnishedDataList[3].IdentifiedCarpark"].ToString();
                    Model.IdentifiedName = Request.Form["FurnishedDataList[3].IdentifiedName"].ToString();
                    Model.IdentifiedEmail = Request.Form["FurnishedDataList[3].IdentifiedEmail"].ToString();
                    Model.IdentifiedMobile = Request.Form["FurnishedDataList[3].IdentifiedMobile"].ToString();
                    Model.IdentifiedRemarks = Request.Form["FurnishedDataList[3].IdentifiedRemarks"].ToString();

                    _RequestBussinessService.SubmitClosureRequests(Model);
                    return RedirectToAction("Dashboard");
                    //  return RedirectToAction("FreshRequests", Convert.ToInt32(Session["FreshRequestID"].ToString()));
                    break;
                case "Back":
                    return RedirectToAction("Dashboard");
                    break;
            }
            return RedirectToAction("InitiatedRequests");

        }

        [HttpPost]
        public ActionResult SearchIdentifiedRequests(RequestModel Model, string Submit)
        {
            switch (Submit)
            {

                case "Approve":
                    int GetStatus = _RequestBussinessService.UpdateIdentifyStatus(Convert.ToInt32(Session["IdentifyRequestID"].ToString()), Session["CurrentUserName"].ToString(), "Approved");
                    return RedirectToAction("Dashboard");
                    break;
                case "Reject":
                    int GetStatus1 = _RequestBussinessService.UpdateIdentifyStatus(Convert.ToInt32(Session["IdentifyRequestID"].ToString()), Session["CurrentUserName"].ToString(), "Rejected");
                    return RedirectToAction("Dashboard");
                    break;
                case "UpdateData":
                    Model.ID = Convert.ToInt32(Session["IdentifyRequestID"].ToString());
                    Model.LoggedInUser = Session["CurrentUserName"].ToString();
                    Model.InitiateOfficeName = Request.Form["FurnishedDataList[2].InitiateOfficeName"].ToString();
                    Model.InitiateLocation = Request.Form["FurnishedDataList[2].InitiateLocation"].ToString();
                    Model.InitiateLeaseStartDate = Request.Form["FurnishedDataList[2].InitiateLeaseStartDate"].ToString();
                    Model.InitiateLeaseStartAmount = Request.Form["FurnishedDataList[2].InitiateLeaseStartAmount"].ToString();
                    Model.InitiateRentalEscallation = Request.Form["FurnishedDataList[2].InitiateRentalEscallation"].ToString();
                    Model.InitiateEscallationPeriod = Request.Form["FurnishedDataList[2].InitiateEscallationPeriod"].ToString();
                    Model.InitiateLeasePeriod = Request.Form["FurnishedDataList[2].InitiateLeasePeriod"].ToString();
                    //Model.LeaseClosureDate = Request.Form["FurnishedDataList[2].LeaseClosureDate"].ToString();
                    Model.InitiateLeaseAdvanceRental = Request.Form["FurnishedDataList[2].InitiateLeaseAdvanceRental"].ToString();
                    Model.InitiateAmtHoldWithOwner = Request.Form["FurnishedDataList[2].InitiateAmtHoldWithOwner"].ToString();
                    Model.InitiateNoticePeriod = Request.Form["FurnishedDataList[2].InitiateNoticePeriod"].ToString();
                    Model.InitiateSignage = Request.Form["FurnishedDataList[2].InitiateSignage"].ToString();
                    Model.InitiateNoOfPerson = Request.Form["FurnishedDataList[2].InitiateNoOfPerson"].ToString();
                    Model.InitiateSuperBuiltUp = Request.Form["FurnishedDataList[2].InitiateSuperBuiltUp"].ToString();
                    Model.InitiateBuiltup = Request.Form["FurnishedDataList[2].InitiateBuiltup"].ToString();
                    Model.InitiateCarpetArea = Request.Form["FurnishedDataList[2].InitiateCarpetArea"].ToString();
                    Model.InitiateRentalArea = Request.Form["FurnishedDataList[2].InitiateRentalArea"].ToString();
                    Model.InitiateRentalCost = Request.Form["FurnishedDataList[2].InitiateRentalCost"].ToString();
                    Model.InitiateSecurityDeposit = Request.Form["FurnishedDataList[2].InitiateSecurityDeposit"].ToString();
                    //       Model.InitiateTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[2].PresentMonthlyRenatlCost"].ToString();
                    Model.InitiateMonthlyRentalCost = Request.Form["FurnishedDataList[2].InitiateMonthlyRentalCost"].ToString();
                    Model.InitiateMonthlyBilling = Request.Form["FurnishedDataList[2].InitiateMonthlyBilling"].ToString();
                    Model.InitiateRentalCostPer = Request.Form["FurnishedDataList[2].InitiateRentalCostPer"].ToString();
                    Model.InitiatemonthlyMaintenanceCost = Request.Form["FurnishedDataList[2].InitiatemonthlyMaintenanceCost"].ToString();
                    Model.InitiateAvgMaintenanceCost = Request.Form["FurnishedDataList[2].InitiateAvgMaintenanceCost"].ToString();
                    Model.InitiateMonthlyElectricCost = Request.Form["FurnishedDataList[2].InitiateMonthlyElectricCost"].ToString();
                    Model.InitiateMonthlyOtherCosts = Request.Form["FurnishedDataList[2].InitiateMonthlyOtherCosts"].ToString();
                    Model.InitiateTotalMonthlyRenatlCost = Request.Form["FurnishedDataList[2].InitiateTotalMonthlyRenatlCost"].ToString();
                    Model.InitiateCarpark = Request.Form["FurnishedDataList[2].InitiateCarpark"].ToString();
                    Model.InitiateName = Request.Form["FurnishedDataList[2].InitiateName"].ToString();
                    Model.InitiateEmail = Request.Form["FurnishedDataList[2].InitiateEmail"].ToString();
                    Model.InitiateMobile = Request.Form["FurnishedDataList[2].InitiateMobile"].ToString();
                    Model.InitiateRemarks = Request.Form["FurnishedDataList[2].InitiateRemarks"].ToString();

                    _RequestBussinessService.UpdateIdentifyRequests(Model);
                    //   return RedirectToAction("Dashboard");
                    return RedirectToAction("IdentifiedRequests", "Request", new { @RequestId = Convert.ToInt32(Session["IdentifyRequestID"].ToString()) });
                    break;
                case "Back":
                    return RedirectToAction("Dashboard");
                    break;
            }
            return RedirectToAction("InitiatedRequests");

        }

        public ActionResult Dashboard()
        {
            if (Session["CurrentUserName"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                RequestViewModel request = null;
                ViewBag.pageSize = 10;

                if (Session["ApprovalRequest"] != null)
                {
                    ViewBag.ShowDiv = true;
                    request = (RequestViewModel)Session["ApprovalRequest"];
                    //request.SearchParam.UserName = Session["CurrentUserName"].ToString();
                    //request.ApprovalRequests = _RequestBussinessService.GetApprovalRequests(request.SearchParam);

                }
                else
                {
                    ViewBag.ShowDiv = true;
                    request = RequestViewModel.EmptyInstance;

                }
                Session["ApprovalRequest"] = null;
                request.SearchParam.CompanyList = _MasterBussinessService.GetCompanyList("President", Session["CurrentUserName"].ToString());
                request.SearchParam.CityList = _MasterBussinessService.GetCityList();
                request.SearchParam.StatusList = _MasterBussinessService.GetStatus(Session["CurrentUserName"].ToString());
                request.SearchParam.UserName = Session["CurrentUserName"].ToString();
                request.ApprovalRequests = _RequestBussinessService.GetApprovalRequests(request.SearchParam);
                return View(request);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult SearchApprovalRequests(RequestViewModel Model)
        {
          //  Request.Form["ApprovalRequests[0].ID"]
            ViewBag.ShowDiv = true;
            try
            {
                Model.SearchParam.Company = Request.Form["Company"];
                Model.SearchParam.City = Request.Form["City"];
                Model.SearchParam.Status = Request.Form["Status"];
                Session["ApprovalRequest"] = Model;

                return RedirectToAction("Dashboard");


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Home
        public ActionResult NewRequest()
        {

            try
            {
                //   NavigationModel model = new NavigationModel();
                RequestModel request = new RequestModel();
                request.CityList = _MasterBussinessService.GetCityList();

                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (Session["RequestDetails"] != null)
                {
                    var requestDetails = (RequestDetails)Session["RequestDetails"];
                    request.Company = requestDetails.Company;
                    request.Region = requestDetails.Region;
                    request.Branch = requestDetails.Branch;
                }


                return View(request);

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult Parse()
        {
            RequestDetails request = new RequestDetails();
            request.Company = Request.Form["Company"];
            request.Region = Request.Form["Region"];
            request.Branch = Request.Form["Branch"];
            Session["RequestDetails"] = request;
            if (request.Branch == "New")
            {
                return RedirectToAction("NewRequest");
            }
            else
            {
                return RedirectToAction("RelocationRequest");
            }


        }

        public ActionResult RelocationRequest()
        {
            try
            {
                //   NavigationModel model = new NavigationModel();
                RequestModel request = new RequestModel();
                request.CityList = _MasterBussinessService.GetCityList();

                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (Session["RequestDetails"] != null)
                {
                    var requestDetails = (RequestDetails)Session["RequestDetails"];
                    request.Company = requestDetails.Company;
                    request.Region = requestDetails.Region;
                    request.Branch = requestDetails.Branch;
                    _RequestBussinessService.GetExistingData(request);
                }


                return View(request);

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult SaveNewRequest(RequestModel model)
        {
            try
            {

                //RequestModel Nmodel = new RequestModel();
                //   ViewBag.CityList = _MasterBussinessService.GetCityList();
                model.CityList = _MasterBussinessService.GetCityList();
                //  model.City = Request.Form["City"].ToString();
                if (ModelState.IsValid)
                {
                    if (Session["CurrentUserName"] == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    model.LoggedInUser = Session["CurrentUserName"].ToString();
                    int id = _RequestBussinessService.CreateNewRequest(model);
                    model.ID = id;
                    Session["RequestDetails"] = null;

                    ViewBag.MessageInfo = new MessageInfo { Message = "A New Request is Sucessfully Created", HasError = false };
                    _RequestBussinessService.EmailNewRequests(model, false, "ND");
                    //EmailManager emailManager = new EmailManager();
                    //emailManager.EmailIntitate(model, false, "ND");
                    //  return RedirectToAction("Index", "Home");
                    return View("Index");
                }
                else if (!ModelState.IsValid && ModelState.Values.Count == 45)
                {
                    //model = new NavigationModel();
                    //model.GetCityList();
                    //ModelState.AddModelError("", );
                    return RedirectToAction("NewRequest", new object[] { model.Company, model.SelectedMenu, "Looks like you accidentally tried to double post." });
                }
                else
                {
                    return View("NewRequest", model);
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult SaveRelocationRequest(RequestModel model)
        {
            try
            {
                //RequestModel Nmodel = new RequestModel();
                model.CityList = _MasterBussinessService.GetCityList();
                if (ModelState.IsValid)
                {
                    if (Session["CurrentUserName"] == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    model.LoggedInUser = Session["CurrentUserName"].ToString();
                    int id = _RequestBussinessService.CreateRelocationRequest(model);
                    model.ID = id;
                    Session["RequestDetails"] = null;

                    ViewBag.MessageInfo = new MessageInfo { Message = "A Relocation Request is Sucessfully Created", HasError = false };
                    _RequestBussinessService.EmailRelocationRequests(model, false, "ND");
                    //EmailManager emailManager = new EmailManager();
                    //emailManager.EmailIntitate(model, false, "ND");
                    //  return RedirectToAction("Index", "Home");
                    return View("Index");
                }
                else if (!ModelState.IsValid && ModelState.Values.Count == 72)
                {
                    //model = new NavigationModel();
                    //model.GetCityList();
                    //ModelState.AddModelError("", );
                    return RedirectToAction("RelocationRequest", new object[] { model.Company, model.SelectedMenu, "Looks like you accidentally tried to double post." });
                }
                else
                {
                    return View("RelocationRequest", model);
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }



    }
}