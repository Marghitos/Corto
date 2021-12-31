namespace Corto.BL.Services
{
    public interface IAlgorithmService
    {
        string GenerateShortString(int seed);
        int RestoreSeedFromString(string str);
    }
}