using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSpace.Services
{
    public interface IMasterRepositoryService
    {
     
     
        List<string> GetBranchList(string City);
        List<string> GetCityList();
        List<string> GetCompanyList(string roleName, string userName);
        List<ExpiringPeriod> ExpiringPeriods();


        List<string> GetStatus(string Username);

    }
}
