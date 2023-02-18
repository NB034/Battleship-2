using Battleship_2.Models.Components;

namespace Battleship_2.Models.Logic
{
    internal class PlayerFieldManager
    {
        private Field field;
        public Field Field => field;

        public PlayerFieldManager(ref Field field)
        {
            this.field = field;
        }

        public void Shoot(BaseCell selectedCell)
        {
            Cell fieldCell = field.Cells[selectedCell.X, selectedCell.Y];
            field.OpenCell(fieldCell);
        }
    }
}
