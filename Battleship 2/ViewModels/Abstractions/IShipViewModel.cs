using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Battleship_2.ViewModels
{
    public interface IShipViewModel
    {
        Image ShipImage { get; set; }
        Int32 Row { get; set; }
        Int32 Column { get; set; }
        Int32 RowSpan { get; set; }
        Int32 ColumnSpan { get; set; }
        bool IsVisible { get; set; }
    }
}
