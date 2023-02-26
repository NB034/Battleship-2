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

        public ObservableCollection<CellViewModel> LeftField { get; private set; }
        public ObservableCollection<CellViewModel> RightField { get; private set; }

        public IShipsGridViewModel RightFieldShips
        {
            get => aiShips;
            private set => SetProperty(ref aiShips, value, nameof(RightFieldShips));
        }
        public IShipsGridViewModel LeftFieldShips
        {
            get => playerShips;
            private set => SetProperty(ref playerShips, value, nameof(LeftFieldShips));
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

            LeftField = new ObservableCollection<CellViewModel>(playerCellsList);
            RightField = new ObservableCollection<CellViewModel>(aiCellsList);

            shootCommand = new AutoEventCommandBase(o => Shoot((CellViewModel)o), _ => true);
        }

        private void Shoot(CellViewModel cell)
        {
            int indexOfCell = RightField.IndexOf(cell);
            int rows = indexOfCell / LogicAccessories.NumberOfFieldRows;
            int columns = indexOfCell % LogicAccessories.NumberOfFieldColumns;
            gameManager.PlayerTurn(new BaseCell(rows, columns));

            aiShips.RefreshState(gameManager.AiFleet);
            playerShips.RefreshState(gameManager.PlayerFleet);

            BaseCell lastOpenedCell = gameManager.AiLastOpenedCell;
            indexOfCell = lastOpenedCell.Y * 10 + lastOpenedCell.X;
            RightField[indexOfCell].IsOpen = true;
            if (RightField[indexOfCell].IsShipDeck)
                RightField[indexOfCell].IsShipDestroyed = gameManager.AiFleet
                    .GetShip(gameManager.AiField[lastOpenedCell.X, lastOpenedCell.Y].ShipsGuids.First()).IsDestroyed;

            lastOpenedCell = gameManager.PlayerLastOpenedCell;
            indexOfCell = lastOpenedCell.Y * 10 + lastOpenedCell.X;
            LeftField[indexOfCell].IsOpen = true;
            if (LeftField[indexOfCell].IsShipDeck)
                LeftField[indexOfCell].IsShipDestroyed = gameManager.PlayerFleet
                    .GetShip(gameManager.PlayerField[lastOpenedCell.X, lastOpenedCell.Y].ShipsGuids.First()).IsDestroyed;
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
