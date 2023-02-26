using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private bool isOpen;
        private bool isShipDeck;
        private bool isShipDestroyed;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CellViewModel(bool isShipDeck)
        {
            this.isShipDeck = isShipDeck;
            isOpen = false;
            isShipDestroyed = false;
        }

        public bool IsOpen { get => isOpen; set => SetProperty(ref isOpen, value, nameof(IsOpen)); }
        public bool IsShipDeck { get => isShipDeck; }
        public bool IsShipDestroyed { get => isShipDestroyed; set => SetProperty(ref isShipDestroyed, value, nameof(IsShipDestroyed)); }

        private void SetProperty<T>(ref T oldValue, T newValue, string propertyName)
        {
            if (!oldValue?.Equals(newValue) ?? newValue != null)
            {
                oldValue = newValue;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
