using System;
using Prism.Mvvm;
using UIComposition.EmployeeModule.Models;

namespace UIComposition.EmployeeModule.ViewModels
{
    public class EmployeeMasterDetailViewModel : BindableBase
    {
        public EmployeeMasterDetailViewModel(EmployeeListViewModel employeeListViewModel,
            EmployeeSummaryViewModel employeeSummaryViewModel)
        {
            EmployeeListViewModel = employeeListViewModel;
            EmployeeSummaryViewModel = employeeSummaryViewModel;

            EmployeeListViewModel.Employees.CurrentChanged += EmployeesOnCurrentChanged;

        }

        private void EmployeesOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            var employee = EmployeeListViewModel.Employees.CurrentItem as Employee;
            if (employee != null)
            {
                EmployeeSummaryViewModel.ShowEmployee(employee.Id);
            }
        }

        public EmployeeListViewModel EmployeeListViewModel { get; }

        public EmployeeSummaryViewModel EmployeeSummaryViewModel { get; }
    }
}
