using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.api.Proxies.Base;

/// <summary>
/// Abstract Class : BaseProxyManipulator
/// </summary>
public abstract class BaseProxyManipulator
{
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Constructor : BaseProxyManipulator
    /// </summary>
    /// <param name="scopeFactory"></param>
    protected BaseProxyManipulator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

} // Abstract Class : BaseProxyManipulator