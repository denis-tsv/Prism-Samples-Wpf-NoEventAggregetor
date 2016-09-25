using System;
using System.Collections.Generic;
using System.Windows;

namespace UIComposition.Infrastructure
{
    public class ViewModelMapper : IViewModelMapper
    {
        private readonly Dictionary<Type, Type> _mapping = new Dictionary<Type, Type>();

        public void Register<TView, TViewModel>() where TView : FrameworkElement
        {
            _mapping[typeof(TViewModel)] = typeof(TView);
        }

        public FrameworkElement ResolveView(object viewModel)
        {
            if (!_mapping.ContainsKey(viewModel.GetType()))
            {
                return null;
            }

            var viewType = _mapping[viewModel.GetType()];
            return (FrameworkElement) Activator.CreateInstance(viewType);
        }
    }
}
