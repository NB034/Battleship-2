using Battleship_2.Command;
using Battleship_2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Battleship_2.ViewModels
{
    public class WinPageViewModel
    {
        private readonly AutoEventCommandBase toMainMenuCommand;
        private readonly AutoEventCommandBase exitCommand;

        public WinPageViewModel(string message, Brush messageBrush)
        {
            Message = message;
            MessageBrush = messageBrush;

            toMainMenuCommand = new AutoEventCommandBase(o => ToMainMenu(o), _ => true);
            exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        private void ToMainMenu(object o)
        {
            if (o is WinPage page)
            {
                var mainMenu = new MainMenuPage();
                page.NavigationService.Navigate(mainMenu);
            }

        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public string Message { get; }
        public Brush MessageBrush { get; }

        public AutoEventCommandBase ToMainMenuCommand => toMainMenuCommand;
        public AutoEventCommandBase ExitCommand => exitCommand;
    }
}
