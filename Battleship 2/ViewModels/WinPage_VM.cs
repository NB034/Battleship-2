using Battleship_2.Command;
using Battleship_2.Views;
using System.Windows;
using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    public class WinPage_VM
    {
        private readonly AutoEventCommandBase _toMainMenuCommand;
        private readonly AutoEventCommandBase _exitCommand;

        public WinPage_VM(string message, Brush messageBrush)
        {
            Message = message;
            MessageBrush = messageBrush;

            _toMainMenuCommand = new AutoEventCommandBase(o => ToMainMenu(o), _ => true);
            _exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        private void ToMainMenu(object o)
        {
            if (o is WinPage page)
            {
                var mainMenu = new MainPage();
                page.NavigationService.Navigate(mainMenu);
            }

        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public string Message { get; }
        public Brush MessageBrush { get; }

        public AutoEventCommandBase ToMainMenuCommand => _toMainMenuCommand;
        public AutoEventCommandBase ExitCommand => _exitCommand;
    }
}
