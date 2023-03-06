using Battleship_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship_2.Accessories
{
    internal class RightFieldCellToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellViewModel viewModel = (CellViewModel)value;
            if (!viewModel.IsOpen && !CellViewModel.IsTurnOver)
                return Application.Current.Resources["BackgroundBrush"];
            if (!viewModel.IsOpen)
                return Brushes.Transparent;
            if (viewModel.IsOpen && !viewModel.IsShipDeck)
                return Application.Current.Resources["OpenCellBrush"];
            if (viewModel.IsShipDeck && !viewModel.IsShipDestroyed)
                return Application.Current.Resources["DamagedShipCellBrush"];

            return Application.Current.Resources["DestroyedShipBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
