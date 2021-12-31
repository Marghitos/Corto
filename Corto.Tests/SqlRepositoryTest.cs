using Corto.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Corto.Tests
{
    [TestClass]
    public class SqlRepositoryTest
    {
        private readonly ISqlRepository _sqlRepository;

        public SqlRepositoryTest()
        {
            var configuration = Utils.InitConfiguration();

            string serverName = configuration.GetSection("SqlServer:ServerName").Value;
            string databaseName = configuration.GetSection("SqlServer:DatabaseName").Value;
            string userName = configuration.GetSection("SqlServer:Username").Value;
            string password = configuration.GetSection("SqlServer:Password").Value;
            string port = configuration.GetSection("SqlServer:Port").Value;

            _sqlRepository = new SqlRepository(serverName, databaseName, userName, password, port);
        }

        [TestMethod]
        public void Should_be_healthy()
        {
            var dataSet = _sqlRepository.ExecuteStoredProcedure("check_health");

            Assert.IsTrue(dataSet.Tables[0].Rows[0]["result"].ToString() == "1");
        }

        [TestMethod]
        public void Should_get_and_mark_key_range()
        {
            var dataSet = _sqlRepository.ExecuteStoredProcedure("get_and_mark_key_range");

            DataTable dataTable = dataSet.Tables[0];

            var startRangeExists = !string.IsNullOrWhiteSpace(dataTable.Rows[0]["start_range"].ToString());
            var endRangeExists = !string.IsNullOrWhiteSpace(dataTable.Rows[0]["end_range"].ToString());

            Assert.IsTrue(startRangeExists && endRangeExists);
        }
    }
}
