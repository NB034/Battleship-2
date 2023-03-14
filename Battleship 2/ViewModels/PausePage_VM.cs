using Battleship_2.Command;
using Battleship_2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Battleship_2.ViewModels
{
    internal class PausePage_VM
    {
        private readonly AutoEventCommandBase _resumeCommand;
        private readonly AutoEventCommandBase _toMainMenuCommand;
        private readonly AutoEventCommandBase _exitCommand;

        public PausePage_VM()
        {
            _resumeCommand = new AutoEventCommandBase(o => Resume(o), _ => true);
            _toMainMenuCommand = new AutoEventCommandBase(o => ToMainMenu(o), _ => true);
            _exitCommand = new AutoEventCommandBase(o => Application.Current.Shutdown(), _ => true);
        }

        public AutoEventCommandBase ResumeCommand => _resumeCommand;
        public AutoEventCommandBase ToMainMenuCommand => _toMainMenuCommand;
        public AutoEventCommandBase ExitCommand => _exitCommand;

        private void Resume(object o)
        {
            var page = (Page)o;
            page.NavigationService.GoBack();
        }

        private void ToMainMenu(object o)
        {
            var page = (Page)o;
            page.NavigationService.Navigate(new MainPage());
        }
    }
}
