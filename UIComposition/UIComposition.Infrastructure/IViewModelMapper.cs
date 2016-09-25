using System.Windows;

namespace UIComposition.Infrastructure
{
    public interface IViewModelMapper
    {
        void Register<TView, TViewModel>() where TView : FrameworkElement;
        FrameworkElement ResolveView(object viewModel);
    }
}
