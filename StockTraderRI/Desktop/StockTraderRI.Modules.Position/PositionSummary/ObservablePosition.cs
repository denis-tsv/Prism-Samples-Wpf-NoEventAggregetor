

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Events;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using System.ComponentModel.Composition;
using System;
using System.Collections.Specialized;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    [Export(typeof(IObservablePosition))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ObservablePosition : IObservablePosition
    {
        private IAccountPositionService accountPositionService;
        private IMarketFeedService marketFeedService;

        public ObservableCollection<PositionSummaryItem> Items { get; private set; }

        [ImportingConstructor]
        public ObservablePosition(IAccountPositionService accountPositionService, IMarketFeedService marketFeedService)
        {
            this.Items = new ObservableCollection<PositionSummaryItem>();

            this.accountPositionService = accountPositionService;
            this.marketFeedService = marketFeedService;

            marketFeedService.Prices.CollectionChanged += PricesUpdated;

            PopulateItems();

            this.accountPositionService.Updated += PositionSummaryItems_Updated;
        }

        private void PricesUpdated(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    foreach (var newPrice in e.NewItems.Cast<KeyValuePair<string, decimal>>())
                    {
                        var position = Items.FirstOrDefault(p => p.TickerSymbol == newPrice.Key);

                        if (position != null)
                        {
                            position.CurrentPrice = newPrice.Value;
                        }
                    }
                    break;
                //TODO implement support of e.Remove and e.Reset when NotifyCollectionChangedAction.Remove will be supported by ObservableDictionary
            }
        }
        
        private void PositionSummaryItems_Updated(object sender, AccountPositionModelEventArgs e)
        {
            if (e.AcctPosition != null)
            {
                PositionSummaryItem positionSummaryItem = this.Items.First(p => p.TickerSymbol == e.AcctPosition.TickerSymbol);

                if (positionSummaryItem != null)
                {
                    positionSummaryItem.Shares = e.AcctPosition.Shares;
                    positionSummaryItem.CostBasis = e.AcctPosition.CostBasis;
                }
            }
        }

        private void PopulateItems()
        {
            PositionSummaryItem positionSummaryItem;
            foreach (AccountPosition accountPosition in this.accountPositionService.GetAccountPositions())
            {
                positionSummaryItem = new PositionSummaryItem(accountPosition.TickerSymbol, accountPosition.CostBasis, accountPosition.Shares, this.marketFeedService.GetPrice(accountPosition.TickerSymbol));
                this.Items.Add(positionSummaryItem);
            }
        }
    }
}
