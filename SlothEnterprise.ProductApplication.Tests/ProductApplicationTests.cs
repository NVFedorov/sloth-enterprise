using FluentAssertions;
using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Factory;
using SlothEnterprise.ProductApplication.Products;
using SlothEnterprise.ProductApplication.Strategies;
using SlothEnterprise.ProductApplication.Tests.Helpers;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        private static readonly Mock<ISelectInvoiceService> _selectInvoiceServiceMock = new Mock<ISelectInvoiceService>();
        private static readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceServiceMock = new Mock<IConfidentialInvoiceService>();
        private static readonly Mock<IBusinessLoansService> _businessLoansServiceMock = new Mock<IBusinessLoansService>();

        private readonly ProductApplicationService _sut;
        private readonly Mock<IApplicationResult> _result = new Mock<IApplicationResult>();
        private readonly IProductApplicationServiceFactory _productApplicationFactory;

        private static bool _strategiesInitialized = false;

        private ISellerApplication _sellerApplication;

        public ProductApplicationTests()
        {
            // In real world with DI I would simply use DI to have this factory as a singleton.
            // Or I would implement my own singleton, but as it's out of scope of this task, I just think it's worth to mention
            _productApplicationFactory = new ProductApplicationServiceFactory();
            InitStrategies();
            _sut = new ProductApplicationService(_productApplicationFactory);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithSelectiveInvoiceDiscount_ShouldReturnOne()
        {
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new SelectiveInvoiceDiscount());
            _selectInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(1);
            var result = _sut.SubmitApplicationFor(_sellerApplication);
            result.Should().Be(1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithConfidentialInvoiceDiscount_ShouldReturnOne()
        {
            InitResult(success: true);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new ConfidentialInvoiceDiscount());
            _confidentialInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_result.Object);
            var result = _sut.SubmitApplicationFor(_sellerApplication);
            result.Should().Be(1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithBusinessLoans_ShouldReturnOne()
        {
            InitResult(success: true);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new BusinessLoans());
            _businessLoansServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<LoansRequest>())).Returns(_result.Object);
            var result = _sut.SubmitApplicationFor(_sellerApplication);
            result.Should().Be(1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithSelectiveInvoiceDiscount_OnError_ShouldReturnMinusOne()
        {
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new SelectiveInvoiceDiscount());
            _selectInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(-1);
            var result = _sut.SubmitApplicationFor(_sellerApplication);
            result.Should().Be(-1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithConfidentialInvoiceDiscount_OnError_ShouldReturnMinusOne()
        {
            InitResult(success: false);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new ConfidentialInvoiceDiscount());
            _confidentialInvoiceServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(_result.Object);
            var result = _sut.SubmitApplicationFor(_sellerApplication);
            result.Should().Be(-1);
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithBusinessLoans_OnError_ShouldReturnMinusOne()
        {
            InitResult(success: false);
            _sellerApplication = TestSellerApplicationProvider.GetTestSellerApplication(new BusinessLoans());
            _businessLoansServiceMock.Setup(m => m.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<LoansRequest>())).Returns(_result.Object);
            var result = _sut.SubmitApplicationFor(_sellerApplication);
            result.Should().Be(-1);
        }

        private void InitResult(bool success)
        {
            _result.SetupProperty(p => p.ApplicationId, 1);
            _result.SetupProperty(p => p.Success, success);
        }

        private void InitStrategies()
        {
            // to be honest, I'm not really familiar with xUnit best practicies, so had to add static variable in order to avoid adding same values to dictionary
            if (!_strategiesInitialized)
            {
                _productApplicationFactory.RegisterStrategy<SelectiveInvoiceDiscount>(new SelectiveInvoiceStrategy(_selectInvoiceServiceMock.Object));
                _productApplicationFactory.RegisterStrategy<ConfidentialInvoiceDiscount>(new ConfidentialInvoiceStrategy(_confidentialInvoiceServiceMock.Object));
                _productApplicationFactory.RegisterStrategy<BusinessLoans>(new BusinessLoansStrategy(_businessLoansServiceMock.Object));
                _strategiesInitialized = true;
            }
        }
    }
}