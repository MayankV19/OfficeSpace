using OfficeSpace.Models;
using OfficeSpace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.BussinessService
{
    public class ReportBusinessService: BaseBussiness, IReportBussinessService
    {
        private IReportRepositorySerivce _reportRepositorySerivce;
        public ReportBusinessService(IReportRepositorySerivce reportRepositorySerivce, ILogService logService) : base(logService)
        {
            _reportRepositorySerivce = reportRepositorySerivce;
        }

        public LeaseReportData SearchLeaseExpiringReportData(ExpiringLeaseSearchModel expiringLeaseSearchModel)
        {
            return _reportRepositorySerivce.SearchLeaseExpiringReportData(expiringLeaseSearchModel);
        }
        public LeaseReportDetailsData SearchLeaseExpiringReportDetailsData(ExpiringLeaseSearchModel expiringLeaseSearchModel)
        {
            return _reportRepositorySerivce.SearchLeaseExpiringReportDetailsData(expiringLeaseSearchModel); 
        }
    }
}