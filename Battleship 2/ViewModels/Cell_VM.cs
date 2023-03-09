using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels
{
    public class Cell_VM
    {
        public Cell_VM() : this(false) { }
        public Cell_VM(bool isShipDeck)
        {
            IsShipDeck = isShipDeck;
            IsOpen = false;
            IsShipDestroyed = false;
            IsCellActive = true;
        }

        public bool IsOpen { get; set; }
        public bool IsShipDeck { get; set; }
        public bool IsShipDestroyed { get; set; }
        public bool IsCellActive { get; set; }
    }
}
