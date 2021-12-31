using Corto.Common.DTO;

namespace Corto.BL.Services
{
    public interface IKeyRangeService
    {
        int Counter { get; }

        KeyRangeServiceResponse GetAndMarkKeyRange();
    }
}