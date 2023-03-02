using Battleship_2.Command;
using Battleship_2.Views;
using System.Windows;

namespace Battleship_2.ViewModels
{
    public class MainMenuPageViewModel
    {
        private readonly AutoEventCommandBase _exitCommand;
        private readonly AutoEventCommandBase _startGameCommand;

        public MainMenuPageViewModel()
        {
            _startGameCommand = new AutoEventCommandBase(o => StartGame(o), _ => true);
            _exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        public AutoEventCommandBase ExitCommand => _exitCommand;
        public AutoEventCommandBase StartGameCommand => _startGameCommand;

        private void StartGame(object parameter)
        {
            if (parameter is MainMenuPage page)
            {
                var gamePage = new GamePage();
                var viewModel = new GamePlayerVsAiViewModel(gamePage);
                gamePage.DataContext = viewModel;
                page.NavigationService.Navigate(gamePage);
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
