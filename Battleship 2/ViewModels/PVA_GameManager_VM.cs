using Battleship_2.Accessories;
using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using Battleship_2.Models.Logic.Ai;
using Battleship_2.ViewModels.Abstractions;
using Battleship_2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Battleship_2.ViewModels
{
    internal class PVA_GameManager_VM : INotifyPropertyChanged, IGameManager_VM
    {
        private PVA_GameManager gameManager;
        private List<Cell_VM> leftField;
        private List<Cell_VM> rightField;
        private readonly AutoEventCommandBase shootCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Cell_VM> LeftField
        {
            get => leftField;
            private set => SetProperty(ref leftField, value, nameof(LeftField));
        }
        public List<Cell_VM> RightField
        {
            get => rightField;
            private set => SetProperty(ref rightField, value, nameof(RightField));
        }

        public IShipsGrid_VM RightFieldShips { get; }
        public IShipsGrid_VM LeftFieldShips { get; }

        public AutoEventCommandBase ShootCommand => shootCommand;
        public Page PageForNavigationService { get; set; }

        public PVA_GameManager_VM(Page pageForNavigationService)
        {
            var autoPlacer = new ShipsAutoPlacer();
            var aiFieldManager = new Ai_FieldManager(autoPlacer.GenerateField());
            var playerFieldManager = new PlayerFieldManager(autoPlacer.GenerateField());
            gameManager = new PVA_GameManager(aiFieldManager, playerFieldManager);

            ShipsImagesManager imagesManager = new ShipsImagesManager();
            LeftFieldShips = new ShipsGrid_VM(imagesManager.GetFirstShipsSet(), gameManager.PlayerFleet, OrientationsEnum.Left);
            RightFieldShips = new ShipsGrid_VM(imagesManager.GetSecondShipsSet(), gameManager.AiFleet, OrientationsEnum.Right);

            leftField = new List<Cell_VM>();
            rightField = new List<Cell_VM>();
            RefreshLeftField();
            RefreshRightField();

            gameManager.PlayerWin += NavigateToPlayerWinPage;
            gameManager.AiWin += NavigateToAiWinPage;
            gameManager.PlayerFieldChanged += RefreshLeftField;
            gameManager.AiFieldChanged += RefreshRightField;

            shootCommand = new AutoEventCommandBase(o => Shoot(o), o => CanShoot(o));
            PageForNavigationService = pageForNavigationService;
        }

        private void Shoot(object parameter)
        {
            var cell = (Cell_VM)parameter;
            int indexOfCell = RightField.IndexOf(cell);
            int rows = indexOfCell / LogicAccessories.NumberOfFieldRows;
            int columns = indexOfCell % LogicAccessories.NumberOfFieldColumns;
            Task.Run(() => gameManager.PlayerShoot(new BaseCell(columns, rows)));
        }

        private bool CanShoot(object parameter)
        {
            var cell = (Cell_VM)parameter;
            return !cell.IsOpen && cell.IsCellActive;
        }

        private void NavigateToAiWinPage()
        {
            PageForNavigationService.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    var viewModel = new WinPage_VM(
                Application.Current.Resources["PlayerLoseMessage"] as string ?? "Error",
                Application.Current.Resources["TitleLinearGradient"] as Brush ?? Brushes.Gray);
                    var winPage = new WinPage(viewModel);
                    PageForNavigationService.NavigationService.Navigate(winPage);
                });
        }

        private void NavigateToPlayerWinPage()
        {
            PageForNavigationService.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    var viewModel = new WinPage_VM(
                    Application.Current.Resources["PlayerWinMessage"] as string ?? "Error",
                    Application.Current.Resources["TitleLinearGradient"] as Brush ?? Brushes.Gray);
                    var winPage = new WinPage(viewModel);
                    PageForNavigationService.NavigationService.Navigate(winPage);
                });
        }

        private void RefreshLeftField()
        {
            int totalNumberOfCells = LogicAccessories.NumberOfFieldRows * LogicAccessories.NumberOfFieldColumns;
            var playerCellsList = new List<Cell_VM>(totalNumberOfCells);

            for (int i = 0; i < LogicAccessories.NumberOfFieldRows; i++)
            {
                for (int j = 0; j < LogicAccessories.NumberOfFieldColumns; j++)
                {
                    var playerCell = new Cell_VM(gameManager.PlayerField[i, j].CellType == CellTypesEnum.ShipDeck)
                    {
                        IsOpen = gameManager.PlayerField[i, j].IsOpen
                    };
                    if (playerCell.IsShipDeck)
                    {
                        playerCell.IsShipDestroyed = gameManager.PlayerFleet.GetShip(gameManager.PlayerField[i, j].ShipsGuids.First()).IsDestroyed;
                    }
                    playerCellsList.Add(playerCell);
                }
            }
            LeftField = playerCellsList;
        }

        private void RefreshRightField()
        {
            int totalNumberOfCells = LogicAccessories.NumberOfFieldRows * LogicAccessories.NumberOfFieldColumns;
            var aiCellsList = new List<Cell_VM>(totalNumberOfCells);

            for (int i = 0; i < LogicAccessories.NumberOfFieldRows; i++)
            {
                for (int j = 0; j < LogicAccessories.NumberOfFieldColumns; j++)
                {

                    var aiCell = new Cell_VM(gameManager.AiField[i, j].CellType == CellTypesEnum.ShipDeck)
                    {
                        IsOpen = gameManager.AiField[i, j].IsOpen,
                        IsCellActive = gameManager.IsPlayerTurn
                    };
                    if (aiCell.IsShipDeck)
                    {
                        aiCell.IsShipDestroyed = gameManager.AiFleet.GetShip(gameManager.AiField[i, j].ShipsGuids.First()).IsDestroyed;
                    }
                    aiCellsList.Add(aiCell);
                }
            }
            RightField = aiCellsList;

            RightFieldShips.RefreshState(gameManager.AiFleet);
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
