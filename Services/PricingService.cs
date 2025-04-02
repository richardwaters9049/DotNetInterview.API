using DotNetInterview.API.Utilities;

namespace DotNetInterview.API.Services
{
    public class PricingService
    {
        public decimal ApplyDiscounts(decimal originalPrice, int totalQuantity)
        {
            var discount = GetApplicableDiscount(totalQuantity);
            return originalPrice * (1 - discount/100m);
        }

        public decimal GetApplicableDiscount(int quantity)
        {
            if (TimeHelper.IsMondayAfternoonLondon())
                return 50m;
            
            if (quantity > 10) return 20m;
            if (quantity > 5) return 10m;
            
            return 0m;
        }
    }
}