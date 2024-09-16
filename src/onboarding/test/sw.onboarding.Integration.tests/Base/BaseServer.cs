using System.IO;
using System.Reflection;
using dottrack.auth.api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace dottrack.auth.Integration.tests.Base {
    public abstract class BaseServer {
        public TestServer CreateServerAuthenticactionService() {
            var path = Assembly.GetAssembly(typeof(BaseServer))
                .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb => {
                    cb.AddJsonFile("appsettings.json", optional: true)
                        .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();


            var testServer = new TestServer(hostBuilder);

            return testServer;
        }
    }
}