namespace Corto.Common.Interfaces
{
    public interface IAdapter<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {   
        TOutput Adapt(TInput source);
    }
}
