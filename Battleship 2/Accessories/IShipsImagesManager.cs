using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship_2.Accessories
{
    public interface IShipsImagesManager
    {
        String[] GetFirstShipsSet();
        String[] GetSecondShipsSet();
    }
}
