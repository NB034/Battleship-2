using Battleship_2.Models.Components;
using System;
using System.Collections.Generic;

namespace Battleship_2.Models.Logic
{
    public static class LogicAccessories
    {
        public static readonly int[] NumberOfShipsDecks = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        public static readonly int NumberOfFieldRows = 10;
        public static readonly int NumberOfFieldColumns = 10;

        public static readonly Predicate<BaseCell> FieldBoundsCheck =
            (c => c.I >= 0 && c.J >= 0 && c.I < NumberOfFieldRows && c.J < NumberOfFieldColumns);

        private delegate BaseCell Get(int i, int j);
        public static List<BaseCell> GetfNearbyCells(int i, int j)
        {
            var functions = new List<Get>
            {
                new Get((i, j) => new BaseCell(i - 1, j - 1)),
                new Get((i, j) => new BaseCell(i - 1, j)),
                new Get((i, j) => new BaseCell(i - 1, j + 1)),
                                               
                new Get((i, j) => new BaseCell(i, j - 1)),
                new Get((i, j) => new BaseCell(i, j + 1)),
                                               
                new Get((i, j) => new BaseCell(i + 1, j - 1)),
                new Get((i, j) => new BaseCell(i + 1, j)),
                new Get((i, j) => new BaseCell(i + 1, j + 1)),
            };

            var cells = new List<BaseCell>();
            foreach (var function in functions)
            {
                cells.Add(function.Invoke(i, j));
            }

            cells.RemoveAll(t => !FieldBoundsCheck(t));
            return cells;
        }

        public static List<BaseCell> GetfNearbyCells(BaseCell cell)
        {
            return GetfNearbyCells(cell.I, cell.J);
        }

        private static Random random = new Random();
        public static BaseCell GetRandomCoordinates()
        {
            return new BaseCell(random.Next(NumberOfFieldRows), random.Next(NumberOfFieldColumns));
        }

        public static void OpenCellsAroundDestroyedShip(ref Field field, Guid shipGuid)
        {
            for (int i = 0; i < NumberOfFieldRows; i++)
            {
                for (int j = 0; j < NumberOfFieldColumns; j++)
                {
                    var cell = field.Cells[i, j];
                    if (cell.ShipsGuids.Contains(shipGuid))
                    {
                        cell.IsOpen = true;
                    }
                }
            }
        }
    }
}
