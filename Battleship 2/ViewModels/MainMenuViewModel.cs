using Battleship_2.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels
{
    public class MainMenuViewModel
    {
        private readonly AutoEventCommandBase _exitCommand;
        private readonly AutoEventCommandBase _startGameCommand;

        public MainMenuViewModel()
        {
            _exitCommand = new AutoEventCommandBase(_ => ExitButtonAction?.Invoke(), _ => true);
            _startGameCommand = new AutoEventCommandBase(_ => StartGame(), _ => true);
        }

        public AutoEventCommandBase ExitCommand => _exitCommand;
        public AutoEventCommandBase StartGameCommand => _startGameCommand;
        public Action? ExitButtonAction { get; set; } = null;

        private void StartGame()
        {

        }
    }
}
