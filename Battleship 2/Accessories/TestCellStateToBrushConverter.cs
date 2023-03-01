using Battleship_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship_2.Accessories
{
    internal class TestCellStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellViewModel viewModel = (CellViewModel)value;

            if (!viewModel.IsShipDeck)
                return Brushes.Blue;
            if (viewModel.IsShipDeck && !viewModel.IsShipDestroyed)
                return Brushes.Yellow;

            //if (!viewModel.IsOpen)
            //    return Brushes.Black;
            //if (viewModel.IsOpen && !viewModel.IsShipDeck)
            //    return Brushes.Blue;
            //if (viewModel.IsShipDeck && !viewModel.IsShipDestroyed)
            //    return Brushes.Yellow;

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
