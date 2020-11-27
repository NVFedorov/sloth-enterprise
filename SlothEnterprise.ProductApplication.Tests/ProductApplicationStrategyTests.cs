using System;
using FluentAssertions;
using Moq;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using SlothEnterprise.ProductApplication.Strategies;
using SlothEnterprise.ProductApplication.Tests.Helpers;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationStrategyTests
    {
        private readonly Mock<ISelectInvoiceService> _selectInvoiceServiceMock = new Mock<ISelectInvoiceService>();
        private readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceServiceMock = new Mock<IConfidentialInvoiceService>();
        private readonly Mock<IBusinessLoansService> _businessLoansServiceMock = new Mock<IBusinessLoansService>();

        private ProductApplicationStrategyAbstract _strategy;
        private ISellerApplication _sellerApplication;

        [Fact]
        public void SelectiveInvoiceStrategy_SubmitWrongProduct_ShouldThrowError()
        {
            _strategy = new SelectiveInvoiceStrategy(_selectInvoiceServiceMock.Object);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new ConfidentialInvoiceDiscount());
            Action submit = () => _strategy.Submit(_sellerApplication);
            submit.Should().Throw<InvalidOperationException>(because: $"The {typeof(ConfidentialInvoiceDiscount).Name} " +
                $"is not acceptable for {typeof(SelectiveInvoiceStrategy).Name}. " +
                $"Expected type: {typeof(SelectiveInvoiceDiscount).Name}");
        }

        [Fact]
        public void ConfidentialInvoiceStrategy_SubmitWrongProduct_ShouldThrowError()
        {
            _strategy = new ConfidentialInvoiceStrategy(_confidentialInvoiceServiceMock.Object);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new SelectiveInvoiceDiscount());
            Action submit = () => _strategy.Submit(_sellerApplication);
            submit.Should().Throw<InvalidOperationException>(because: $"The {typeof(SelectiveInvoiceDiscount).Name} " +
                $"is not acceptable for {typeof(ConfidentialInvoiceStrategy).Name}. " +
                $"Expected type: {typeof(ConfidentialInvoiceDiscount).Name}");
        }

        [Fact]
        public void BusinessLoansStrategy_SubmitWrongProduct_ShouldThrowError()
        {
            _strategy = new BusinessLoansStrategy(_businessLoansServiceMock.Object);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new ConfidentialInvoiceDiscount());
            Action submit = () => _strategy.Submit(_sellerApplication);
            submit.Should().Throw<InvalidOperationException>(because: $"The {typeof(ConfidentialInvoiceDiscount).Name} " +
                $"is not acceptable for {typeof(BusinessLoansStrategy).Name}. " +
                $"Expected type: {typeof(BusinessLoans).Name}");
        }
    }
}
