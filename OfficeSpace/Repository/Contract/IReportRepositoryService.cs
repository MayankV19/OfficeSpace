using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSpace.Services
{
    public interface IReportRepositorySerivce
    {


        LeaseReportData SearchLeaseExpiringReportData(ExpiringLeaseSearchModel expiringLeaseSearchModel);


        LeaseReportDetailsData SearchLeaseExpiringReportDetailsData(ExpiringLeaseSearchModel expiringLeaseSearchModel);




    }
   
}
