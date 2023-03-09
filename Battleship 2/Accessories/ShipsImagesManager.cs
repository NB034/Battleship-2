using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship_2.Accessories
{
    public class ShipsImagesManager : IShipsImagesManager
    {
        public ImageBrush[] GetFirstShipsSet()
        {
            return new[]
            {
                Application.Current.Resources["Fleet1Ship4"] as ImageBrush ?? new ImageBrush(),
                Application.Current.Resources["Fleet1Ship3"] as ImageBrush ?? new ImageBrush(),
                Application.Current.Resources["Fleet1Ship2"] as ImageBrush ?? new ImageBrush(),
                Application.Current.Resources["Fleet1Ship1"] as ImageBrush ?? new ImageBrush()
            };
        }

        public ImageBrush[] GetSecondShipsSet()
        {
            return new[]
            {
                Application.Current.Resources["Fleet2Ship4"] as ImageBrush ?? new ImageBrush(),
                Application.Current.Resources["Fleet2Ship3"] as ImageBrush ?? new ImageBrush(),
                Application.Current.Resources["Fleet2Ship2"] as ImageBrush ?? new ImageBrush(),
                Application.Current.Resources["Fleet2Ship1"] as ImageBrush ?? new ImageBrush()
            };
        }
    }
}
