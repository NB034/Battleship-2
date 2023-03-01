using Battleship_2.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels.Abstractions
{
    public interface IGameManagerViewModel
    {
        public IShipsGridViewModel LeftFieldShips { get; }
        public IShipsGridViewModel RightFieldShips { get; }
        public List<CellViewModel> LeftField { get; }
        public List<CellViewModel> RightField { get; }
        public AutoEventCommandBase ShootCommand { get; }
    }
}
