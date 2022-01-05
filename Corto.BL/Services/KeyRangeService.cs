using Corto.BL.Exceptions;
using Corto.Common.DataAccess;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using System;
using System.Data;

namespace Corto.BL.Services
{
    public class KeyRangeService : IKeyRangeService
    {
        private readonly ISqlRepository _sqlRepository;
        private readonly IAdapter<DataRow, KeyRangeServiceResponse> _dataRowToKeyRangeServiceResponseAdapter;
        private KeyRangeServiceResponse _keyRange;

        public KeyRangeService(ISqlRepository sqlRepository, IAdapter<DataRow, KeyRangeServiceResponse> dataRowToKeyRangeServiceResponseAdapter)
        {
            _sqlRepository = sqlRepository;
            _dataRowToKeyRangeServiceResponseAdapter = dataRowToKeyRangeServiceResponseAdapter;
        }

        private int _counter;
        public int Counter
        {
            get
            {
                if (_keyRange == null || _counter >= _keyRange?.EndRange)
                {
                    _keyRange = GetAndMarkKeyRange();
                    _counter = _keyRange.StartRange;
                }
                return _counter++;
            }
        }

        public KeyRangeServiceResponse GetAndMarkKeyRange()
        {
            var dataSet = _sqlRepository.ExecuteStoredProcedure("get_and_mark_key_range");

            DataTable dataTable = dataSet.Tables[0];

            if (dataTable == null || dataTable?.Rows?.Count == 0)
                throw new KeyRangeServiceException("Result cannot be null or empty");

            return _dataRowToKeyRangeServiceResponseAdapter.Adapt(dataTable.Rows[0]);
        }
    }
}
