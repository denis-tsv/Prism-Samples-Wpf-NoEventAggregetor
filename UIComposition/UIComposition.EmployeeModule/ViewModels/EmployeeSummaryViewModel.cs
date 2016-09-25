// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using UIComposition.EmployeeModule.Services;

namespace UIComposition.EmployeeModule.ViewModels
{
    /// <summary>
    /// View model to support the Employee Summary view.
    /// </summary>
    public class EmployeeSummaryViewModel : BindableBase
    {
        private readonly IEmployeeDataService dataService;

        public EmployeeSummaryViewModel(IEmployeeDataService dataService, IUnityContainer container)
        {
            this.dataService = dataService;
            this.EmployeeInfoViewModels = container.ResolveAll<IEmployeeInfoViewModel>().OrderBy(vm => vm.Order).ToList();
        }

        public IEnumerable<IEmployeeInfoViewModel> EmployeeInfoViewModels { get; }

        public void ShowEmployee(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            var currentEmployee = this.dataService.GetEmployees().FirstOrDefault(item => item.Id == id);

            foreach (var employeeInfoViewModel in EmployeeInfoViewModels)
            {
                employeeInfoViewModel.CurrentEmployee = currentEmployee;
            }
        }
        
    }
}