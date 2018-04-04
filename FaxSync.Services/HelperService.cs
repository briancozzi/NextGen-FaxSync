using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services 
{
    public class HelperService : IHelperService
    {
        string _excludedFaxNumbersSqlConnectionString { get; set; }
        public HelperService()
        {
            _excludedFaxNumbersSqlConnectionString = "Data Source=chvgsql2012;Initial Catalog=NGPortal_QA;User ID=IntranetPortalQA;Password=P@ssword1;";
        }
        public HelperService(ISQLDbConfig sqlConfig)
        {
            _excludedFaxNumbersSqlConnectionString = sqlConfig.FaxBlackListConnectionString;
        }
        public List<string> GetBlackListedFaxNumbers()
        {
            string connetionString = _excludedFaxNumbersSqlConnectionString;
            SqlConnection connection;
            SqlCommand command;
            SqlDataReader dataReader;
            string sql = null;
            var lstFax = new List<string>();
            
            sql = "SELECT * FROM[dbo].[Dropdowns] where LupKey = 'FaxNumber'";
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lstFax.Add(dataReader["LupValue"].ToString());
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
              
            }
            return lstFax;
        }
    }
}
