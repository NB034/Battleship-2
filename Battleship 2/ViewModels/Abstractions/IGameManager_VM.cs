using Battleship_2.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels.Abstractions
{
    public interface IGameManager_VM
    {
        public IShipsGrid_VM LeftFieldShips { get; }
        public IShipsGrid_VM RightFieldShips { get; }
        public List<Cell_VM> LeftField { get; }
        public List<Cell_VM> RightField { get; }
        public AutoEventCommandBase ShootCommand { get; }
    }
}
