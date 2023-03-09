using Battleship_2.Models.Components;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship_2.Models.Logic
{
    internal class PVA_GameManager
    {
        private readonly AiFieldManager aiFieldManager;
        private readonly PlayerFieldManager playerFieldManager;
        private bool isPlayerTurn;

        public event Action? AiFieldChanged;
        public event Action? PlayerFieldChanged;
        public event Action? PlayerWin;
        public event Action? AiWin;

        public PVA_GameManager(AiFieldManager aiFieldManager, PlayerFieldManager playerFieldManager)
        {
            this.aiFieldManager = aiFieldManager;
            this.playerFieldManager = playerFieldManager;
            AiTurnDelayInMilliseconds = 1000;
            isPlayerTurn = true;
        }

        public Fleet AiFleet => playerFieldManager.Field.Fleet;
        public Fleet PlayerFleet => aiFieldManager.Field.Fleet;
        public Cell[,] AiField => playerFieldManager.Field.Cells;
        public Cell[,] PlayerField => aiFieldManager.Field.Cells;
        public Int32 AiTurnDelayInMilliseconds { get; set; }

        public bool IsPlayerTurn
        {
            get { return isPlayerTurn; }
            private set
            {
                isPlayerTurn = value;
                AiFieldChanged?.Invoke();
            }
        }

        public void PlayerShoot(BaseCell cell)
        {
            IsPlayerTurn = false;

            if (PlayerTurn(cell))
            {
                if (IsPlayerWin) PlayerWin?.Invoke();
                IsPlayerTurn = true;
                return;
            }

            Delay();
            while (AiShoot())
            {
                if (IsAiWin) AiWin?.Invoke();
                Delay();
            }

            IsPlayerTurn = true;
        }

        private bool PlayerTurn(BaseCell cell)
        {
            bool isHit = playerFieldManager.Shoot(cell);
            AiFieldChanged?.Invoke();
            return isHit;
        }

        private bool AiShoot()
        {
            bool isHit = aiFieldManager.Shoot();
            PlayerFieldChanged?.Invoke();
            return isHit;
        }

        private void Delay()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < AiTurnDelayInMilliseconds) ;
        }

        public bool IsPlayerWin => playerFieldManager.Field.Fleet.IsDestroyed;
        public bool IsAiWin => aiFieldManager.Field.Fleet.IsDestroyed;
    }
}
