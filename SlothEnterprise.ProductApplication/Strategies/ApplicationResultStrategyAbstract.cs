using SlothEnterprise.External;
using SlothEnterprise.ProductApplication.Applications;

namespace SlothEnterprise.ProductApplication.Strategies
{
    // As there are different types of results I separated the scenario 
    // when the third party service response should be processed.
    // I use this separate class to comply with Single-Responsibility principle
    // and avoid code duplication.
    public abstract class ApplicationResultStrategyAbstract : ProductApplicationStrategyAbstract
    {
        public override int Submit(ISellerApplication application)
        {
            var result = SubmitWithResult(application);
            return (result.Success) ? result.ApplicationId ?? -1 : -1;
        }

        protected abstract IApplicationResult SubmitWithResult(ISellerApplication application);
    }
}
