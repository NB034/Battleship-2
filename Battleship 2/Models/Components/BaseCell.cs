namespace Battleship_2.Models.Components
{
    public class BaseCell
    {
        public int I { get; set; }
        public int J { get; set; }

        public BaseCell() : this(0, 0) { }
        public BaseCell(int i, int j)
        {
            I = i;
            J = j;
        }

        public static BaseCell NotValid => new BaseCell(-1, -1);
    }
}
