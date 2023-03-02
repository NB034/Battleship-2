using Battleship_2.Models.Components;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class PlayerFieldManager : IFieldManager
    {
        private Field field;
        public Field Field => field;

        public PlayerFieldManager(Field field)
        {
            this.field = field;
        }

        public bool Shoot(BaseCell selectedCell)
        {
            Cell fieldCell = field.Cells[selectedCell.I, selectedCell.J];
            field.OpenCell(fieldCell);

            if(fieldCell.CellType == CellTypesEnum.ShipDeck)
            {
                if (field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                {
                    LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
                }
                return true;
            }
            return false;
        }
    }
}
