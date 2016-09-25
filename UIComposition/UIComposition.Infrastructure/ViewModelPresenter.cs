using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;

namespace UIComposition.Infrastructure
{
    /// <summary>
    /// Presents a View corresponding to the View Model
    /// </summary>
    public class ViewModelPresenter : ContentControl
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public ViewModelPresenter()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        // Dependency properties
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ViewModelPresenter), new PropertyMetadata(null, OnViewModelPropertyChanged));


        /// <summary>
        /// View Model to be presented
        /// </summary>
        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void OnViewModelPropertyChanged(DependencyObject changedObject, DependencyPropertyChangedEventArgs args)
        {
            var contentControl = (ViewModelPresenter)changedObject;
            contentControl.RefreshContentPresenter();
        }

        private void RefreshContentPresenter()
        {
            if (ViewModel == null)
            {
                Content = null;

                return;
            }

            var viewModelMapper = ServiceLocator.Current.GetInstance<IViewModelMapper>();
            var view = viewModelMapper.ResolveView(ViewModel);

            if (view != null)
            {
                view.DataContext = ViewModel;
                Content = view;
            }
            else
            {
                Content = "View hasn't been found";
            }
        }
    }

}
