using Battleship_2.Accessories;
using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using Battleship_2.ViewModels.Abstractions;
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
    internal class PlayerVsAiGameManagerViewModel : INotifyPropertyChanged, IGameManagerViewModel
    {
        private PlayerVsAiGameManager gameManager;

        private IShipsGridViewModel aiShips;
        private IShipsGridViewModel playerShips;

        private AutoEventCommandBase shootCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CellViewModel> PlayerField { get; private set; }
        public ObservableCollection<CellViewModel> AiField { get; private set; }

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
        public AutoEventCommandBase ShootCommand => shootCommand;

        public PlayerVsAiGameManagerViewModel()
        {
            var autoPlacer = new ShipsAutoPlacer();
            var aiField = new AiFieldManager(autoPlacer.GenerateField());
            var playerField = new PlayerFieldManager(autoPlacer.GenerateField());
            gameManager = new PlayerVsAiGameManager(aiField, playerField);

            ShipsImagesManager imagesManager = new ShipsImagesManager();
            playerShips = new ShipsGridViewModel(imagesManager.GetFirstShipsSet(), gameManager.PlayerFleet, OrientationsEnum.Left);
            aiShips = new ShipsGridViewModel(imagesManager.GetSecondShipsSet(), gameManager.AiFleet, OrientationsEnum.Right);

            var playerCellsList = new List<CellViewModel>(100);
            var aiCellsList = new List<CellViewModel>(100);

            for (int i = 0; i < LogicAccessories.NumberOfFieldRows; i++)
            {
                for (int j = 0; j < LogicAccessories.NumberOfFieldColumns; j++)
                {
                    playerCellsList.Add(new CellViewModel(gameManager.PlayerField[i, j].CellType == CellTypesEnum.ShipDeck));
                    aiCellsList.Add(new CellViewModel(gameManager.AiField[i, j].CellType == CellTypesEnum.ShipDeck));
                }
            }

            PlayerField = new ObservableCollection<CellViewModel>(playerCellsList);
            AiField = new ObservableCollection<CellViewModel>(aiCellsList);

            shootCommand = new AutoEventCommandBase(o => Shoot((CellViewModel)o), _ => IsShootAllowed);
            isShootAllowed = true;
        }

        private void Shoot(CellViewModel cell)
        {
            IsShootAllowed = false;

            int indexOfCell = AiField.IndexOf(cell);
            int rows = indexOfCell / LogicAccessories.NumberOfFieldRows;
            int columns = indexOfCell % LogicAccessories.NumberOfFieldColumns;
            gameManager.PlayerTurn(new BaseCell(rows, columns));

            aiShips.RefreshState(gameManager.AiFleet);
            playerShips.RefreshState(gameManager.PlayerFleet);

            BaseCell lastOpenedCell = gameManager.AiLastOpenedCell;
            indexOfCell = lastOpenedCell.Y * 10 + lastOpenedCell.X;
            AiField[indexOfCell].IsOpen = true;
            if (AiField[indexOfCell].IsShipDeck)
                AiField[indexOfCell].IsShipDestroyed = gameManager.AiFleet
                    .GetShip(gameManager.AiField[lastOpenedCell.X, lastOpenedCell.Y].ShipsGuids.First()).IsDestroyed;

            lastOpenedCell = gameManager.PlayerLastOpenedCell;
            indexOfCell = lastOpenedCell.Y * 10 + lastOpenedCell.X;
            PlayerField[indexOfCell].IsOpen = true;
            if (PlayerField[indexOfCell].IsShipDeck)
                PlayerField[indexOfCell].IsShipDestroyed = gameManager.PlayerFleet
                    .GetShip(gameManager.PlayerField[lastOpenedCell.X, lastOpenedCell.Y].ShipsGuids.First()).IsDestroyed;

            IsShootAllowed = true;
        }

        private void SetProperty<T>(ref T oldValue, T newValue, string propertyName)
        {
            if (!oldValue?.Equals(newValue) ?? newValue != null)
            {
                oldValue = newValue;

                NotifyPropertyChanged(propertyName);
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
