

namespace StoreService
{
    public static class TotalPriceChangeManager
    {
        public delegate void TotalPriceChanged(double persentage, PriceChanging option);
        public static TotalPriceChanged EventHandler;
    }
}
