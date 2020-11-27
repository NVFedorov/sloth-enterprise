﻿using System;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Strategies
{
    public class ConfidentialInvoiceStrategy : ApplicationResultStrategyAbstract
    {
        private readonly IConfidentialInvoiceService _confidentialInvoiceWebService;

        public ConfidentialInvoiceStrategy(IConfidentialInvoiceService confidentialInvoiceWebService)
        {
            _confidentialInvoiceWebService = confidentialInvoiceWebService;
        }

        protected override IApplicationResult SubmitWithResult(ISellerApplication application)
        {
            if (!(application.Product is ConfidentialInvoiceDiscount cid))
                throw new InvalidOperationException(GetErrorMessage(typeof(ConfidentialInvoiceDiscount), application.Product.GetType()));

            return _confidentialInvoiceWebService.SubmitApplicationFor(
                   new CompanyDataRequest
                   {
                       CompanyFounded = application.CompanyData.Founded,
                       CompanyNumber = application.CompanyData.Number,
                       CompanyName = application.CompanyData.Name,
                       DirectorName = application.CompanyData.DirectorName
                   }, cid.TotalLedgerNetworth, cid.AdvancePercentage, cid.VatRate);
        }
    }
}
