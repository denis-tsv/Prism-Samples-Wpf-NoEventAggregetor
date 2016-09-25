


using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace StockTraderRI.Infrastructure.Interfaces
{
    public interface IMarketFeedService
    {
        decimal GetPrice(string tickerSymbol);
        long GetVolume(string tickerSymbol);
        bool SymbolExists(string tickerSymbol);
        //TODO it is better to make Prices readonly
        ObservableDictionary<string, decimal> Prices { get; }
    }
}
