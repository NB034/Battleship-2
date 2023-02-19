using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Battleship_2.Accessories
{
    public class ShipsImagesManager : IShipsImagesManager
    {
        public Image[] GetFirstShipsSet()
        {
            return new[]
            {
                Application.Current.Resources["Fleet1Ship4"] as Image ?? new Image(),
                Application.Current.Resources["Fleet1Ship3"] as Image ?? new Image(),
                Application.Current.Resources["Fleet1Ship2"] as Image ?? new Image(),
                Application.Current.Resources["Fleet1Ship1"] as Image ?? new Image()
            };
        }

        public Image[] GetSecondShipsSet()
        {
            return new[]
            {
                Application.Current.Resources["Fleet2Ship4"] as Image ?? new Image(),
                Application.Current.Resources["Fleet2Ship3"] as Image ?? new Image(),
                Application.Current.Resources["Fleet2Ship2"] as Image ?? new Image(),
                Application.Current.Resources["Fleet2Ship1"] as Image ?? new Image()
            };
        }
    }
}
