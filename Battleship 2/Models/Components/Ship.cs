﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship_2.Models.Components
{
    public class Ship
    {
        public int NumberOfDecks => Cells.Count;
        public List<Cell> Cells { get; set; }
        public Guid ShipGuid { get; }
        public bool IsDestroyed => !Cells.Any(c => !c.IsOpen);
        public DirectionsEnum Orientation { get; }

        public Ship(DirectionsEnum orientation) : this(new List<Cell>(), orientation) { }
        public Ship(List<Cell> cells, DirectionsEnum orientation)
        {
            ShipGuid = Guid.NewGuid();
            Cells = cells;
            Orientation = orientation;
        }

        public void AddCell(ref Cell cell)
        {
            Cells.Add(cell);
        }
    }
}
