using OfficeSpace.Models;
using OfficeSpace.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.BussinessService
{
    public class RequestBusinessService : BaseBussiness, IRequestBusinessService
    {
        private INavigationRepositoryService _navigationRepositoryService;
        public RequestBusinessService(INavigationRepositoryService navigationRepositoryService, ILogService logService) : base(logService)
        {
            _navigationRepositoryService = navigationRepositoryService;
        }
        public int CreateNewRequest(RequestModel newRequest)
        {
            try
            {
                return _navigationRepositoryService.CreateNewRequest(newRequest);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GetExistingData(RequestModel Model)
        {
            try
            {
                _navigationRepositoryService.GetExistingData(Model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<string> GetBranchList(string City)
        {
            try
            {
                return _navigationRepositoryService.GetBranchList(City);
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
                return _navigationRepositoryService.GetCityList();
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
                return _navigationRepositoryService.GetCompanyList(roleName, userName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<UserDataModel> GetUserRequests()
        {
            try
            {
                return _navigationRepositoryService.GetUserRequests();

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
                return _navigationRepositoryService.GetStatus(Username);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int CreateRelocationRequest(RequestModel newRequest)
        {
            try
            {
                return _navigationRepositoryService.CreateRelocationRequest(newRequest);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EmailNewRequests(RequestModel model, bool IsReqMerged, string name)
        {
            try
            {
                _navigationRepositoryService.EmailNewRequests(model, IsReqMerged, name);

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public void EmailRelocationRequests(RequestModel model, bool IsReqMerged, string name)
        {
            try
            {
                _navigationRepositoryService.EmailRelocationRequests(model, IsReqMerged, name);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void ApproveDetails(int ID, string Name)
        {
            try
            {
                _navigationRepositoryService.ApproveDetails(ID, Name);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void DisapproveDetails(int ID, string Name)
        {
            try
            {
                _navigationRepositoryService.DisapproveDetails(ID, Name);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RequestModel> GetApprovalRequests(SearchApprovalRequestModel searchParam)
        {
            try
            {
                return _navigationRepositoryService.GetApprovalRequests(searchParam);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RequestModel> FreshRequests(int ID)
        {
            try
            {
                return _navigationRepositoryService.FreshRequests(ID);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RequestModel> InitiatedRequests(int ID)
        {
            try
            {
                return _navigationRepositoryService.InitiatedRequests(ID);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RequestModel> IdentifiedRequests(int ID)
        {
            try
            {
                return _navigationRepositoryService.IdentifiedRequests(ID);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RequestModel> ApprovedRequests(int ID)
        {

            try
            {
                return _navigationRepositoryService.ApprovedRequests(ID);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int UpdateFreshStatus(int ID, string UserName, string Status)
        {

            try
            {
                return _navigationRepositoryService.UpdateFreshStatus(ID, UserName,Status);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

      public  void UpdateFreshRequests(RequestModel Model)
        {
            try
            {
                 _navigationRepositoryService.UpdateFreshRequests(Model);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public void SubmitInitiateRequests(RequestModel Model)
        {
            try
            {
                _navigationRepositoryService.SubmitInitiateRequests(Model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

      public  int UpdateIdentifyStatus(int ID, string UserName, string Status)
        {
            try
            {
                return _navigationRepositoryService.UpdateIdentifyStatus(ID, UserName, Status);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateIdentifyRequests(RequestModel Model)
        {
            try
            {
                 _navigationRepositoryService.UpdateIdentifyRequests(Model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SubmitClosureRequests(RequestModel Model)
        {
            try
            {
                _navigationRepositoryService.SubmitClosureRequests(Model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void UploadLeaseDocument(int ID, string FileName)
        {
            try
            {
                _navigationRepositoryService.UploadLeaseDocument(ID, FileName);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}

