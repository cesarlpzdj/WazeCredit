using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecaster : IMarketForecaster
    {
        public MarketResult GetMarketForecast()
        {
            // Call API to get market data
            return new MarketResult
            {
                MarketCondition = MarketCondition.StableUp
            };
        }
    }
}
