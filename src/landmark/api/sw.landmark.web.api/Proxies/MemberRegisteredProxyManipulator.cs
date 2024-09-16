using sw.auth.messaging.Commanding;
using sw.auth.messaging.Commanding.Events;
using sw.auth.messaging.Commanding.Listeners;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using sw.landmark.api.Proxies.Base;

namespace sw.landmark.api.Proxies {
    /// <summary>
    /// Class : MemberRegisteredProxyManipulator
    /// </summary>
    public class MemberRegisteredProxyManipulator : BaseProxyManipulator, IMemberRegisteredProxyManipulator,
        IMemberRegistrationNotificationActionListener {
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
            : base(scopeFactory) {
            this.Configuration = configuration;

            this._scopeFactory = scopeFactory;
            this._service = service;
        }

        /// <summary>
        /// Method : ProxyInitializer
        /// </summary>
        public void ProxyInitializer() {
            MessageCommander.Instance.Attach(this);
        }

        /// <summary>
        /// Method : Update IMemberRegistrationNotificationActionListener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Update(object sender, MemberRegistrationEventArgs e) {
            Log.Information($"********************************************************\r\n");
            Log.Information($"Member Id:{e.IsEnabled}\r\n");
            Log.Information($"********************************************************\r\n");
        }

    } // Class : MemberRegisteredProxyManipulator
}// Namespace : sw.landmark.web.api.Proxies
