using Battleship_2.Accessories;
using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using Battleship_2.ViewModels.Abstractions;
using Battleship_2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Battleship_2.ViewModels
{
    internal class PlayerVsAiGameManagerViewModel : INotifyPropertyChanged, IGameManagerViewModel
    {
        private PlayerVsAiGameManager gameManager;

        private IShipsGridViewModel aiShips;
        private IShipsGridViewModel playerShips;
        private List<CellViewModel> leftField;
        private List<CellViewModel> rightField;
        private AutoEventCommandBase shootCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<CellViewModel> LeftField
        {
            get => leftField;
            private set => SetProperty(ref leftField, value, nameof(leftField));
        }
        public List<CellViewModel> RightField
        {
            get => rightField;
            private set => SetProperty(ref rightField, value, nameof(rightField));
        }

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
        public Page PageForNavigationService { get; set; }

        public PlayerVsAiGameManagerViewModel(Page pageForNavigationService)
        {
            var autoPlacer = new ShipsAutoPlacer();
            var aiFieldManager = new AiFieldManager(autoPlacer.GenerateField());
            var playerFieldManager = new PlayerFieldManager(autoPlacer.GenerateField());
            gameManager = new PlayerVsAiGameManager(aiFieldManager, playerFieldManager);

            ShipsImagesManager imagesManager = new ShipsImagesManager();
            playerShips = new ShipsGridViewModel(imagesManager.GetFirstShipsSet(), gameManager.PlayerFleet, OrientationsEnum.Left);
            aiShips = new ShipsGridViewModel(imagesManager.GetSecondShipsSet(), gameManager.AiFleet, OrientationsEnum.Right);

            leftField = new List<CellViewModel>();
            rightField = new List<CellViewModel>();
            RefreshFields();

            gameManager.FieldsChanged += RefreshView;
            gameManager.PlayerWin += NavigateToPlayerWinPage;
            gameManager.AiWin += NavigateToAiWinPage;
            gameManager.IsTurnOverChanged += RefreshTurnState;

            shootCommand = new AutoEventCommandBase(o => Shoot(o), o => CanShoot(o));
            PageForNavigationService = pageForNavigationService;
        }

        private void Shoot(object parameter)
        {
            var cell = (CellViewModel)parameter;
            int indexOfCell = RightField.IndexOf(cell);
            int rows = indexOfCell / LogicAccessories.NumberOfFieldRows;
            int columns = indexOfCell % LogicAccessories.NumberOfFieldColumns;
            Task.Run(() => gameManager.PlayerTurn(new BaseCell(columns, rows)));
        }

        private bool CanShoot(object parameter)
        {
            var cell = (CellViewModel)parameter;
            return gameManager.IsTurnOver && !cell.IsOpen;
        }

        private void NavigateToAiWinPage()
        {
            var viewModel = new WinPageViewModel(
                Application.Current.Resources["PlayerLoseMessage"] as string ?? "Error",
                Application.Current.Resources["TitleLinearGradient"] as Brush ?? Brushes.Gray);
            var winPage = new WinPage(viewModel);
            PageForNavigationService.NavigationService.Navigate(winPage);
        }

        private void NavigateToPlayerWinPage()
        {
            var viewModel = new WinPageViewModel(
                Application.Current.Resources["PlayerWinMessage"] as string ?? "Error",
                Application.Current.Resources["TitleLinearGradient"] as Brush ?? Brushes.Gray);
            var winPage = new WinPage(viewModel);
            PageForNavigationService.NavigationService.Navigate(winPage);

        }

        private void RefreshView()
        {
            aiShips.RefreshState(gameManager.AiFleet);
            RefreshFields();
        }

        private void RefreshTurnState()
        {
            CellViewModel.IsTurnOver = gameManager.IsTurnOver;
        }

        private void RefreshFields()
        {
            int totalNumberOfCells = LogicAccessories.NumberOfFieldRows * LogicAccessories.NumberOfFieldColumns;
            var playerCellsList = new List<CellViewModel>(totalNumberOfCells);
            var aiCellsList = new List<CellViewModel>(totalNumberOfCells);

            for (int i = 0; i < LogicAccessories.NumberOfFieldRows; i++)
            {
                for (int j = 0; j < LogicAccessories.NumberOfFieldColumns; j++)
                {
                    var playerCell = new CellViewModel(gameManager.PlayerField[i, j].CellType == CellTypesEnum.ShipDeck)
                    {
                        IsOpen = gameManager.PlayerField[i, j].IsOpen
                    };
                    if (playerCell.IsShipDeck)
                    {
                        playerCell.IsShipDestroyed = gameManager.PlayerFleet.GetShip(gameManager.PlayerField[i, j].ShipsGuids.First()).IsDestroyed;
                    }
                    playerCellsList.Add(playerCell);

                    var aiCell = new CellViewModel(gameManager.AiField[i, j].CellType == CellTypesEnum.ShipDeck)
                    {
                        IsOpen = gameManager.AiField[i, j].IsOpen
                    };
                    if (aiCell.IsShipDeck)
                    {
                        aiCell.IsShipDestroyed = gameManager.AiFleet.GetShip(gameManager.AiField[i, j].ShipsGuids.First()).IsDestroyed;
                    }
                    aiCellsList.Add(aiCell);
                }
            }
            LeftField = playerCellsList;
            RightField = aiCellsList;
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
