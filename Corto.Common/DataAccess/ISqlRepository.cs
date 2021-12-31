using System.Data;

namespace Corto.Common.DataAccess
{
    public interface ISqlRepository
    {
        DataSet ExecuteStoredProcedure(string name);
    }
}