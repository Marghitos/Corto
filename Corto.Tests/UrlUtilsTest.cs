using Corto.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Corto.Tests
{
    [TestClass]
    public class UrlUtilsTest
    {
        [TestMethod]
        public void Should_validation_succed_expanded_url()
        {
            var url = "http://google.it";
            Assert.IsTrue(UrlUtils.IsExpandedUrlValid(url));
        }

        [TestMethod]
        public void Should_validation_fail_expanded_url()
        {
            var url = "htp://google.it";
            Assert.IsFalse(UrlUtils.IsExpandedUrlValid(url));
        }

        [TestMethod]
        public void Should_validation_succed_shortened_url()
        {
            var url = "at5";
            Assert.IsTrue(UrlUtils.IsShortenedUrlValid(url));
        }

        [TestMethod]
        public void Should_validation_fail_shortened_url()
        {
            var url = "X6a0e";
            Assert.IsFalse(UrlUtils.IsShortenedUrlValid(url));
        }
    }
}
