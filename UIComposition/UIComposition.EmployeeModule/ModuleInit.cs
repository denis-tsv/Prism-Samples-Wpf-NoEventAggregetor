// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using UIComposition.EmployeeModule.Services;
using UIComposition.EmployeeModule.ViewModels;
using UIComposition.EmployeeModule.Views;
using UIComposition.Infrastructure;

namespace UIComposition.EmployeeModule
{
    public class ModuleInit : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager  regionManager;

        public ModuleInit(IUnityContainer container, IRegionManager regionManager)
        {
            this.container     = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            // Register the EmployeeDataService concrete type with the container.
            // Change this to swap in another data service implementation.
            this.container.RegisterType<IEmployeeDataService, EmployeeDataService>();

            container.RegisterType<IEmployeeInfoViewModel, EmployeeProjectsViewModel>("Projects");
            container.RegisterType<IEmployeeInfoViewModel, EmployeeDetailsViewModel>("Details");

            var viewModelMapper = new ViewModelMapper();
            container.RegisterInstance<IViewModelMapper>(viewModelMapper);

            viewModelMapper.Register<EmployeeDetailsView, EmployeeDetailsViewModel>();
            viewModelMapper.Register<EmployeeProjectsView, EmployeeProjectsViewModel>();

            viewModelMapper.Register<EmployeeListView, EmployeeListViewModel>();
            viewModelMapper.Register<EmployeeSummaryView, EmployeeSummaryViewModel>();

            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                       () => this.container.Resolve<EmployeeMasterDetailView>());
            
        }
    }
}