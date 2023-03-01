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
    internal class CellStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellViewModel viewModel = (CellViewModel)value;
            if (!viewModel.IsOpen) 
                return Application.Current.Resources["BackgroundBrush"] as SolidColorBrush ?? Brushes.Black;
            if (viewModel.IsOpen && !viewModel.IsShipDeck)
                return Application.Current.Resources["OpenCellBrush"] as SolidColorBrush ?? Brushes.Blue;
            if (viewModel.IsShipDeck && !viewModel.IsShipDestroyed) 
                return Application.Current.Resources["DamagedShipCellBrush"] as SolidColorBrush ?? Brushes.Yellow;

            return Application.Current.Resources["DestroyedShipBrush"] as SolidColorBrush ?? Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
