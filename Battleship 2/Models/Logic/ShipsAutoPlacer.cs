﻿using Battleship_2.Models.Components;
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
            DirectionsEnum orientation = random.NextDouble() > 0.5 ? DirectionsEnum.Right : DirectionsEnum.Down;
            while (true)
            {
                if (CanPlaceShip(firstCell, orientation, numberOfCells))
                    break;
                firstCell = LogicAccessories.GetRandomCoordinates();
                orientation = random.NextDouble() > 0.5 ? DirectionsEnum.Right : DirectionsEnum.Down;
            }

            var ship = new Ship(orientation);
            for (int i = 0; i < numberOfCells; i++)
            {
                Cell cell = new Cell(firstCell.J, firstCell.I, CellTypesEnum.ShipDeck);
                cell.AddShipGuid(ship.ShipGuid);
                ship.Cells.Add(cell);
                field.Cells[firstCell.I, firstCell.J] = cell;

                if (orientation == DirectionsEnum.Right) firstCell.J++;
                else if (orientation == DirectionsEnum.Down) firstCell.I++;
            }

            field.AddShip(ref ship);
            MarkCellsNearTheShip(ship);
        }

        private bool CanPlaceShip(BaseCell firstCell, DirectionsEnum orientation, int numberOfCells)
        {
            int I = firstCell.I;
            int J = firstCell.J;
            for (int i = 0; i < numberOfCells; i++)
            {
                if (!IsCellAllowed(new BaseCell(J,I))) return false;

                if (orientation == DirectionsEnum.Right) J++;
                else if (orientation == DirectionsEnum.Down) I++;
            }
            return true;
        }

        private bool IsCellAllowed(BaseCell firstCell)
        {
            if (LogicAccessories.FieldBoundsCheck(firstCell)
                && field.Cells[firstCell.I, firstCell.J].CellType != CellTypesEnum.ShipDeck
                && IsNearCellsSatisfactory(firstCell))
                return true;

            return false;
        }

        private bool IsNearCellsSatisfactory(BaseCell firstCell)
        {
            foreach (var cell in LogicAccessories.GetfNearbyCells(firstCell))
            {
                if (field.Cells[cell.I, cell.J].CellType == CellTypesEnum.ShipDeck) return false;
            }
            return true;
        }

        private void MarkCellsNearTheShip(Ship ship)
        {
            foreach (var shipCell in ship.Cells)
            {
                foreach (var nearbyCell in LogicAccessories.GetfNearbyCells(shipCell.J, shipCell.I))
                {
                    Cell cell = field.Cells[nearbyCell.I, nearbyCell.J];
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
