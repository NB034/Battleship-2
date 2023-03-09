using Battleship_2.Command;
using Battleship_2.Models.Components;
using Battleship_2.ViewModels.Abstractions;
using System.Collections.ObjectModel;

namespace Battleship_2.ViewModels
{
    public interface IGamePage_VM
    {
        IGameManager_VM GameManager { get; }
        AutoEventCommandBase OpenMenuCommand { get; }
    }
}
