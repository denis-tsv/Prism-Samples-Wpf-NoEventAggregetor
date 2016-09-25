using System.Windows.Controls;
using UIComposition.EmployeeModule.ViewModels;

namespace UIComposition.EmployeeModule.Views
{
    /// <summary>
    /// Interaction logic for EmployeeMasterDetailView.xaml
    /// </summary>
    public partial class EmployeeMasterDetailView : UserControl
    {
        public EmployeeMasterDetailView(EmployeeMasterDetailViewModel employeeMasterDetailViewModel)
        {
            InitializeComponent();

            DataContext = employeeMasterDetailViewModel;
        }
    }
}
