using System;
using sw.admin.api.Proxies.Base;
using sw.auth.messaging.Commanding;
using sw.auth.messaging.Commanding.Events;
using sw.auth.messaging.Commanding.Listeners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace sw.admin.api.Proxies
{
    /// <summary>
    /// Class : MemberRegisteredProxyManipulator
    /// </summary>
    public class MemberRegisteredProxyManipulator : BaseProxyManipulator, IMemberRegisteredProxyManipulator,
        IMemberRegistrationNotificationActionListener
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _service;

        /// <summary>
        /// Property : Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor : MemberRegisteredProxyManipulator
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="service"></param>
        public MemberRegisteredProxyManipulator(IConfiguration configuration, 
            IServiceScopeFactory scopeFactory, 
            IServiceProvider service) 
            : base(scopeFactory)
        {
            Configuration = configuration;

            _scopeFactory = scopeFactory;
            _service = service;
        }

        /// <summary>
        /// Method : ProxyInitializer
        /// </summary>
        public void ProxyInitializer()
        {
            MessageCommander.Instance.Attach((IMemberRegistrationNotificationActionListener)this);
        }

        /// <summary>
        /// Method : Update IMemberRegistrationNotificationActionListener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Update(object sender, MemberRegistrationEventArgs e)
        {
            Log.Information($"********************************************************\r\n");
            Log.Information($"Member Id:{e.IsEnabled}\r\n");
            Log.Information($"********************************************************\r\n");
        }

    } // Class : MemberRegisteredProxyManipulator
}// Namespace : sw.admin.api.Proxies
