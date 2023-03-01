using System;

namespace Battleship_2.Models.Components
{
    public class Field
    {
        public Cell[,] Cells { get; set; }
        public Fleet Fleet { get; private set; }

        public Field(int fieldRows, int fieldColumns) : this(fieldRows, fieldColumns, new Fleet()) { }
        public Field(int fieldRows, int fieldColumns, Fleet fleet)
        {
            Cells = new Cell[fieldRows, fieldColumns];
            Fleet = fleet;

            for (int i = 0; i < fieldRows; i++)
            {
                for (int j = 0; j < fieldColumns; j++)
                {
                    Cell cell = new Cell(j, i);
                    Cells[i, j] = cell;
                }
            }
        }

        public void PlaceNewFleet(ref Fleet fleet)
        {
            Fleet = fleet;
            foreach (var cell in Fleet.GetAllShipsCells())
            {
                Cells[cell.Y, cell.X] = cell;
            }
        }

        public void AddShip(ref Ship ship)
        {
            Fleet.AddShip(ref ship);
            for (int i = 0; i < ship.Cells.Count; i++)
            {
                Cell cell = ship.Cells[i];
                Cells[cell.Y, cell.X] = cell;
            }
        }

        public void OpenCell(BaseCell cell)
        {
            Cell fieldCell = Cells[cell.Y, cell.X];
            if (fieldCell.IsOpen) throw new ArgumentException("Cell was already open.");
            fieldCell.IsOpen = true;
        }

        public void OpenRange(BaseCell[] cells)
        {
            foreach (var cell in cells)
            {
                if (!Cells[cell.Y, cell.X].IsOpen)
                {
                    Cells[cell.Y, cell.X].IsOpen = true;
                }
            }
        }
    }
}
