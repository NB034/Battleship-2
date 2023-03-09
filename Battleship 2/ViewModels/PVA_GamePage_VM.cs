using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Battleship_2.ViewModels
{
    internal class PVA_GamePage_VM : IGamePage_VM
    {
        private readonly IGameManager_VM gameManager;
        private readonly AutoEventCommandBase openMenuCommand;

        public PVA_GamePage_VM(Page pageForNavigationService)
        {
            gameManager = new PVA_GameManager_VM(pageForNavigationService);
            openMenuCommand = new AutoEventCommandBase(o => Array.Reverse(new[] { 1 }), _ => true);
        }

        public IGameManager_VM GameManager => gameManager;
        public AutoEventCommandBase OpenMenuCommand => openMenuCommand;
    }
}
