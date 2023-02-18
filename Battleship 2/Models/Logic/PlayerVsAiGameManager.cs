using Battleship_2.Models.Components;
using System;
using System.Diagnostics;

namespace Battleship_2.Models.Logic
{
    internal class PlayerVsAiGameManager
    {
        private readonly AiFieldManager aiFieldManager;
        private readonly PlayerFieldManager playerFieldManager;

        public PlayerVsAiGameManager(AiFieldManager aiFieldManager, PlayerFieldManager playerFieldManager)
        {
            this.aiFieldManager = aiFieldManager;
            this.playerFieldManager = playerFieldManager;
            AiTurnDelayInMilliseconds = 1000;
        }

        public Field AiField => aiFieldManager.Field;
        public Field PlayerField => playerFieldManager.Field;
        public Int64 AiTurnDelayInMilliseconds { get; set; }

        public void PlayerTurn(BaseCell cell)
        {
            playerFieldManager.Shoot(cell);
            Delay();
            aiFieldManager.Shoot();
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
    }
}
