using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship_2.Models.Components
{
    internal class Ship
    {
        public int NumberOfDecks => Cells.Count;
        public List<Cell> Cells { get; set; }
        public Guid ShipGuid { get; }
        public bool IsDestroyed => Cells.Any(c => !c.IsOpen);

        public Ship() : this(new List<Cell>()) { }
        public Ship(List<Cell> cells)
        {
            ShipGuid = Guid.NewGuid();
            Cells = cells;
        }

        public void AddCell(ref Cell cell)
        {
            Cells.Add(cell);
        }
    }
}
