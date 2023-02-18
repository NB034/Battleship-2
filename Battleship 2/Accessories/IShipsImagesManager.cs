using System.Windows.Controls;

namespace Battleship_2.Accessories
{
    public interface IShipsImagesManager
    {
        Image[] GetFirstShipsSet();
        Image[] GetSecomdShipsSet();
    }
}
