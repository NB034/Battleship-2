using Battleship_2.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels
{
    public interface IShipsGridViewModel
    {
        IShipViewModel Decks_4 { get; }
        IShipViewModel Decks_3_Num_1 { get; }
        IShipViewModel Decks_3_Num_2 { get; }
        IShipViewModel Decks_2_Num_1 { get; }
        IShipViewModel Decks_2_Num_2 { get; }
        IShipViewModel Decks_2_Num_3 { get; }
        IShipViewModel Decks_1_Num_1 { get; }
        IShipViewModel Decks_1_Num_2 { get; }
        IShipViewModel Decks_1_Num_3 { get; }
        IShipViewModel Decks_1_Num_4 { get; }

        void RefreshState(Fleet fleet);
    }
}
