using Battleship_2.Command;
using Battleship_2.Models.Components;
using System.Collections.ObjectModel;

namespace Battleship_2.ViewModels
{
    internal class GamePlayerVsAiViewModel : IGamePageViewModel
    {
        private readonly AutoEventCommandBase openMenuCommand;
        private readonly AutoEventCommandBase shootCommand;

        //private readonly IShipsGridViewModel leftShips;
        //private readonly IShipsGridViewModel rightShips;

        public GamePlayerVsAiViewModel()
        {
            openMenuCommand = new AutoEventCommandBase(o => o.ToString(), _ => true);
            shootCommand = new AutoEventCommandBase(o => o.ToString(), _ => true);

            //leftShips = new ShipsGridViewModel();
            //rightShips = new ShipsGridViewModel();

            LeftField = new ObservableCollection<Cell>();
            RightField = new ObservableCollection<Cell>();
            for (int i = 0; i < 100; i++)
            {
                LeftField.Add(new Cell());
                RightField.Add(new Cell());
            }
        }

        public AutoEventCommandBase OpenMenuCommand => openMenuCommand;
        public AutoEventCommandBase ShootCommand => shootCommand;
        public ObservableCollection<Cell> LeftField { get; set; }
        public ObservableCollection<Cell> RightField { get; set; }
        //public IShipsGridViewModel LeftShips => leftShips;
        //public IShipsGridViewModel RightShips => rightShips;
    }
}
