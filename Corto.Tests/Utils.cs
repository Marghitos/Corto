
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Corto.Tests
{
    public static class Utils
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}
