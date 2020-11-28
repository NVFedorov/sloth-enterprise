using SlothEnterprise.External;
using SlothEnterprise.ProductApplication.Applications;

namespace SlothEnterprise.ProductApplication.Extensions
{
    public static class ModelExtensions
    {
        // Normally I would do it with AutoMapper
        public static CompanyDataRequest ToCompanyDataRequest(this ISellerCompanyData seller)
        {
            return new CompanyDataRequest
            {
                CompanyFounded = seller.Founded,
                CompanyNumber = seller.Number,
                CompanyName = seller.Name,
                DirectorName = seller.DirectorName
            };
        }
    }
}
