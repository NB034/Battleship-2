using Battleship_2.Models.Components;

namespace Battleship_2.Models.Ai
{
    internal class Ai_TurnInfo
    {
        public BaseCell LastOpenedCell { get; set; } = BaseCell.NotValid;
        public bool WasShipDestroyed { get; set; }
        public bool WasShotSuccessfull { get; set; }
    }
}
