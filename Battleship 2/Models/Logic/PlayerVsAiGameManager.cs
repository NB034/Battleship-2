using Battleship_2.Models.Components;
using System;
using System.Diagnostics;

namespace Battleship_2.Models.Logic
{
    internal class PlayerVsAiGameManager
    {
        private readonly AiFieldManager aiFieldManager;
        private readonly PlayerFieldManager playerFieldManager;
        public event Action? FieldChanged;

        public PlayerVsAiGameManager(AiFieldManager aiFieldManager, PlayerFieldManager playerFieldManager)
        {
            this.aiFieldManager = aiFieldManager;
            this.playerFieldManager = playerFieldManager;
            AiTurnDelayInMilliseconds = 0;
        }

        public BaseCell AiLastOpenedCell => aiFieldManager.LastOpenedCell;
        public BaseCell PlayerLastOpenedCell => playerFieldManager.LastOpenedCell;
        public Fleet AiFleet => playerFieldManager.Field.Fleet;
        public Fleet PlayerFleet => aiFieldManager.Field.Fleet;
        public Cell[,] AiField => playerFieldManager.Field.Cells;
        public Cell[,] PlayerField => aiFieldManager.Field.Cells;
        public Int64 AiTurnDelayInMilliseconds { get; set; }

        public void PlayerTurn(BaseCell cell)
        {
            playerFieldManager.Shoot(cell);
            FieldChanged?.Invoke();
            Delay();
            aiFieldManager.Shoot();
            FieldChanged?.Invoke();
        }

        private void Delay()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds >= AiTurnDelayInMilliseconds)
                {
                    break;
                }
            }
        }

        public bool IsPlayerWin => playerFieldManager.Field.Fleet.IsDestroyed;
        public bool IsAiWin => aiFieldManager.Field.Fleet.IsDestroyed;
    }
}
