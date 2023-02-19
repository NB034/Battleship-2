using Battleship_2.Models.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels
{
    internal class PlayerVsAiGameManagerViewModel : INotifyPropertyChanged
    {
        private PlayerVsAiGameManager gameManager;
        private IShipsGridViewModel aiShips;
        private IShipsGridViewModel playerShips;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CellViewModel> PlayerCells { get; private set; }
        public ObservableCollection<CellViewModel> AiCells { get; private set; }

        public IShipsGridViewModel AiShips
        {
            get => aiShips;
            private set => SetProperty(ref aiShips, value, nameof(AiShips));
        }
        public IShipsGridViewModel PlayerShips
        {
            get => playerShips;
            private set => SetProperty(ref playerShips, value, nameof(PlayerShips));
        }

        public PlayerVsAiGameManagerViewModel()
        {
            var autoPlacer = new ShipsAutoPlacer();
            var aiField = new AiFieldManager(autoPlacer.GenerateField());
            var playerField = new PlayerFieldManager(autoPlacer.GenerateField());
            gameManager = new PlayerVsAiGameManager(aiField, playerField);

            aiShips = new ShipsGridViewModel();
            playerShips = new ShipsGridViewModel();


        }

        private void SetProperty<T>(ref T oldValue, T newValue, string propertyName)
        {
            if (!oldValue?.Equals(newValue) ?? newValue != null)
            {
                oldValue = newValue;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
