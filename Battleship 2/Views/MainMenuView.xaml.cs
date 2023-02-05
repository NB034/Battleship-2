using System.Windows;
using Battleship_2.ViewModels;

namespace Battleship_2.Views
{
    public partial class MainMenuView : Window
    {
        public MainMenuView(MainMenuViewModel viewModel)
        {
            InitializeComponent();

            viewModel.ExitButtonAction = this.Close;
            DataContext = viewModel;
        }
    }
}
