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
            (c => c.X > 0 || c.Y > 0 || c.X <= NumberOfFieldRows || c.Y <= NumberOfFieldColumns);

        private delegate BaseCell Get(int x, int y);
        public static List<BaseCell> GetfNearbyCells(int x, int y)
        {
            var functions = new List<Get>
            {
                new Get((x, y) => new BaseCell(x - 1, y - 1)),
                new Get((x, y) => new BaseCell(x, y - 1)),
                new Get((x, y) => new BaseCell(x + 1, y - 1)),

                new Get((x, y) => new BaseCell(x - 1, y)),
                new Get((x, y) => new BaseCell(x + 1, y)),

                new Get((x, y) => new BaseCell(x - 1, y + 1)),
                new Get((x, y) => new BaseCell(x, y + 1)),
                new Get((x, y) => new BaseCell(x + 1, y + 1)),
            };

            var cells = new List<BaseCell>();
            foreach (var function in functions)
            {
                cells.Add(function.Invoke(x, y));
            }

            cells.RemoveAll(t => !FieldBoundsCheck(t));
            return cells;
        }

        public static List<BaseCell> GetfNearbyCells(BaseCell cell)
        {
            return GetfNearbyCells(cell.X, cell.Y);
        }

        public static OrientationsEnum GetRandomDirection()
        {
            return (OrientationsEnum)GetRandomDirectionAsNumber();
        }

        public static int GetRandomDirectionAsNumber()
        {
            return (new Random().Next(Enum.GetValues(typeof(OrientationsEnum)).Length));
        }

        public static OrientationsEnum GetOppositeDirection(OrientationsEnum orientation)
        {
            switch (orientation)
            {
                case OrientationsEnum.Left:
                    return OrientationsEnum.Right;
                case OrientationsEnum.Right:
                    return OrientationsEnum.Left;
                case OrientationsEnum.Up:
                    return OrientationsEnum.Down;
                case OrientationsEnum.Down:
                    return OrientationsEnum.Up;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation));
            }
        }

        public static int GetOppositeDirectionAsNumber(OrientationsEnum orientation)
        {
            return (int)GetOppositeDirection(orientation);
        }

        public static int GetOppositeDirectionAsNumber(int orientation)
        {
            return (int)GetOppositeDirection((OrientationsEnum)orientation);
        }

        private static Random random = new Random();
        public static BaseCell GetRandomCoordinates()
        {
            return new BaseCell(random.Next(NumberOfFieldRows), NumberOfFieldColumns);
        }

        public static bool GetNextCell_IfValid(BaseCell cell, OrientationsEnum direction, out BaseCell? nextCell)
        {
            switch (direction)
            {
                case OrientationsEnum.Left:
                    nextCell = new BaseCell(cell.X - 1, cell.Y);
                    break;
                case OrientationsEnum.Right:
                    nextCell = new BaseCell(cell.X + 1, cell.Y);
                    break;
                case OrientationsEnum.Up:
                    nextCell = new BaseCell(cell.X, cell.Y - 1);
                    break;
                case OrientationsEnum.Down:
                    nextCell = new BaseCell(cell.X, cell.Y + 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
            if(FieldBoundsCheck(nextCell)) return true;
            nextCell = null;
            return false;
        }
    }
}
