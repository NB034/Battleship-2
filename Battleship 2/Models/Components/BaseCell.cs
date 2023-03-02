namespace Battleship_2.Models.Components
{
    public class BaseCell
    {
        public int I { get; set; }
        public int J { get; set; }

        public BaseCell() : this(0, 0) { }
        public BaseCell(int x, int y)
        {
            I = y;
            J = x;
        }

        public static BaseCell NotValid => new BaseCell(-1, -1);
    }
}
