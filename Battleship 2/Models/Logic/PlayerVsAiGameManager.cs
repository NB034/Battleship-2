using Battleship_2.Models.Components;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship_2.Models.Logic
{
    internal class PlayerVsAiGameManager
    {
        private readonly AiFieldManager aiFieldManager;
        private readonly PlayerFieldManager playerFieldManager;
        private bool isTurnOver;

        public event Action? FieldsChanged;
        public event Action? PlayerWin;
        public event Action? AiWin;
        public event Action? IsTurnOverChanged;


        public PlayerVsAiGameManager(AiFieldManager aiFieldManager, PlayerFieldManager playerFieldManager)
        {
            this.aiFieldManager = aiFieldManager;
            this.playerFieldManager = playerFieldManager;
            AiTurnDelayInMilliseconds = 800;
            isTurnOver = true;
        }

        public Fleet AiFleet => playerFieldManager.Field.Fleet;
        public Fleet PlayerFleet => aiFieldManager.Field.Fleet;
        public Cell[,] AiField => playerFieldManager.Field.Cells;
        public Cell[,] PlayerField => aiFieldManager.Field.Cells;
        public Int64 AiTurnDelayInMilliseconds { get; set; }
        public bool IsTurnOver
        {
            get { return isTurnOver; }
            private set
            {
                isTurnOver = value;
                Task.Run(() => IsTurnOverChanged?.Invoke());
            }
        }

        public void PlayerTurn(BaseCell cell)
        {
            IsTurnOver = false;

            if (playerFieldManager.Shoot(cell))
            {
                FieldsChanged?.Invoke();
                if (IsPlayerWin)
                {
                    PlayerWin?.Invoke();
                }
                IsTurnOver = true;
                return;
            }
            FieldsChanged?.Invoke();

            Delay();
            while (aiFieldManager.Shoot())
            {
                FieldsChanged?.Invoke();
                if (IsAiWin)
                {
                    AiWin?.Invoke();
                }
                Delay();
            }
            FieldsChanged?.Invoke();

            IsTurnOver = true;
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
