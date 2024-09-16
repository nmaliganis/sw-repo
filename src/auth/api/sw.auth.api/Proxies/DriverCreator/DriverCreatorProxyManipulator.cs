using System;
using sw.auth.api.Proxies.Base;
using sw.auth.common.infrastructure.Commanding;
using sw.auth.common.infrastructure.Commanding.Events;
using sw.auth.common.infrastructure.Commanding.Listeners;
using sw.auth.common.infrastructure.Commanding.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.api.Proxies.DriverCreator;

/// <summary>
/// Class : DriverCreatorProxyManipulator
/// </summary>
public class DriverCreatorProxyManipulator : BaseProxyManipulator, IDriverCreatorProxyManipulator, IDriverCreationNotificationActionListener
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceProvider _service;
    private readonly IMediator _mediator;
    private readonly IBus _bus;

    /// <summary>
    /// Property : Configuration
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Constructor : IoTMessageProxyManipulator
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="scopeFactory"></param>
    /// <param name="service"></param>
    /// <param name="mediator"></param>
    /// <param name="bus"></param>
    public DriverCreatorProxyManipulator(IConfiguration configuration,
        IServiceScopeFactory scopeFactory,
        IServiceProvider service, IMediator mediator, IBus bus)
        : base(scopeFactory)
    {
        Configuration = configuration;

        _scopeFactory = scopeFactory;
        _service = service;
        _mediator = mediator;

        _bus = bus;
    }

    /// <summary>
    /// Method : ProxyInitializer
    /// </summary>
    public void ProxyInitializer()
    {
        Commander.Instance.Attach((IDriverCreationNotificationActionListener)this);
    }

    /// <summary>
    /// Method : Update
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Update(object sender, DriverCreationEventArgs e)
    {
        _bus.Publish<DriverCreationReceived>(new DriverCreationReceived()
        {
            CorrelationId = Guid.NewGuid(),
            Firstname = e.Firstname,
            Lastname = e.Lastname,
            Gender = e.Gender,
            Timestamp = DateTime.UtcNow
        });
    }
} // Class : DriverCreatorProxyManipulator