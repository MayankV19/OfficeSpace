using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OfficeSpace
{
    public class BAL
    {
        public List<Company> GetCompanyList(string roleName, string userName=null)
        {
            List<Company> result = new List<Company>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (roleName == "User")
                {
                    
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "'  and  RoleName = 'User') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                else
                {
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Company { CompanyId = reader["CompanyName"].ToString(), CompayName = reader["CompanyName"].ToString() });
                        }
                    }
                }

            }
            return result;
        }
    }
}
    