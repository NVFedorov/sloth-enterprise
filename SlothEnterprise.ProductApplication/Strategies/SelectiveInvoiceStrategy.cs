using System;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Strategies
{
    public class SelectiveInvoiceStrategy : ProductApplicationStrategyAbstract
    {
        private readonly ISelectInvoiceService _selectInvoiceService;

        public SelectiveInvoiceStrategy(ISelectInvoiceService selectInvoiceService)
        {
            _selectInvoiceService = selectInvoiceService;
        }

        public override int Submit(ISellerApplication application)
        {
            if (!(application.Product is SelectiveInvoiceDiscount sid))
                throw new InvalidOperationException(GetErrorMessage(typeof(SelectiveInvoiceDiscount), application.Product.GetType()));

            return _selectInvoiceService.SubmitApplicationFor(application.CompanyData.Number.ToString(), sid.InvoiceAmount, sid.AdvancePercentage);

        }
    }
}
