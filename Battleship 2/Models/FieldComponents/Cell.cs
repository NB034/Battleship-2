using System;

namespace Battleship_2.Models.FieldComponents
{
    public class Cell
    {
        public int I { get; set; }
        public int J { get; set; }

        public Cell() : this(0, 0) { }
        public Cell(int i, int j)
        {
            I = i;
            J = j;
        }

        public static Cell NotValid => new(-1, -1);

        public static Cell RandomCell =>  new(Random.Shared.Next(Field.FieldRows), Random.Shared.Next(Field.FieldColumns));
    }
}
