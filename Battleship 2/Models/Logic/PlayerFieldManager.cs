using Battleship_2.Models.Components;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class PlayerFieldManager
    {
        private Field field;
        public Field Field => field;

        public PlayerFieldManager(Field field)
        {
            this.field = field;
        }

        public void Shoot(BaseCell selectedCell)
        {
            Cell fieldCell = field.Cells[selectedCell.X, selectedCell.Y];
            field.OpenCell(fieldCell);
            if (fieldCell.CellType == CellTypesEnum.ShipDeck
                && field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
            {
                LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
            }
        }
    }
}
