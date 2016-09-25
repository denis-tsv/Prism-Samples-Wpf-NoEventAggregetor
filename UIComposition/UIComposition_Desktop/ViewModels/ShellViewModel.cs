// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;

namespace UIComposition.Shell.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public ShellViewModel()
        {
            // Initialize this ViewModel's commands.
            this.ExitCommand = new DelegateCommand<object>(this.AppExit, this.CanAppExit);
        }

        #region ExitCommand

        public DelegateCommand<object> ExitCommand { get; private set; }

        private void AppExit(object commandArg)
        {
        }

        private bool CanAppExit(object commandArg)
        {
            return true;
        }

        #endregion
    }
}