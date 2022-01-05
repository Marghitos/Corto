using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corto.BL.Services
{
    public class AlgorithmService : IAlgorithmService
    {
        private const string _alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
        private readonly IDictionary<char, int> _alphabetIndex;
        private readonly int _base;

        public AlgorithmService()
        {
            _base = _alphabet.Length;

            _alphabetIndex = _alphabet
                .Select((c, i) => new { Index = i, Char = c })
                .ToDictionary(c => c.Char, c => c.Index);
        }

        public string GenerateShortString(int seed)
        {
            var str = new StringBuilder();
            var i = seed;

            if (i == 0) return _alphabet[0].ToString();

            while (i > 0)
            {
                str.Insert(0, _alphabet[i % _base]);
                i /= _base;
            }

            return str.ToString();
        }

        public int RestoreSeedFromString(string str)
        {
            return str
                    .Aggregate(0, (current, c) => current * _base + _alphabetIndex[c]);
        }
    }
}