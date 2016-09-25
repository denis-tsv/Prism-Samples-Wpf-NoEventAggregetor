using UIComposition.EmployeeModule.Models;

namespace UIComposition.EmployeeModule.ViewModels
{
    public interface IEmployeeInfoViewModel
    {
        Employee CurrentEmployee { get; set; }

        int Order { get; }

        string ViewName { get; }
    }
}
