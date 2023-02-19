using Battleship_2.Exceptions;
using Battleship_2.Models.Components;
using System;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class AiFieldManager
    {
        private AiCore core;
        private Field field;
        public Field Field => field;

        public AiFieldManager(Field field)
        {
            core = new AiCore();
            this.field = field;
        }

        public void Shoot()
        {
            if (core.HasFoundShip) DestroyShip();
            else FindTarget();
        }

        private void FindTarget()
        {
            while (true)
            {
                BaseCell randomCell = LogicAccessories.GetRandomCoordinates();
                Cell fieldCell = field.Cells[randomCell.X, randomCell.Y];
                if (!fieldCell.IsOpen)
                {
                    fieldCell.IsOpen = true;

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        core.ShipWasDamaged(randomCell);
                    }
                    else if(fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                    }
                    break;
                }
            }
        }

        private void DestroyShip()
        {
            if (core.LastFoundedCell != null && core.NextShotDirection != null)
            {
                BaseCell? nextCell;
                while (LogicAccessories.GetNextCell_IfValid(core.LastFoundedCell, core.NextShotDirection.Value, out nextCell))
                {
                    core.Miss();
                }

                if (nextCell != null)
                {
                    field.OpenCell(nextCell);
                    Cell fieldCell = field.Cells[nextCell.X, nextCell.Y];

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        core.ShipWasDamaged(nextCell);
                    }
                    else if(fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        core.ShipWasDestroyed();
                        LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                    }
                    else
                    {
                        core.Miss();
                    }
                    return;
                }
            }

            throw new AiException("Ai core data has null");
        }
    }
}
