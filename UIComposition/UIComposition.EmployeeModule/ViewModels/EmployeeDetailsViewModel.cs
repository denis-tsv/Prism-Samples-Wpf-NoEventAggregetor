// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Prism.Mvvm;
using UIComposition.EmployeeModule.Models;

namespace UIComposition.EmployeeModule.ViewModels
{
    /// <summary>
    /// View model to support the Employee Details view.
    /// </summary>
    public class EmployeeDetailsViewModel : BindableBase, IEmployeeInfoViewModel
    {
        public EmployeeDetailsViewModel()
        {
        }

        public string ViewName => "Employee Details";

        private Employee currentEmployee;

        public Employee CurrentEmployee
        {
            get { return this.currentEmployee; }
            set { SetProperty(ref currentEmployee, value); }
        }

        public int Order => 1;
    }
}