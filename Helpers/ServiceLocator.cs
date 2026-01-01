namespace calculator.Helpers;

/// <summary>
/// Simple service locator for dependency injection.
/// Provides singleton service registration and resolution.
/// </summary>
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();
    private static readonly Dictionary<Type, Func<object>> _factories = new();

    /// <summary>
    /// Registers a singleton service instance.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <param name="instance">The service instance.</param>
    public static void Register<TService>(TService instance) where TService : class
    {
        _services[typeof(TService)] = instance;
    }

    /// <summary>
    /// Registers a factory for lazy service creation.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <param name="factory">The factory function.</param>
    public static void Register<TService>(Func<TService> factory) where TService : class
    {
        _factories[typeof(TService)] = () => factory();
    }

    /// <summary>
    /// Resolves a registered service.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when service is not registered.</exception>
    public static TService Resolve<TService>() where TService : class
    {
        if (_services.TryGetValue(typeof(TService), out var service))
        {
            return (TService)service;
        }

        if (_factories.TryGetValue(typeof(TService), out var factory))
        {
            var instance = (TService)factory();
            _services[typeof(TService)] = instance; // Cache the instance
            return instance;
        }

        throw new InvalidOperationException($"Service {typeof(TService).Name} is not registered.");
    }

    /// <summary>
    /// Tries to resolve a registered service.
    /// </summary>
    /// <typeparam name="TService">The service interface type.</typeparam>
    /// <param name="service">The resolved service, or null if not found.</param>
    /// <returns>True if the service was resolved; otherwise, false.</returns>
    public static bool TryResolve<TService>(out TService? service) where TService : class
    {
        try
        {
            service = Resolve<TService>();
            return true;
        }
        catch
        {
            service = null;
            return false;
        }
    }

    /// <summary>
    /// Clears all registered services. Useful for testing.
    /// </summary>
    public static void Clear()
    {
        _services.Clear();
        _factories.Clear();
    }
}
