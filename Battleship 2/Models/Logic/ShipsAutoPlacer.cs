using Battleship_2.Models.Components;
using System;

namespace Battleship_2.Models.Logic
{
    internal class ShipsAutoPlacer
    {
        private Field field = new Field(0, 0);
        private Random random = new Random();

        public Field GenerateField()
        {
            ResetFields();
            int[] numberOfShipsDecks = LogicAccessories.NumberOfShipsDecks;
            for (int i = 0; i < numberOfShipsDecks.Length; i++)
            {
                PlaceShip(numberOfShipsDecks[i]);
            }
            return field;
        }

        private void ResetFields()
        {
            field = new Field(LogicAccessories.NumberOfFieldRows, LogicAccessories.NumberOfFieldColumns);
            random = new Random();
        }

        private void PlaceShip(int numberOfCells)
        {
            BaseCell firstCell = LogicAccessories.GetRandomCoordinates();
            OrientationsEnum orientation = random.NextDouble() > 0.5 ? OrientationsEnum.Right : OrientationsEnum.Down;
            while (true)
            {
                if (CanPlaceShip(firstCell, orientation, numberOfCells))
                    break;
                firstCell = LogicAccessories.GetRandomCoordinates();
                orientation = random.NextDouble() > 0.5 ? OrientationsEnum.Right : OrientationsEnum.Down;
            }

            var ship = new Ship(orientation);
            for (int i = 0; i < numberOfCells; i++)
            {
                Cell cell = new Cell(firstCell.X, firstCell.Y, CellTypesEnum.ShipDeck);
                cell.AddShipGuid(ship.ShipGuid);
                ship.Cells.Add(cell);
                field.Cells[firstCell.X,firstCell.Y] = cell;

                if (orientation == OrientationsEnum.Right) firstCell.X++;
                else if (orientation == OrientationsEnum.Down) firstCell.Y++;
            }

            field.AddShip(ref ship);
            MarkCellsNearTheShip(ship);
        }

        private bool CanPlaceShip(BaseCell firstCell, OrientationsEnum orientation, int numberOfCells)
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                if (!IsCellAllowed(firstCell)) return false;

                if (orientation == OrientationsEnum.Right) firstCell.X++;
                else if (orientation == OrientationsEnum.Down) firstCell.Y++;
            }
            return true;
        }

        private bool IsCellAllowed(BaseCell firstCell)
        {
            if (LogicAccessories.FieldBoundsCheck(firstCell)
                && field.Cells[firstCell.X, firstCell.Y].CellType != CellTypesEnum.ShipDeck
                && IsNearCellsSatisfactory(firstCell))
                return true;

            return false;
        }

        private bool IsNearCellsSatisfactory(BaseCell firstCell)
        {
            foreach (var cell in LogicAccessories.GetfNearbyCells(firstCell))
            {
                if (field.Cells[cell.X, cell.Y].CellType == CellTypesEnum.ShipDeck) return false;
            }
            return true;
        }

        private void MarkCellsNearTheShip(Ship ship)
        {
            foreach (var shipCell in ship.Cells)
            {
                foreach (var nearbyCell in LogicAccessories.GetfNearbyCells(shipCell.X, shipCell.Y))
                {
                    Cell cell = field.Cells[nearbyCell.X, nearbyCell.Y];
                    if (cell.CellType != CellTypesEnum.ShipDeck)
                    {
                        cell.CellType = CellTypesEnum.NearTheShip;
                        cell.AddShipGuid(ship.ShipGuid);
                    }
                }
            }
        }
    }
}
