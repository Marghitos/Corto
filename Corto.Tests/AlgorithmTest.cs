using Corto.BL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Corto.Tests
{
    [TestClass]
    public class AlgorithmTest
    {
        [TestMethod]
        public void Should_generate_short_string()
        {
            var seed = 860250;
            var encodedString = "ifru";

            var algorithmService = new AlgorithmService();
            var res = algorithmService.GenerateShortString(seed);

            Assert.IsTrue(res == encodedString);
        }

        [TestMethod]
        public void Should_restore_seed_from_string()
        {
            var encodedUrl = "h56yt2";
            var seed = 1036648118;

            var algorithmService = new AlgorithmService();
            var res = algorithmService.RestoreSeedFromString(encodedUrl);

            Assert.IsTrue(res == seed);
        }
    }
}
