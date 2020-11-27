using System;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Strategies
{
    public class BusinessLoansStrategy : ApplicationResultStrategyAbstract
    {
        private readonly IBusinessLoansService _businessLoansService;

        public BusinessLoansStrategy(IBusinessLoansService businessLoansService)
        {
            _businessLoansService = businessLoansService;
        }

        protected override IApplicationResult SubmitWithResult(ISellerApplication application)
        {
            if (!(application.Product is BusinessLoans loans))
                throw new InvalidOperationException(GetErrorMessage(typeof(BusinessLoans), application.Product.GetType()));

            return _businessLoansService.SubmitApplicationFor(new CompanyDataRequest
                {
                    CompanyFounded = application.CompanyData.Founded,
                    CompanyNumber = application.CompanyData.Number,
                    CompanyName = application.CompanyData.Name,
                    DirectorName = application.CompanyData.DirectorName
                }, new LoansRequest
                {
                    InterestRatePerAnnum = loans.InterestRatePerAnnum,
                    LoanAmount = loans.LoanAmount
                });
        }
    }
}
