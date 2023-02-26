using Battleship_2.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels.Abstractions
{
    public interface IGameManagerViewModel
    {
        public IShipsGridViewModel AiShips { get; }
        public IShipsGridViewModel PlayerShips { get; }
        public AutoEventCommandBase ShootCommand { get; }
    }
}
