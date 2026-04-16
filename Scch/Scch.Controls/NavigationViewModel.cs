using System.Collections.ObjectModel;
using EplusE.Inspire.Mvvm.ViewModel;

namespace EplusE.Inspire.Controls
{
    public abstract class NavigationViewModel : NavigationViewModelBase
    {
        protected NavigationViewModel()
        {
            NavigationViewModels = new ObservableCollection<ViewModelBase>
                                       {
                CommonCommands.CreateFirstCommand(FirstCommand),
                CommonCommands.CreatePreviousCommand(PreviousCommand),
                this, // The navigation view model itself lies between the previous and next CommandViewModel
                CommonCommands.CreateNextCommand(NextCommand),
                CommonCommands.CreateLastCommand(LastCommand),
                CommonCommands.CreateDeleteCommand(DeleteCommand),
                CommonCommands.CreateAddCommand(AddCommand)
            };
        }

    }
}
