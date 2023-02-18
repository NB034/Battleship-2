using System.Windows.Controls;

namespace Battleship_2.ViewModels
{
    public class ShipViewModel : IShipViewModel
    {
        public Image ShipImage { get; set; }
        public int Row { get; set; } = 0;
        public int Column { get; set; } = 0;
        public int RowSpan { get; set; } = 1;
        public int ColumnSpan { get; set; } = 1;

        public ShipViewModel(Image shipImage)
        {
            ShipImage = shipImage;
        }
    }
}
