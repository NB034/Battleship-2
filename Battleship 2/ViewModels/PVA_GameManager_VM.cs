﻿using Battleship_2.Accessories;
using Battleship_2.Command;
using Battleship_2.Models.Ai;
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
        private PVA_GameManager _gameManager;
        private List<Cell_VM> _leftField;
        private List<Cell_VM> _rightField;
        private readonly AutoEventCommandBase _shootCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Cell_VM> LeftField
        {
            get => _leftField;
            private set => SetProperty(ref _leftField, value, nameof(LeftField));
        }
        public List<Cell_VM> RightField
        {
            get => _rightField;
            private set => SetProperty(ref _rightField, value, nameof(RightField));
        }

        public IShipsGrid_VM RightFieldShips { get; }
        public IShipsGrid_VM LeftFieldShips { get; }

        public AutoEventCommandBase ShootCommand => _shootCommand;
        public Page PageForNavigationService { get; set; }

        public PVA_GameManager_VM(Page pageForNavigationService)
        {
            var autoPlacer = new ShipsAutoPlacer();
            var aiFieldManager = new SimpleAiFieldManager(autoPlacer.GenerateField());
            var playerFieldManager = new PlayerFieldManager(autoPlacer.GenerateField());
            _gameManager = new PVA_GameManager(aiFieldManager, playerFieldManager);

            ShipsImagesManager imagesManager = new ShipsImagesManager();
            LeftFieldShips = new ShipsGrid_VM(imagesManager.GetFirstShipsSet(), _gameManager.PlayerFleet, DirectionsEnum.Left);
            RightFieldShips = new ShipsGrid_VM(imagesManager.GetSecondShipsSet(), _gameManager.AiFleet, DirectionsEnum.Right);

            _leftField = new List<Cell_VM>();
            _rightField = new List<Cell_VM>();
            RefreshLeftField();
            RefreshRightField();

            _gameManager.PlayerWin += NavigateToPlayerWinPage;
            _gameManager.AiWin += NavigateToAiWinPage;
            _gameManager.PlayerFieldChanged += RefreshLeftField;
            _gameManager.AiFieldChanged += RefreshRightField;

            _shootCommand = new AutoEventCommandBase(o => Shoot(o), o => CanShoot(o));
            PageForNavigationService = pageForNavigationService;
        }

        private async void Shoot(object parameter)
        {
            var cell = (Cell_VM)parameter;
            int indexOfCell = RightField.IndexOf(cell);
            int rows = indexOfCell / LogicAccessories.FieldRows;
            int columns = indexOfCell % LogicAccessories.FieldColumns;
            await Task.Run(() => _gameManager.PlayerShoot(new BaseCell(rows, columns)));
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
                    Application.Current.Resources["LoseTextBrush"] as Brush ?? Brushes.Gray);
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
                    Application.Current.Resources["WinTextBrush"] as Brush ?? Brushes.Gray);
                    var winPage = new WinPage(viewModel);
                    PageForNavigationService.NavigationService.Navigate(winPage);
                });
        }

        private void RefreshLeftField()
        {
            int totalNumberOfCells = LogicAccessories.FieldRows * LogicAccessories.FieldColumns;
            var playerCellsList = new List<Cell_VM>(totalNumberOfCells);

            for (int i = 0; i < LogicAccessories.FieldRows; i++)
            {
                for (int j = 0; j < LogicAccessories.FieldColumns; j++)
                {
                    var playerCell = new Cell_VM(_gameManager.PlayerField[i, j].CellType == CellTypesEnum.ShipDeck)
                    {
                        IsOpen = _gameManager.PlayerField[i, j].IsOpen
                    };
                    if (playerCell.IsShipDeck)
                    {
                        playerCell.IsShipDestroyed = _gameManager.PlayerFleet.GetShip(_gameManager.PlayerField[i, j].ShipsGuids.First()).IsDestroyed;
                    }
                    playerCellsList.Add(playerCell);
                }
            }
            LeftField = playerCellsList;
        }

        private void RefreshRightField()
        {
            int totalNumberOfCells = LogicAccessories.FieldRows * LogicAccessories.FieldColumns;
            var aiCellsList = new List<Cell_VM>(totalNumberOfCells);

            for (int i = 0; i < LogicAccessories.FieldRows; i++)
            {
                for (int j = 0; j < LogicAccessories.FieldColumns; j++)
                {

                    var aiCell = new Cell_VM(_gameManager.AiField[i, j].CellType == CellTypesEnum.ShipDeck)
                    {
                        IsOpen = _gameManager.AiField[i, j].IsOpen,
                        IsCellActive = _gameManager.IsPlayerTurn
                    };
                    if (aiCell.IsShipDeck)
                    {
                        aiCell.IsShipDestroyed = _gameManager.AiFleet.GetShip(_gameManager.AiField[i, j].ShipsGuids.First()).IsDestroyed;
                    }
                    aiCellsList.Add(aiCell);
                }
            }
            RightField = aiCellsList;

            RightFieldShips.RefreshState(_gameManager.AiFleet);
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
