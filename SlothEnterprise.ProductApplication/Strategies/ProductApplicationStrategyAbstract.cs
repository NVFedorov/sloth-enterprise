using System;
using SlothEnterprise.ProductApplication.Applications;

namespace SlothEnterprise.ProductApplication.Strategies
{
    public abstract class ProductApplicationStrategyAbstract
    {
        public abstract int Submit(ISellerApplication application);

        // Avoiding a code duplication
        protected string GetErrorMessage(Type expected, Type actual)
        {
            return $"The {actual.Name} is not acceptable for {this.GetType().Name}. " +
                    $"Expected type: {expected.Name}";
        }
    }
}
