using Battleship_2.Exceptions;
using Battleship_2.Models.Components;
using System;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class AiFieldManager : IFieldManager
    {
        private AiCore core;
        private Field field;
        private BaseCell lastOpenedCell;

        public Field Field => field;
        public BaseCell LastOpenedCell => lastOpenedCell;

        public AiFieldManager(Field field)
        {
            core = new AiCore();
            this.field = field;
            lastOpenedCell = BaseCell.NotValid;
        }

        public void Shoot()
        {
            if (core.HasFoundShip)
            {
                lastOpenedCell = DestroyShip();
            }
            else
            {
                lastOpenedCell = FindTarget();
            }
        }

        private BaseCell FindTarget()
        {
            while (true)
            {
                BaseCell randomCell = LogicAccessories.GetRandomCoordinates();
                Cell fieldCell = field.Cells[randomCell.Y, randomCell.X];
                if (!fieldCell.IsOpen)
                {
                    fieldCell.IsOpen = true;

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        core.ShipWasDamaged(randomCell);
                    }
                    else if (fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        core.ShipWasDestroyed();
                        LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                    }
                    else
                    {
                        core.Miss();
                    }
                    return fieldCell.Base;
                }
            }
        }

        private BaseCell DestroyShip()
        {
            if (core.LastFoundedCell != null && core.NextShotDirection != null)
            {
                BaseCell? nextCell;
                while (!LogicAccessories.GetNextCell_IfValid(core.LastFoundedCell, core.NextShotDirection.Value, out nextCell))
                {
                    core.Miss();
                }

                if (nextCell != null)
                {
                    field.OpenCell(nextCell);
                    Cell fieldCell = field.Cells[nextCell.Y, nextCell.X];

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        core.ShipWasDamaged(nextCell);
                    }
                    else if (fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        core.ShipWasDestroyed();
                        LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                    }
                    else
                    {
                        core.Miss();
                    }
                    return fieldCell.Base;
                }
            }

            throw new AiException("Ai core data has null");
        }
    }
}
