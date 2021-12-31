using Corto.BL.Adapters;
using Corto.BL.Services;
using Corto.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;

namespace Corto.Tests
{
    [TestClass]
    public class KeyRangeServiceTest
    {
        private IKeyRangeService _keyRangeServiceTest;
        private const int _startRange = 0;
        private const int _endRange = 1000;

        [TestInitialize]
        public void Initialize()
        {
            var sqlRepositoryMock = new Mock<ISqlRepository>();

            var mockResult = new DataSet();

            var rangeKeyTable = mockResult.Tables.Add("range_key");

            rangeKeyTable.Columns.Add("start_range", typeof(int));
            rangeKeyTable.Columns.Add("end_range", typeof(int));

            var row = rangeKeyTable.NewRow();
            row["start_range"] = _startRange;
            row["end_range"] = _endRange;
            rangeKeyTable.Rows.Add(row);

            var dataRowToKeyRangeServiceResponseAdapter = new DataRowToKeyRangeServiceResponseAdapter();

            sqlRepositoryMock.Setup(p => p.ExecuteStoredProcedure("get_and_mark_key_range")).Returns(mockResult);

            _keyRangeServiceTest = new KeyRangeService(sqlRepositoryMock.Object, dataRowToKeyRangeServiceResponseAdapter);
        }

        [TestMethod]
        public void Should_get_key_range()
        {
            var res = _keyRangeServiceTest.GetAndMarkKeyRange();
            Assert.IsTrue(res.StartRange == 0 && res.EndRange == 1000);
        }
    }
}
