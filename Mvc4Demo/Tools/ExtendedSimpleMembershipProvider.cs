using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace Mvc4Demo.Tools
{
    /// <summary>
    /// Custom membership provider for manage authentication
    /// </summary>
    public class ExtendedSimpleMembershipProvider : SimpleMembershipProvider
    {
        private string _connectionString = string.Empty;
     

        /// <summary>
        /// Initialize your memebershipp provider
        /// </summary>
        /// <param name="name">Name of membership provider</param>
        /// <param name="config">All properties of your membership provider</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            _connectionString = Convert.ToString(config["connectionStringName"]);
            base.Initialize(name, config);
        }

        /// <summary>
        /// Validate login passwor for authentication
        /// </summary>
        /// <param name="login">Login of user</param>
        /// <param name="password">Password of user</param>
        /// <returns></returns>
        public override bool ValidateUser(string login, string password)
        {

            bool isValid = false;
            string queryString = " SELECT case when count(*)=1 THEN 1 ELSE 0 END FROM DemoUser WHERE Login=@Login AND Password=@Password ";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command=new SqlCommand(queryString,connection);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                connection.Open();
                isValid = Convert.ToBoolean(command.ExecuteScalar());
            }

            return isValid;
        }
    }
}