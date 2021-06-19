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
    public class MasterRepositoryService : BaseRepository, IMasterRepositoryService
    {
       

        public MasterRepositoryService(ILogService logService) : base(logService)
        {

        }
        public List<ExpiringPeriod> ExpiringPeriods()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "UDP_OfficeSpace_FetchExpiringPriods";
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adpt = new SqlDataAdapter();
                adpt.SelectCommand = command;
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    return dt.ToList<ExpiringPeriod>();
                }
            }
            return null;
        }

        public List<string> GetCompanyList(string roleName, string userName)
        {
            var result = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (roleName == "User")
                {
                    //command.CommandText = @"SELECT CompanyMaster.CompanyName FROM UserDetails JOIN CompanyMaster 
                    //                        ON CompanyMaster.CompanyName = UserDetails.CompanyName
                    //                        WHERE  RoleName = 'User' and UserName = '" + userName + "'";
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "'  and  RoleName = 'User') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                else
                {
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return result;
        }

        public List<string> GetCityList()
        {
            var result  = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT City FROM CityMaster ORDER BY City";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return result;
        }

        public List<string> GetBranchList(string City)
        {
          var  BranchList = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT distinct OfficeName FROM [FurnishedRentalDetails] ORDER BY OfficeName";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BranchList.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return BranchList;
        }

        public List<string> GetStatus(string Username)
        {
            var result = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();
                command.CommandText = "udp_Store_GetStatus";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", Username);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return result;
        }



    }
}