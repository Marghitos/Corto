using System;
using System.Data;
using System.Data.SqlClient;

namespace Corto.Common.DataAccess
{
    public class SqlRepository : ISqlRepository
    {
        private readonly string _connectionString;

        public SqlRepository(string serverName, string databaseName, string userName, string password, string port)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                throw new ArgumentNullException(nameof(serverName), $"{nameof(serverName)} cannot be null or empty");
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException(nameof(databaseName), $"{nameof(databaseName)} cannot be null or empty");
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName), $"{nameof(userName)} cannot be null or empty");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), $"{nameof(password)} cannot be null or empty");
            if (string.IsNullOrWhiteSpace(port))
                throw new ArgumentNullException(nameof(port), $"{nameof(port)} cannot be null or empty");

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = databaseName,
                DataSource = $"{serverName},{port}",
                UserID = userName,
                Password = password
            };

            _connectionString = sqlConnectionStringBuilder.ToString();
        }
        public DataSet ExecuteStoredProcedure(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), $"{nameof(name)} cannot be null or empty");

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    var sqlCommand = new SqlCommand(name, sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    var dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);

                    return dataSet;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
