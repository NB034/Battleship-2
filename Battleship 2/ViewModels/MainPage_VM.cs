using Battleship_2.Command;
using Battleship_2.Views;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace Battleship_2.ViewModels
{
    public class MainPage_VM
    {
        private readonly AutoEventCommandBase _exitCommand;
        private readonly AutoEventCommandBase _startGameCommand;

        public MainPage_VM()
        {
            _startGameCommand = new AutoEventCommandBase(o => StartGame(o), _ => true);
            _exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        public AutoEventCommandBase ExitCommand => _exitCommand;
        public AutoEventCommandBase StartGameCommand => _startGameCommand;

        private void StartGame(object parameter)
        {
            if (parameter is MainPage page)
            {
                while (page.NavigationService.CanGoBack)
                {
                    try
                    {
                        page.NavigationService.RemoveBackEntry();
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                var gamePage = new GamePage();
                var viewModel = new PVA_GamePage_VM(gamePage);
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
