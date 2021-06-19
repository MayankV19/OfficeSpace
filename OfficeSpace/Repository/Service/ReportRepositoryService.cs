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
    public class ReportRepositoryService : BaseRepository, IReportRepositorySerivce
    {


        public ReportRepositoryService(ILogService logService) : base(logService)
        {

        }
        public LeaseReportDetailsData SearchLeaseExpiringReportDetailsData(ExpiringLeaseSearchModel expiringLeaseSearchModel)
        {
            LeaseReportDetailsData result = new LeaseReportDetailsData();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "UDP_OfficeSpace_LeaseExpiringReport";
                command.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(expiringLeaseSearchModel.SelectedPeriod))
                {
                    command.Parameters.Add(new SqlParameter("@MonthsToExpire", expiringLeaseSearchModel.SelectedPeriod));
                }
                if (!string.IsNullOrEmpty(expiringLeaseSearchModel.SelectedCity))
                {
                    command.Parameters.Add(new SqlParameter("@City", expiringLeaseSearchModel.SelectedCity));
                }
                SqlDataAdapter adpt = new SqlDataAdapter();
                adpt.SelectCommand = command;
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    var table1 = ds.Tables[0];
                    var table2 = ds.Tables[1];
                    result.ExpiringLeaseList = table1.ToList<LeaseExpiringModel>();
                    result.Footer = table2.ToList<ExpiringLeaseFooterModel>()[0];

                    return result;
                }
            }
            return null;
        }
        public LeaseReportData SearchLeaseExpiringReportData(ExpiringLeaseSearchModel expiringLeaseSearchModel)
        {
            LeaseReportData result = new LeaseReportData();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "UDP_OfficeSpace_LeaseExpiringReportModified";
                command.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(expiringLeaseSearchModel.SelectedPeriod))
                {
                    command.Parameters.Add(new SqlParameter("@MonthsToExpire", expiringLeaseSearchModel.SelectedPeriod));
                }
                if (!string.IsNullOrEmpty(expiringLeaseSearchModel.SelectedCity))
                {
                    command.Parameters.Add(new SqlParameter("@City", expiringLeaseSearchModel.SelectedCity));
                }
                SqlDataAdapter adpt = new SqlDataAdapter();
                adpt.SelectCommand = command;
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    var table1 = ds.Tables[0];
                    var table2 = ds.Tables[1];
                    var table3 = ds.Tables[2];
                    result.ExpiringLeaseList = table1.ToList<LeaseExpiringMasterModel>();
                    var details = table2.ToList<LeaseExpiringDetailModel>();
                    foreach (var master in result.ExpiringLeaseList)
                    {
                        master.Details = new List<LeaseExpiringDetailModel>();
                        master.Details.AddRange(details.Where(det => det.City == master.City));
                    }
                    result.Footer = table3.ToList<ExpiringLeaseFooterModel>()[0];

                    return result;
                }
            }
            return null;
        }




    }
}