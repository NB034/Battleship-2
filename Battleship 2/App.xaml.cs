using Battleship_2.ViewModels;
using Battleship_2.Views;
using System.Windows;
using System.Windows.Navigation;

namespace Battleship_2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = new MainMenuViewModel();
            var mainMenuPage = new MainMenuPage(viewModel);

            

            //var viewModel = new MainMenuViewModel();
            //var view = new MainMenuView(viewModel);

            //view.Show();
        }
    }
}
