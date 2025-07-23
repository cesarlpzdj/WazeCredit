using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecasterV2 : IMarketForecaster
    {
        public MarketResult GetMarketForecast()
        {
            // Call API to get market data
            return new MarketResult
            {
                MarketCondition = MarketCondition.Volatile
            };
        }
    }
}
