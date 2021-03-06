

using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Prism.Mvvm;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Controllers;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    [Export(typeof(IPositionSummaryViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionSummaryViewModel : BindableBase, IPositionSummaryViewModel
    {
        private readonly IApplicationStateManager _applicationStateManager;
        private PositionSummaryItem currentPositionSummaryItem;

        public IObservablePosition Position { get; private set; }

        [ImportingConstructor]
        public PositionSummaryViewModel(IOrdersController ordersController, IApplicationStateManager applicationStateManager, IObservablePosition observablePosition)
        {
            if (ordersController == null)
            {
                throw new ArgumentNullException("ordersController");
            }

            _applicationStateManager = applicationStateManager;

            this.Position = observablePosition;

            BuyCommand = ordersController.BuyCommand;
            SellCommand = ordersController.SellCommand;

            this.CurrentPositionSummaryItem = new PositionSummaryItem("FAKEINDEX", 0, 0, 0);
        }

        public ICommand BuyCommand { get; private set; }

        public ICommand SellCommand { get; private set; }

        public string HeaderInfo
        {
            get { return "POSITION"; }
        }

        public PositionSummaryItem CurrentPositionSummaryItem
        {
            get { return currentPositionSummaryItem; }
            set
            {
                if (SetProperty(ref currentPositionSummaryItem, value))
                {
                    if (currentPositionSummaryItem != null)
                    {
                        _applicationStateManager.CurrentTickerSymbol = CurrentPositionSummaryItem.TickerSymbol;
                    }
                }
            }
        }
    }
}
