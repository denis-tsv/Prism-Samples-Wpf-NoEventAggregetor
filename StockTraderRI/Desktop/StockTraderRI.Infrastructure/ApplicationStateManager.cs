using System.ComponentModel;
using System.ComponentModel.Composition;
using Prism.Mvvm;

namespace StockTraderRI.Infrastructure
{
    public interface IApplicationStateManager : INotifyPropertyChanged
    {
        string CurrentTickerSymbol { get; set; }
    }

    [Export(typeof(IApplicationStateManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ApplicationStateManager : BindableBase, IApplicationStateManager
    {
        private string _currentTickerSymbol;

        public string CurrentTickerSymbol
        {
            get { return _currentTickerSymbol; }
            set { SetProperty(ref _currentTickerSymbol, value); }
        }
    }
}
