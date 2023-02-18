using Battleship_2.Command;
using Battleship_2.Models.Components;
using System.Collections.ObjectModel;

namespace Battleship_2.ViewModels
{
    public interface IGamePageViewModel
    {
        AutoEventCommandBase OpenMenuCommand { get; }
        AutoEventCommandBase ShootCommand { get; }

        ObservableCollection<Cell> LeftField { get; }
        ObservableCollection<Cell> RightField { get;}

        //IShipsGridViewModel LeftShips { get; }
        //IShipsGridViewModel RightShips { get; }
    }
}
