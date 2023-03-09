using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    public class Ship_VM : IShip_VM
    {
        public ImageBrush ShipImage { get; set; }
        public int Row { get; set; } = 0;
        public int Column { get; set; } = 0;
        public int RowSpan { get; set; } = 1;
        public int ColumnSpan { get; set; } = 1;
        public bool IsVisible { get; set; } = false;

        public Ship_VM()
        {
            ShipImage = new ImageBrush();
        }
        public Ship_VM(ImageBrush shipImage)
        {
            ShipImage = shipImage;
        }
    }
}
