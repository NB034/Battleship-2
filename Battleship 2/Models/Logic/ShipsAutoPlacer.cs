﻿using Battleship_2.Models.Components;
using System;

namespace Battleship_2.Models.Logic
{
    internal class ShipsAutoPlacer
    {
        private Field _field = new Field(0, 0);
        private Random _random = new Random();

        public Field GenerateField()
        {
            ResetFields();
            int[] numberOfShipsDecks = Fleet.ShipsDecks;
            for (int i = 0; i < numberOfShipsDecks.Length; i++)
            {
                PlaceShip(numberOfShipsDecks[i]);
            }
            return _field;
        }

        private void ResetFields()
        {
            _field = new Field(LogicAccessories.FieldRows, LogicAccessories.FieldColumns);
            _random = new Random();
        }

        private void PlaceShip(int numberOfCells)
        {
            BaseCell firstCell;
            OrientationsEnum orientation;
            while (true)
            {
                firstCell = LogicAccessories.GetRandomCoordinates();
                orientation = _random.NextDouble() > 0.5 ? OrientationsEnum.Horizontal : OrientationsEnum.Vertical;
                if (CanPlaceShip(firstCell, orientation, numberOfCells))
                    break;
            }

            var ship = new Ship(orientation);
            for (int i = 0; i < numberOfCells; i++)
            {
                Cell cell = new Cell(firstCell.I, firstCell.J, CellTypesEnum.ShipDeck);
                cell.AddShipGuid(ship.ShipGuid);
                ship.Cells.Add(cell);
                _field.Cells[firstCell.I, firstCell.J] = cell;

                if (orientation == OrientationsEnum.Horizontal) firstCell.J++;
                else if (orientation == OrientationsEnum.Vertical) firstCell.I++;
            }

            _field.AddShip(ship);
            MarkCellsNearTheShip(ship);
        }

        private bool CanPlaceShip(BaseCell firstCell, OrientationsEnum orientation, int numberOfCells)
        {
            int I = firstCell.I;
            int J = firstCell.J;
            for (int i = 0; i < numberOfCells; i++)
            {
                if (!IsCellAllowed(new BaseCell(I, J))) return false;

                if (orientation == OrientationsEnum.Horizontal) J++;
                else if (orientation == OrientationsEnum.Vertical) I++;
            }
            return true;
        }

        private bool IsCellAllowed(BaseCell firstCell)
        {
            if (LogicAccessories.FieldBoundsCheck(firstCell)
                && _field.Cells[firstCell.I, firstCell.J].CellType != CellTypesEnum.ShipDeck
                && IsNearCellsSatisfactory(firstCell))
                return true;

            return false;
        }

        private bool IsNearCellsSatisfactory(BaseCell firstCell)
        {
            foreach (var cell in LogicAccessories.GetfNearbyCells(firstCell))
            {
                if (_field.Cells[cell.I, cell.J].CellType == CellTypesEnum.ShipDeck) return false;
            }
            return true;
        }

        private void MarkCellsNearTheShip(Ship ship)
        {
            foreach (var shipCell in ship.Cells)
            {
                foreach (var nearbyCell in LogicAccessories.GetfNearbyCells(shipCell.I, shipCell.J))
                {
                    Cell cell = _field.Cells[nearbyCell.I, nearbyCell.J];
                    if (cell.CellType != CellTypesEnum.ShipDeck && !cell.ShipsGuids.Contains(ship.ShipGuid))
                    {
                        cell.CellType = CellTypesEnum.NearTheShip;
                        cell.AddShipGuid(ship.ShipGuid);
                    }
                }
            }
        }
    }
}
