using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship_2.Models.Components
{
    public class Fleet
    {
        public List<Ship> Ships { get; set; }
        public bool IsDestroyed => Ships.Any(s => !s.IsDestroyed);

        public Fleet() : this(new List<Ship>()) { }
        public Fleet(List<Ship> ships)
        {
            Ships = ships;
        }

        public void AddShip(ref Ship ship)
        {
            Ships.Add(ship);
        }

        public Ship GetShip(Guid guid)
        {
            return Ships.First(s => s.ShipGuid == guid) ?? throw new ArgumentException("Invalid ship guid");
        }

        public List<Cell> GetAllShipsCells()
        {
            var cells = new List<Cell>();
            foreach (var ship in Ships)
            {
                cells.AddRange(ship.Cells);
            }
            return cells;
        }
    }
}
