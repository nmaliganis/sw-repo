using System.IO;
using System.Reflection;
using dottrack.asset.web.api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace dottrack.test.Integration.tests.Base
{
    public abstract class BaseServer
    {
        public TestServer CreateServerAssetService()
        {
            var path = Assembly.GetAssembly(typeof(BaseServer))
              .Location;

            var hostBuilder = new WebHostBuilder()
              .UseContentRoot(Path.GetDirectoryName(path))
              .ConfigureAppConfiguration(cb =>
              {
                  cb.AddJsonFile("appsettings.json", optional: true)
              .AddEnvironmentVariables();
              })
              .UseStartup<Startup>();


            var testServer = new TestServer(hostBuilder);

            return testServer;
        }

        public TestServer CreateServerAuthenticactionService()
        {
            var path = Assembly.GetAssembly(typeof(BaseServer))
              .Location;

            var hostBuilder = new WebHostBuilder()
              .UseContentRoot(Path.GetDirectoryName(path))
              .ConfigureAppConfiguration(cb =>
              {
                  cb.AddJsonFile("appsettings.authentication.json", optional: true)
              .AddEnvironmentVariables();
              })
              .UseStartup<Startup>();


            var testServer = new TestServer(hostBuilder);

            return testServer;
        }
    }
}