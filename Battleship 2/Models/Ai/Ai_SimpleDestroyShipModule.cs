using Battleship_2.Models.Components;
using Battleship_2.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using Battleship_2.Accessories;
using Battleship_2.Exceptions;

namespace Battleship_2.Models.Ai
{
    internal class Ai_SimpleDestroyShipModule : IAi_DestroyShipModule
    {
        private Random _random;
        private Field _field;
        private List<BaseCell> _openedShipCells;
        private BaseCell _lastOpenedCell;
        private OrientationsEnum _shipOrientation;
        private (DirectionsEnum direction, bool isChecked)[] _checkedDirections;

        public Field Field { get => _field; }

        public Ai_SimpleDestroyShipModule(Field field)
        {
            _random = new Random();
            _field = field;
            _openedShipCells = new List<BaseCell>(LogicAccessories.NumberOfShipsDecks.Max());
            _lastOpenedCell = BaseCell.NotValid;
            _shipOrientation = OrientationsEnum.Unknown;
            _checkedDirections = new[]
            {
                (DirectionsEnum.Left, false),
                (DirectionsEnum.Right, false),
                (DirectionsEnum.Up, false),
                (DirectionsEnum.Down, false)
            };
        }

        public Ai_TurnInfo DestroyShip(BaseCell lastOpenedCell)
        {
            if (_shipOrientation == OrientationsEnum.Unknown)
            {
                _openedShipCells.Add(lastOpenedCell);
                return FindSecondCell();
            }
            else
            {
                return FindLeftCells();
            }
        }

        private Ai_TurnInfo FindSecondCell()
        {
            while (true)
            {
                var availableDirections = _checkedDirections.Where(d => !d.isChecked);
                var count = availableDirections.Count();

                if (count == 0) throw new AiException("AI checked every direction");

                var direction = availableDirections.ElementAt(_random.Next(count)).direction;
                var nextCell = GetNextCell(direction, _openedShipCells.First());

                if (LogicAccessories.FieldBoundsCheck(nextCell) && !Field[nextCell.I, nextCell.J].IsOpen)
                {
                    var info = OpenCellAndMakeReport(nextCell);
                    _lastOpenedCell = nextCell;

                    if (!info.WasShotSuccessfull)
                    {
                        MarkCheckedDirection(direction);
                    }
                    else if (info.WasShipDestroyed)
                    {
                        ResetMemory();
                    }
                    else
                    {
                        _openedShipCells.Add(nextCell);
                        _shipOrientation = direction.DirectionToOrientation();
                    }

                    return info;
                }
                else
                {
                    MarkCheckedDirection(direction);
                }
            }
        }

        private Ai_TurnInfo FindLeftCells()
        {
            if (_shipOrientation == OrientationsEnum.Unknown) throw new AiException("Orientation of the ship was not found out");

            BaseCell firstPossibleCell;
            BaseCell secondPossibleCell;

            if (_shipOrientation == OrientationsEnum.Horizontal)
            {
                _openedShipCells.Sort((c1, c2) => c1.J.CompareTo(c2.J));
                firstPossibleCell = GetNextCell(DirectionsEnum.Left, _openedShipCells.First());
                secondPossibleCell = GetNextCell(DirectionsEnum.Right, _openedShipCells.Last());
            }
            else
            {
                _openedShipCells.Sort((c1, c2) => c1.I.CompareTo(c2.I));
                firstPossibleCell = GetNextCell(DirectionsEnum.Up, _openedShipCells.First());
                secondPossibleCell = GetNextCell(DirectionsEnum.Down, _openedShipCells.Last());
            }

            var isFirstCellAlowed = LogicAccessories.FieldBoundsCheck(firstPossibleCell)
                && !Field[firstPossibleCell.I, firstPossibleCell.J].IsOpen;
            var isSecondCellAlowed = LogicAccessories.FieldBoundsCheck(secondPossibleCell)
                && !Field[secondPossibleCell.I, secondPossibleCell.J].IsOpen;

            var info = new Ai_TurnInfo();

            if (isFirstCellAlowed && isSecondCellAlowed)
            {
                _lastOpenedCell = Convert.ToBoolean(_random.Next(2)) ? firstPossibleCell : secondPossibleCell;
            }
            else if (isFirstCellAlowed)
            {
                _lastOpenedCell = firstPossibleCell;
            }
            else if (isSecondCellAlowed)
            {
                _lastOpenedCell = secondPossibleCell;
            }
            else
            {
                throw new AiException("AI checked every direction");
            }

            info = OpenCellAndMakeReport(_lastOpenedCell);

            if (info.WasShipDestroyed)
            {
                ResetMemory();
            }
            else if (info.WasShotSuccessfull)
            {
                _openedShipCells.Add(info.LastOpenedCell);
            }

            return info;
        }

        private Ai_TurnInfo OpenCellAndMakeReport(BaseCell cell)
        {
            Field.OpenCell(cell);
            var fieldCell = Field[cell.I, cell.J];
            var info = new Ai_TurnInfo();

            if (fieldCell.CellType != CellTypesEnum.ShipDeck)
            {
                info.WasShotSuccessfull = false;
                info.WasShipDestroyed = false;
                info.LastOpenedCell = _lastOpenedCell;
            }
            else if (Field.IsShipDestroyed(fieldCell.ShipsGuids.First()))
            {
                info.WasShotSuccessfull = true;
                info.WasShipDestroyed = true;
                info.LastOpenedCell = cell;

                LogicAccessories.OpenCellsAroundDestroyedShip(ref _field, fieldCell.ShipsGuids.First());
            }
            else
            {
                info.WasShotSuccessfull = true;
                info.WasShipDestroyed = false;
                info.LastOpenedCell = cell;
            }

            return info;
        }

        private void MarkCheckedDirection(DirectionsEnum direction)
        {
            for (int i = 0; i < _checkedDirections.Length; i++)
            {
                if (_checkedDirections[i].direction == direction)
                {
                    _checkedDirections[i].isChecked = true;
                }
            }
        }

        private BaseCell GetNextCell(DirectionsEnum direction, BaseCell cell) => direction switch
        {
            DirectionsEnum.Left => new BaseCell(cell.J - 1, cell.I),
            DirectionsEnum.Right => new BaseCell(cell.J + 1, cell.I),
            DirectionsEnum.Up => new BaseCell(cell.J, cell.I - 1),
            DirectionsEnum.Down => new BaseCell(cell.J, cell.I + 1),
            _ => cell
        };

        public void ResetMemory()
        {
            _shipOrientation = OrientationsEnum.Unknown;
            _openedShipCells.Clear();
            _lastOpenedCell = BaseCell.NotValid;
            for (int i = 0; i < _checkedDirections.Length; i++)
            {
                _checkedDirections[i].isChecked = false;
            }
        }
    }
}
