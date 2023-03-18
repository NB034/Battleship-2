using Battleship_2.Models.Ai;
using Battleship_2.Models.FieldComponents;
using System;
using System.Diagnostics;

namespace Battleship_2.Models
{
    internal class PVA_GameManager
    {
        private readonly SimpleAiFieldManager _aiFieldManager;
        private readonly PlayerFieldManager _playerFieldManager;
        private bool _isPlayerTurn;

        public event Action? AiFieldChanged;
        public event Action? PlayerFieldChanged;
        public event Action? PlayerWin;
        public event Action? AiWin;

        public PVA_GameManager(SimpleAiFieldManager aiFieldManager, PlayerFieldManager playerFieldManager)
        {
            _aiFieldManager = aiFieldManager;
            _playerFieldManager = playerFieldManager;
            AiTurnDelayInMilliseconds = 800;
            _isPlayerTurn = true;
        }

        public Fleet AiFleet => _playerFieldManager.Field.Fleet;
        public Fleet PlayerFleet => _aiFieldManager.Field.Fleet;
        public FieldCell[,] AiField => _playerFieldManager.Field.Cells;
        public FieldCell[,] PlayerField => _aiFieldManager.Field.Cells;
        public int AiTurnDelayInMilliseconds { get; set; }

        public bool IsPlayerTurn
        {
            get { return _isPlayerTurn; }
            private set
            {
                _isPlayerTurn = value;
                AiFieldChanged?.Invoke();
            }
        }

        public void PlayerShoot(Cell cell)
        {
            IsPlayerTurn = false;

            if (PlayerTurn(cell))
            {
                if (IsPlayerWin) PlayerWin?.Invoke();
                IsPlayerTurn = true;
                return;
            }

            Delay(500);
            while (AiTurn())
            {
                if (IsAiWin) AiWin?.Invoke();
                Delay(AiTurnDelayInMilliseconds);
            }

            IsPlayerTurn = true;
        }

        private bool PlayerTurn(Cell cell)
        {
            bool isHit = _playerFieldManager.Shoot(cell);
            AiFieldChanged?.Invoke();
            return isHit;
        }

        private bool AiTurn()
        {
            bool isHit = _aiFieldManager.Shoot();
            PlayerFieldChanged?.Invoke();
            return isHit;
        }

        private void Delay(long milliseconds)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < milliseconds) ;
        }

        public bool IsPlayerWin => _playerFieldManager.Field.Fleet.IsDestroyed;
        public bool IsAiWin => _aiFieldManager.Field.Fleet.IsDestroyed;
    }
}
