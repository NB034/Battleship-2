using Battleship_2.Exceptions;
using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using System;
using System.Linq;

namespace Battleship_2.Models.Ai
{
    internal class Obsolete_Ai_FieldManager : IFieldManager
    {
        private Obsolete_Ai_Core core;
        private Field field;

        public Field Field => field;

        public Obsolete_Ai_FieldManager(Field field)
        {
            core = new Obsolete_Ai_Core();
            this.field = field;
        }

        public bool Shoot()
        {
            if (core.HasFoundShip)
            {
                return DestroyShip();
            }
            else
            {
                return FindTarget();
            }
        }

        private bool FindTarget()
        {
            while (true)
            {
                BaseCell randomCell = LogicAccessories.GetRandomCoordinates();
                Cell fieldCell = field.Cells[randomCell.I, randomCell.J];
                if (!fieldCell.IsOpen)
                {
                    fieldCell.IsOpen = true;

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        core.ShipWasDamaged(randomCell);
                        return true;
                    }
                    else if (fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        core.ShipWasDestroyed();
                        LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                        return true;
                    }
                    else
                    {
                        core.Miss();
                        return false;
                    }
                }
            }
        }

        private bool DestroyShip()
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
                    Cell fieldCell = field.Cells[nextCell.I, nextCell.J];

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        core.ShipWasDamaged(nextCell);
                        return true;
                    }
                    else if (fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        core.ShipWasDestroyed();
                        LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                        return true;
                    }
                    else
                    {
                        core.Miss();
                        return false;
                    }
                }
            }
            throw new AiException("Ai core data has null");
        }
    }
}
