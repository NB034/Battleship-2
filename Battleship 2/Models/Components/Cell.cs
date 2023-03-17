using System;
using System.Collections.Generic;

namespace Battleship_2.Models.Components
{
    public class Cell : BaseCell
    {
        public bool IsOpen { get; set; }
        public CellTypesEnum CellType { get; set; }
        public List<Guid> ShipsGuids { get; }

        public Cell() : this(0, 0, CellTypesEnum.Empty) { }
        public Cell(int i, int j) : this(i, j, CellTypesEnum.Empty) { }
        public Cell(int i, int j, CellTypesEnum cellType) : base(i, j)
        {
            CellType = cellType;
            IsOpen = false;
            ShipsGuids = new List<Guid>();
        }

        public virtual void AddShipGuid(Guid guid)
        {
            if (CellType == CellTypesEnum.Empty && ShipsGuids.Count >= 0) return;
            if (CellType == CellTypesEnum.ShipDeck && ShipsGuids.Count >= 1) return;
            if (CellType == CellTypesEnum.NearTheShip && ShipsGuids.Count >= 4) return;
            ShipsGuids.Add(guid);
        }

        public BaseCell Base => new BaseCell(I, J);
    }
}
