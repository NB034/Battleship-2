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
            (c => c.J >= 0 && c.I >= 0 && c.J < NumberOfFieldColumns && c.I < NumberOfFieldRows);

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
            return GetfNearbyCells(cell.J, cell.I);
        }

        public static DirectionsEnum GetRandomDirection()
        {
            return (DirectionsEnum)GetRandomDirectionAsNumber();
        }

        public static int GetRandomDirectionAsNumber()
        {
            return (new Random().Next(4));
        }

        public static DirectionsEnum GetOppositeDirection(DirectionsEnum orientation)
        {
            switch (orientation)
            {
                case DirectionsEnum.Left:
                    return DirectionsEnum.Right;
                case DirectionsEnum.Right:
                    return DirectionsEnum.Left;
                case DirectionsEnum.Up:
                    return DirectionsEnum.Down;
                case DirectionsEnum.Down:
                    return DirectionsEnum.Up;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation));
            }
        }

        public static int GetOppositeDirectionAsNumber(DirectionsEnum orientation)
        {
            return (int)GetOppositeDirection(orientation);
        }

        public static int GetOppositeDirectionAsNumber(int orientation)
        {
            return (int)GetOppositeDirection((DirectionsEnum)orientation);
        }

        private static Random random = new Random();
        public static BaseCell GetRandomCoordinates()
        {
            return new BaseCell(random.Next(NumberOfFieldColumns), random.Next(NumberOfFieldRows));
        }

        public static bool GetNextCell_IfValid(BaseCell cell, DirectionsEnum direction, out BaseCell? nextCell)
        {
            switch (direction)
            {
                case DirectionsEnum.Left:
                    nextCell = new BaseCell(cell.J - 1, cell.I);
                    break;
                case DirectionsEnum.Right:
                    nextCell = new BaseCell(cell.J + 1, cell.I);
                    break;
                case DirectionsEnum.Up:
                    nextCell = new BaseCell(cell.J, cell.I - 1);
                    break;
                case DirectionsEnum.Down:
                    nextCell = new BaseCell(cell.J, cell.I + 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
            if(FieldBoundsCheck(nextCell)) return true;
            nextCell = null;
            return false;
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
