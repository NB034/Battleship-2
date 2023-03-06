using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Battleship_2.ViewModels
{
    internal class GamePlayerVsAiViewModel : IGamePageViewModel
    {
        private readonly IGameManagerViewModel gameManager;
        private readonly AutoEventCommandBase openMenuCommand;

        public GamePlayerVsAiViewModel(Page pageForNavigationService)
        {
            gameManager = new PlayerVsAiGameManagerViewModel(pageForNavigationService);
            openMenuCommand = new AutoEventCommandBase(o => Array.Reverse(new[] { 1 }), _ => true);
        }

        public IGameManagerViewModel GameManager => gameManager;
        public AutoEventCommandBase OpenMenuCommand => openMenuCommand;
    }
}
