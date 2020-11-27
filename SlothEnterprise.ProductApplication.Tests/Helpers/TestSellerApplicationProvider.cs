using Moq;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Tests.Helpers
{
    internal class TestSellerApplicationProvider
    {
        internal static ISellerApplication GetTestSellerApplication(IProduct product)
        {
            var sellerApplicationMock = new Mock<ISellerApplication>();
            sellerApplicationMock.SetupProperty(p => p.Product, product);
            sellerApplicationMock.SetupProperty(p => p.CompanyData, new SellerCompanyData());
            return sellerApplicationMock.Object;
        }
    }
}
