using Corto.Common.DTO;
using Corto.Common.Interfaces;
using System;
using System.Data;

namespace Corto.BL.Adapters
{
    public class DataRowToKeyRangeServiceResponseAdapter : IAdapter<DataRow, KeyRangeServiceResponse>
    {
        public KeyRangeServiceResponse Adapt(DataRow source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), $"{nameof(source)} source cannot be null");

            return new KeyRangeServiceResponse
            {
                StartRange = Convert.ToInt32(source["start_range"]),
                EndRange = Convert.ToInt32(source["end_range"])
            };
        }
    }
}
