using Battleship_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battleship_2.Views
{
    /// <summary>
    /// Interaction logic for PausePage.xaml
    /// </summary>
    public partial class PausePage : Page
    {
        public PausePage()
        {
            InitializeComponent();

            DataContext = new PausePage_VM();
        }
    }
}
