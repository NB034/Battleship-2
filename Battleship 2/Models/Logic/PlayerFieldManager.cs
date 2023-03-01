using Battleship_2.Models.Components;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class PlayerFieldManager : IFieldManager
    {
        private Field field;
        private BaseCell lastOpenedCell;

        public Field Field => field;
        public BaseCell LastOpenedCell => lastOpenedCell;

        public PlayerFieldManager(Field field)
        {
            this.field = field;
            lastOpenedCell = BaseCell.NotValid;
        }

        public void Shoot(BaseCell selectedCell)
        {
            Cell fieldCell = field.Cells[selectedCell.Y, selectedCell.X];
            field.OpenCell(fieldCell);
            if (fieldCell.CellType == CellTypesEnum.ShipDeck
                && field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
            {
                LogicAccessories.OpenCellsAroundDestroyedShip(ref field, fieldCell.ShipsGuids.First());
            }
            lastOpenedCell = fieldCell.Base;
        }
    }
}
