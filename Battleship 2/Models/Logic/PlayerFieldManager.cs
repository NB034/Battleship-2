using Battleship_2.Models.Components;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class PlayerFieldManager : IFieldManager
    {
        private Field _field;
        public Field Field => _field;

        public PlayerFieldManager(Field field)
        {
            this._field = field;
        }

        public bool Shoot(BaseCell selectedCell)
        {
            Cell fieldCell = _field.Cells[selectedCell.I, selectedCell.J];
            _field.OpenCell(fieldCell);

            if(fieldCell.CellType == CellTypesEnum.ShipDeck)
            {
                if (_field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                {
                    LogicAccessories.OpenCellsAroundDestroyedShip(ref _field, fieldCell.ShipsGuids.First());
                }
                return true;
            }
            return false;
        }
    }
}
