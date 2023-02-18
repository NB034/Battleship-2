using Battleship_2.Exceptions;
using Battleship_2.Models.Components;
using System;
using System.Linq;

namespace Battleship_2.Models.Logic
{
    internal class AiCore
    {
        private bool _hasFoundShip;
        private bool[] _isDirectionAvailable;
        private int? _nextShotDirection;
        private BaseCell? _firstFoundedCell;
        private BaseCell? _lastFoundedCell;

        public bool HasFoundShip => _hasFoundShip;
        public OrientationsEnum? NextShotDirection => _nextShotDirection is null ? null : (OrientationsEnum)_nextShotDirection;
        public BaseCell? FirstFoundedCell => _firstFoundedCell;
        public BaseCell? LastFoundedCell => _lastFoundedCell;

        public AiCore()
        {
            _isDirectionAvailable = new bool[Enum.GetValues(typeof(OrientationsEnum)).Length];
            ResetMemory();
        }

        public void ResetMemory()
        {
            _hasFoundShip = false;
            _nextShotDirection = null;
            _firstFoundedCell = null;
            _lastFoundedCell = null;
            for (int i = 0; i < _isDirectionAvailable.Length; i++)
                _isDirectionAvailable[i] = true;
        }

        public void ShipWasDamaged(BaseCell coordinates)
        {
            if (_hasFoundShip)
            {
                _lastFoundedCell = coordinates;
                return;
            }

            _hasFoundShip = true;
            _nextShotDirection = null;
            _firstFoundedCell = coordinates;
            _lastFoundedCell = coordinates;
        }

        public void ShipWasDestroyed()
        {
            ResetMemory();
        }

#pragma warning disable CS8629 // Nullable value type may be null. In this method use of the null assigned fields excluded.
        public void Miss()
        {
            if (_hasFoundShip)
            {
                if (_firstFoundedCell == _lastFoundedCell)
                {
                    _isDirectionAvailable[_nextShotDirection.Value] = false;
                    if (!_isDirectionAvailable.Any(b => b)) throw new AiException("AI checked every direction");

                    while (true)
                    {
                        int direction = LogicAccessories.GetRandomDirectionAsNumber();
                        if (_isDirectionAvailable[(direction)])
                        {
                            _nextShotDirection = direction;
                            break;
                        }
                    }
                    return;
                }

                _isDirectionAvailable[_nextShotDirection.Value] = false;
                if (_isDirectionAvailable[LogicAccessories.GetOppositeDirectionAsNumber(_nextShotDirection.Value)])
                    throw new AiException("AI checked every direction");

                _nextShotDirection = LogicAccessories.GetOppositeDirectionAsNumber(_nextShotDirection.Value);
            }
        }
    }
}
#pragma warning restore CS8629 // Nullable value type may be null.
