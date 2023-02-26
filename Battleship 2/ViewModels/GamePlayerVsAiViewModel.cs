using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Battleship_2.ViewModels
{
    internal class GamePlayerVsAiViewModel : IGamePageViewModel, INotifyPropertyChanged
    {
        private readonly IGameManagerViewModel gameManager;
        private readonly AutoEventCommandBase openMenuCommand;
        private readonly AutoEventCommandBase shootCommand;
        private bool isShootAllowed;

        public GamePlayerVsAiViewModel()
        {
            gameManager = new PlayerVsAiGameManagerViewModel();
            openMenuCommand = new AutoEventCommandBase(o => o.ToString(), _ => true);
            shootCommand = new AutoEventCommandBase(o => Shoot(o), _ => IsShootAllowed);
            isShootAllowed = true;
        }

        private void Shoot(object o)
        {
            IsShootAllowed = false;
            gameManager.ShootCommand.Execute(o);
            IsShootAllowed= true;
        }

        public bool IsShootAllowed
        {
            get => isShootAllowed;
            set
            {
                isShootAllowed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsShootAllowed)));
            }
        }

        public IGameManagerViewModel GameManager => gameManager;
        public AutoEventCommandBase OpenMenuCommand => openMenuCommand;
        public AutoEventCommandBase ShootCommand => shootCommand;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
