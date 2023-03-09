using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    public interface IShip_VM
    {
        ImageBrush ShipImage { get; set; }
        Int32 Row { get; set; }
        Int32 Column { get; set; }
        Int32 RowSpan { get; set; }
        Int32 ColumnSpan { get; set; }
        bool IsVisible { get; set; }
    }
}
