using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSpace.Services
{
    public interface INavigationRepositoryService
    {
         int CreateNewRequest(RequestModel newRequest);
        List<UserDataModel> GetUserRequests();
        List<string> GetBranchList(string City);
        List<string> GetCityList();
        List<string> GetCompanyList(string roleName, string userName);
         void GetExistingData(RequestModel Model);
        int CreateRelocationRequest(RequestModel newRequest);

        void EmailNewRequests(RequestModel model, bool IsReqMerged, string name);

        void EmailInitiateRequests(RequestModel model, bool IsReqMerged, string name);
        void EmailRelocationRequests(RequestModel model, bool IsReqMerged, string name);

        void ApproveDetails(int ID, string Name);

        void DisapproveDetails(int ID, string Name);

        List<RequestModel> FreshRequests(int ID);
        List<RequestModel> InitiatedRequests(int ID);

        List<RequestModel> IdentifiedRequests(int ID);

        List<RequestModel> ApprovedRequests(int ID);

        List<RequestModel> GetApprovalRequests(SearchApprovalRequestModel searchParam);

        int UpdateFreshStatus(int ID, string UserName, string Status);

        void UpdateFreshRequests(RequestModel Model);

        void SubmitInitiateRequests(RequestModel Model);

        int UpdateIdentifyStatus(int ID, string UserName, string Status);

        void UpdateIdentifyRequests(RequestModel Model);

        void SubmitClosureRequests(RequestModel Model);

        List<string> GetStatus(string Username);
        void UploadLeaseDocument(int ID, string FileName);

    }
}
