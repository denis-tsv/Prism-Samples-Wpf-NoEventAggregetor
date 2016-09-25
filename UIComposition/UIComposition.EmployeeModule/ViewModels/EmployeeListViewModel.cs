// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Windows.Data;
using Prism.Mvvm;
using UIComposition.EmployeeModule.Services;

namespace UIComposition.EmployeeModule.ViewModels
{
    /// <summary>
    /// View model to support the Employee List view.
    /// </summary>
    public class EmployeeListViewModel : BindableBase
    {
        public EmployeeListViewModel(IEmployeeDataService dataService)
        {
            if (dataService == null) throw new ArgumentNullException("dataService");
        
            // Initialize the CollectionView for the underlying model
            // and track the current selection.
            this.Employees = new ListCollectionView(dataService.GetEmployees());
            
        }

        public ICollectionView Employees { get;  }
    }
}