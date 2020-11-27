using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Factory;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService
    {
        private readonly IProductApplicationServiceFactory _serviceFactory;

        public ProductApplicationService(IProductApplicationServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {
            var strategy = _serviceFactory.GetProductApplicationStrategy(application.Product.GetType());
            return strategy.Submit(application);
        }
    }
}
