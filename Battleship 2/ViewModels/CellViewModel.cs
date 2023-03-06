using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels
{
    public class CellViewModel
    {
        public CellViewModel() : this(false) { }
        public CellViewModel(bool isShipDeck)
        {
            IsShipDeck = isShipDeck;
            IsOpen = false;
            IsShipDestroyed = false;
        }
        static CellViewModel()
        {
            IsTurnOver = true;
        }

        public bool IsOpen { get; set; }
        public bool IsShipDeck { get; set; }
        public bool IsShipDestroyed { get; set; }
        public static bool IsTurnOver { get; set; }
    }
}
