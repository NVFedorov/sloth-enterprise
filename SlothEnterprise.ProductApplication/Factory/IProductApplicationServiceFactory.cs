using System;
using System.Collections.Generic;
using SlothEnterprise.ProductApplication.Strategies;

namespace SlothEnterprise.ProductApplication.Factory
{
    public interface IProductApplicationServiceFactory
    {
        void RegisterStrategy<T>(ProductApplicationStrategyAbstract strategy);
        ProductApplicationStrategyAbstract GetProductApplicationStrategy<T>();
        ProductApplicationStrategyAbstract GetProductApplicationStrategy(Type type);
    }

    /// <summary>
    /// Represents a strategy factory. Each strategy is responsible for connection between current module and external services.
    /// This factory should be registred in DI container as singleton to have one single registry of the strategies.
    /// It is possible to configure the strategies in configuration file and then use reflection to register all the strategies.
    /// However in my opinion it's an overengineering, because it doesn't bring any benefits (we still need to define strategy and it's IProduct implementation), and highly complicates things.
    /// </summary>
    public class ProductApplicationServiceFactory : IProductApplicationServiceFactory
    {
        private static readonly Dictionary<Type, ProductApplicationStrategyAbstract> _strategies = new Dictionary<Type, ProductApplicationStrategyAbstract>();

        public ProductApplicationStrategyAbstract GetProductApplicationStrategy<T>() => GetProductApplicationStrategy(typeof(T));
        public ProductApplicationStrategyAbstract GetProductApplicationStrategy(Type type) => _strategies[type];
        public void RegisterStrategy<T>(ProductApplicationStrategyAbstract strategy) => _strategies.Add(typeof(T), strategy);
    }
}
