using Battleship_2.Accessories;
using Battleship_2.Exceptions;
using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Battleship_2.ViewModels
{
    class ShipsGrid_VM : IShipsGrid_VM, INotifyPropertyChanged
    {
        private readonly IShip_VM[] shipViewModels;
        private readonly DirectionsEnum location;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IShip_VM Decks_4 => shipViewModels[0];

        public IShip_VM Decks_3_Num_1 => shipViewModels[1];

        public IShip_VM Decks_3_Num_2 => shipViewModels[2];

        public IShip_VM Decks_2_Num_1 => shipViewModels[3];

        public IShip_VM Decks_2_Num_2 => shipViewModels[4];

        public IShip_VM Decks_2_Num_3 => shipViewModels[5];

        public IShip_VM Decks_1_Num_1 => shipViewModels[6];

        public IShip_VM Decks_1_Num_2 => shipViewModels[7];

        public IShip_VM Decks_1_Num_3 => shipViewModels[8];

        public IShip_VM Decks_1_Num_4 => shipViewModels[9];

        public ShipsGrid_VM(string[] shipImagesUri, Fleet fleet, DirectionsEnum gridLocation)
        {
            location = gridLocation;
            int[] decks = LogicAccessories.NumberOfShipsDecks;
            fleet.Ships.Sort((s1, s2) => s2.Cells.Count.CompareTo(s1.Cells.Count));
            shipViewModels = new Ship_VM[decks.Length];

            if (shipImagesUri.Length != 4) throw new ArgumentException("Wrong number of images");
            if (gridLocation == DirectionsEnum.Up || gridLocation == DirectionsEnum.Down) throw new ArgumentException("Grid location can be only left or right");
            if (decks.Length != 10) throw new ShipsGridViewModelException("Class [ShipsGridViewModel] can only be used when [LogicAccessories.NumberOfShipsDecks.Length] equals 10");

            for (int i = 0; i < shipViewModels.Length; i++)
            {
                var shipVm = new Ship_VM();
                var fleetShip = fleet.Ships[i];
                shipVm.ShipImageUri = shipImagesUri[decks[i] - 1];
                //Cell rootCell;

                if (gridLocation == DirectionsEnum.Left) shipVm.IsVisible = true;
                if (gridLocation == DirectionsEnum.Right) shipVm.IsVisible = false;

                if (fleetShip.Orientation == OrientationsEnum.Horizontal)
                {
                    if (gridLocation == DirectionsEnum.Left) shipVm.Rotation = new RotateTransform(90);
                    if (gridLocation == DirectionsEnum.Right) shipVm.Rotation = new RotateTransform(270);

                    shipVm.ColumnSpan = fleetShip.Cells.Count;
                    fleetShip.Cells.Sort((s1, s2) => s1.J.CompareTo(s2.J));
                }
                else
                {
                    shipVm.RowSpan = fleetShip.Cells.Count;
                    fleetShip.Cells.Sort((s1, s2) => s1.I.CompareTo(s2.I));
                }

                shipVm.Row = fleetShip.Cells.First().I;
                shipVm.Column = fleetShip.Cells.First().J;

                shipViewModels[i] = shipVm;
            }
        }

        public void RefreshState(Fleet fleet)
        {
            if (location == DirectionsEnum.Right)
            {
                var ships = fleet.Ships;
                for (int i = 0; i < ships.Count; i++)
                {
                    if (ships[i].IsDestroyed != shipViewModels[i].IsVisible)
                    {
                        shipViewModels[i].IsVisible = ships[i].IsDestroyed;
                    }
                }
            }
            NotifyAllPropertiesChanged();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotifyAllPropertiesChanged()
        {
            NotifyPropertyChanged(nameof(Decks_1_Num_1));
            NotifyPropertyChanged(nameof(Decks_1_Num_2));
            NotifyPropertyChanged(nameof(Decks_1_Num_3));
            NotifyPropertyChanged(nameof(Decks_1_Num_4));
            NotifyPropertyChanged(nameof(Decks_2_Num_1));
            NotifyPropertyChanged(nameof(Decks_2_Num_2));
            NotifyPropertyChanged(nameof(Decks_2_Num_3));
            NotifyPropertyChanged(nameof(Decks_3_Num_1));
            NotifyPropertyChanged(nameof(Decks_3_Num_2));
            NotifyPropertyChanged(nameof(Decks_4));
        }
    }
}
