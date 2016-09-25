using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Properties;
using System.ComponentModel.Composition;
using Prism.Events;

namespace StockTraderRI.Modules.Market.Services
{
    [Export(typeof(IMarketFeedService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MarketFeedService : IMarketFeedService, IDisposable
    {
        private readonly Dictionary<string, long> _volumeList = new Dictionary<string, long>();
        static readonly Random randomGenerator = new Random(unchecked((int)DateTime.Now.Ticks));
        private Timer _timer;
        private int _refreshInterval = 10000;
        private readonly object _lockObject = new object();

        [ImportingConstructor]
        public MarketFeedService()
            : this(XDocument.Parse(Resources.Market))
        {
        }

        public ObservableDictionary<string, decimal> Prices { get; } = new ObservableDictionary<string, decimal>();

        protected MarketFeedService(XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            _timer = new Timer(TimerTick);

            var marketItemsElement = document.Element("MarketItems");
            var refreshRateAttribute = marketItemsElement.Attribute("RefreshRate");
            if (refreshRateAttribute != null)
            {
                RefreshInterval = CalculateRefreshIntervalMillisecondsFromSeconds(int.Parse(refreshRateAttribute.Value, CultureInfo.InvariantCulture));
            }

            var itemElements = marketItemsElement.Elements("MarketItem");
            foreach (XElement item in itemElements)
            {
                string tickerSymbol = item.Attribute("TickerSymbol").Value;
                decimal lastPrice = decimal.Parse(item.Attribute("LastPrice").Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                long volume = Convert.ToInt64(item.Attribute("Volume").Value, CultureInfo.InvariantCulture);
                Prices.Add(tickerSymbol, lastPrice);
                _volumeList.Add(tickerSymbol, volume);
            }
        }

        public int RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                _refreshInterval = value;
                _timer.Change(_refreshInterval, _refreshInterval);
            }
        }

        /// <summary>
        /// Callback for Timer
        /// </summary>
        /// <param name="state"></param>
        private void TimerTick(object state)
        {
            UpdatePrices();
        }

        public decimal GetPrice(string tickerSymbol)
        {
            if (!SymbolExists(tickerSymbol))
                throw new ArgumentException(Resources.MarketFeedTickerSymbolNotFoundException, "tickerSymbol");

            return Prices[tickerSymbol];
        }

        public long GetVolume(string tickerSymbol)
        {
            return _volumeList[tickerSymbol];
        }

        public bool SymbolExists(string tickerSymbol)
        {
            return Prices.ContainsKey(tickerSymbol);
        }

        

        protected void UpdatePrice(string tickerSymbol, decimal newPrice, long newVolume)
        {
            lock (_lockObject)
            {
                Prices[tickerSymbol] = newPrice;
                _volumeList[tickerSymbol] = newVolume;
            }
        }

        protected void UpdatePrices()
        {
            lock (_lockObject)
            {
                foreach (string symbol in Prices.Keys.ToArray())
                {
                    decimal newValue = Prices[symbol];
                    newValue += Convert.ToDecimal(randomGenerator.NextDouble() * 10f) - 5m;
                    Prices[symbol] = newValue > 0 ? newValue : 0.1m;
                }
            }
        }
        
        private static int CalculateRefreshIntervalMillisecondsFromSeconds(int seconds)
        {
            return seconds * 1000;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_timer != null)
                _timer.Dispose();
            _timer = null;
        }

        // Use C# destructor syntax for finalization code.
        ~MarketFeedService()
        {
            Dispose(false);
        }

        #endregion
    }
}
