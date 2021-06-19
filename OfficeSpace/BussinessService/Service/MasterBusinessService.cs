using OfficeSpace.Models;
using OfficeSpace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace OfficeSpace.BussinessService
{
    public class MasterBusinessService: BaseBussiness, IMasterBussinessService
    {
        private IMasterRepositoryService _masterRepositoryService;
        public MasterBusinessService(IMasterRepositoryService masternRepositoryService, ILogService logService) : base(logService)
        {
            _masterRepositoryService = masternRepositoryService;
        }
       

        public List<string> GetBranchList(string City)
        {
            try
            {
                return _masterRepositoryService.GetBranchList(City);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetCityList()
        {
            try
            {
                return _masterRepositoryService.GetCityList();
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public List<string> GetCompanyList(string roleName, string userName)
        {
            try
            {
                return _masterRepositoryService.GetCompanyList(roleName, userName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ExpiringPeriod> ExpiringPeriods()
        {
            try
            {
                return _masterRepositoryService.ExpiringPeriods();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetStatus(string Username)
        {
            try
            {
                return _masterRepositoryService.GetStatus(Username);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}