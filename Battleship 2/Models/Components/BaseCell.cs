namespace Battleship_2.Models.Components
{
    public class BaseCell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public BaseCell() : this(0, 0) { }
        public BaseCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
