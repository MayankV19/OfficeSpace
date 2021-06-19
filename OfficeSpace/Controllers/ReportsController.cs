using OfficeSpace.BussinessService;
using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeSpace.Controllers
{
    public class ReportsController : BaseController
    {
        readonly IReportBussinessService _reportBussinessService;
        IMasterBussinessService _masterBussinessService;
        public ReportsController(IReportBussinessService reportBussinessService, IMasterBussinessService masterBussinessService, ILogService logService) : base(logService)
        {
            _reportBussinessService = reportBussinessService;
            _masterBussinessService = masterBussinessService;

        }
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LeaveExpiringDetails()
        {
            LeaseExpiringReportViewModel model = LeaseExpiringReportViewModel.DefaultInstance;
            if (TempData["LeaseExpiringreportSearchParam"] != null)
            {
                var searchparam = (ExpiringLeaseSearchModel)TempData["LeaseExpiringreportSearchParam"];
                model.SearchParams = searchparam;
            }
            var reportdata = _reportBussinessService.SearchLeaseExpiringReportDetailsData(model.SearchParams);
            model.ExpiringLeaseList = reportdata.ExpiringLeaseList;
            model.Footer = reportdata.Footer;
            model.SearchParams.Cities = _masterBussinessService.GetCityList();
            model.SearchParams.ExpiringPeriods = _masterBussinessService.ExpiringPeriods();
            return View(model);
        }
        public ActionResult LeaveExpiringData()
        {
            LeaseExpiringReportViewDetailModel model = LeaseExpiringReportViewDetailModel.DefaultInstance;
            if (TempData["LeaseExpiringreportSearchParam"] != null)
            {
                var searchparam = (ExpiringLeaseSearchModel)TempData["LeaseExpiringreportSearchParam"];
                model.SearchParams = searchparam;
            }
            var reportdata = _reportBussinessService.SearchLeaseExpiringReportData(model.SearchParams);
            model.ExpiringLeaseList = reportdata.ExpiringLeaseList;
            model.Footer = reportdata.Footer;
            model.SearchParams.Cities = _masterBussinessService.GetCityList();
            model.SearchParams.ExpiringPeriods = _masterBussinessService.ExpiringPeriods();
            return View(model);
        }
        [HttpPost]
        public ActionResult SearchLeaseExpiringDetails()
        {
            try
            {
                ExpiringLeaseSearchModel searchParam = ExpiringLeaseSearchModel.DefaultInstance;
                searchParam.SelectedCity = Convert.ToString(Request.Form["CityList"]);
                searchParam.SelectedPeriod = Convert.ToString(Request.Form["LeaseExpiringPeriodList"]);
                TempData["LeaseExpiringreportSearchParam"] = searchParam;
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("LeaveExpiringDetails");


        }
        [HttpPost]
        public ActionResult SearchLeaseExpiringDataDetails()
        {
            try
            {
                ExpiringLeaseSearchModel searchParam = ExpiringLeaseSearchModel.DefaultInstance;
                searchParam.SelectedCity = Convert.ToString(Request.Form["CityList"]);
                searchParam.SelectedPeriod = Convert.ToString(Request.Form["LeaseExpiringPeriodList"]);
                TempData["LeaseExpiringreportSearchParam"] = searchParam;
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("LeaveExpiringData");


        }
    }
}