

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Prism.Mvvm;
using Prism.Events;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Market.TrendLine
{
    [Export(typeof(TrendLineViewModel))]
    public class TrendLineViewModel : BindableBase
    {
        private readonly IMarketHistoryService marketHistoryService;
        private readonly IApplicationStateManager _applicationStateManager;

        private string tickerSymbol;

        private MarketHistoryCollection historyCollection;

        [ImportingConstructor]
        public TrendLineViewModel(IMarketHistoryService marketHistoryService, IApplicationStateManager applicationStateManager)
        {
            this.marketHistoryService = marketHistoryService;

            _applicationStateManager = applicationStateManager;
            _applicationStateManager.PropertyChanged += (s, e) => TickerSymbolChanged();
        }

        public void TickerSymbolChanged()
        {
            MarketHistoryCollection newHistoryCollection = this.marketHistoryService.GetPriceHistory(_applicationStateManager.CurrentTickerSymbol);

            this.TickerSymbol = _applicationStateManager.CurrentTickerSymbol;
            this.HistoryCollection = newHistoryCollection;
        }

        public string TickerSymbol
        {
            get
            {
                return this.tickerSymbol;
            }
            set
            {
                SetProperty(ref this.tickerSymbol, value);
            }
        }

        public MarketHistoryCollection HistoryCollection
        {
            get
            {
                return historyCollection;
            }
            private set
            {
                SetProperty(ref this.historyCollection, value);
            }
        }
    }
}
