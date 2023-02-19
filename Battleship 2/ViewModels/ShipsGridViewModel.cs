using Battleship_2.Accessories;
using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    class ShipsGridViewModel : IShipsGridViewModel, INotifyPropertyChanged
    {
        private IShipViewModel[] shipViewModels;
        private OrientationsEnum location;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IShipViewModel Decks_4 => throw new NotImplementedException();

        public IShipViewModel Decks_3_Num_1 => throw new NotImplementedException();

        public IShipViewModel Decks_3_Num_2 => throw new NotImplementedException();

        public IShipViewModel Decks_2_Num_1 => throw new NotImplementedException();

        public IShipViewModel Decks_2_Num_2 => throw new NotImplementedException();

        public IShipViewModel Decks_2_Num_3 => throw new NotImplementedException();

        public IShipViewModel Decks_1_Num_1 => throw new NotImplementedException();

        public IShipViewModel Decks_1_Num_2 => throw new NotImplementedException();

        public IShipViewModel Decks_1_Num_3 => throw new NotImplementedException();

        public IShipViewModel Decks_1_Num_4 => throw new NotImplementedException();

        public ShipsGridViewModel(Image[] images, Fleet fleet, OrientationsEnum gridLocation)
        {
            if (images.Length != 4)
                throw new ArgumentException("Wrong number of images");
            if (gridLocation == OrientationsEnum.Up || gridLocation == OrientationsEnum.Down)
                throw new ArgumentException("Grid location can be only left or right");
            location = gridLocation;

            int[] decks = LogicAccessories.NumberOfShipsDecks;
            shipViewModels = new ShipViewModel[decks.Length];

            for (int i = 0; i < shipViewModels.Length; i++)
            {
                var shipViewModel = new ShipViewModel(images[decks[i] - 1]);
                var fleetShip = fleet.Ships[i];

                if (gridLocation == OrientationsEnum.Left) shipViewModel.IsVisible = true;
                if (gridLocation == OrientationsEnum.Right) shipViewModel.IsVisible = false;

                Cell rootCell;
                if (fleetShip.Orientation == OrientationsEnum.Left || fleetShip.Orientation == OrientationsEnum.Right)
                {
                    if (gridLocation == OrientationsEnum.Left)
                        shipViewModel.ShipImage.RenderTransform = new RotateTransform(90);
                    if (gridLocation == OrientationsEnum.Right)
                        shipViewModel.ShipImage.RenderTransform = new RotateTransform(270);

                    shipViewModel.ColumnSpan = fleetShip.Cells.Count;

                    int x = fleetShip.Cells.Select(cell => cell.X).Min();
                    rootCell = fleetShip.Cells.Where(cell => cell.X == x).First();
                }
                else
                {
                    shipViewModel.RowSpan = fleetShip.Cells.Count;

                    int y = fleetShip.Cells.Select(cell => cell.Y).Min();
                    rootCell = fleetShip.Cells.Where(cell => cell.Y == y).First();
                }

                shipViewModel.Row = rootCell.X;
                shipViewModel.Column = rootCell.Y;

                shipViewModels[i] = shipViewModel;
            }
        }

        public void RefreshState(Fleet fleet)
        {
            if(location == OrientationsEnum.Right)
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
        }
    }
}
