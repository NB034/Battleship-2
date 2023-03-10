using Battleship_2.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.Models.Ai
{
    internal abstract class Ai_Base
    {
        public IAi_FindTargetModule FindTargetModule { get; }
        public IAi_DestroyShipModule DestroyShipModule { get; }

        private bool _aiHasFoundShip;
        private BaseCell _lastOpenedCell;

        public Ai_Base(IAi_FindTargetModule findTargetModule, IAi_DestroyShipModule destroyShipModule)
        {
            FindTargetModule = findTargetModule;
            DestroyShipModule = destroyShipModule;
            _aiHasFoundShip = false;
            _lastOpenedCell = BaseCell.NotValid;
        }

        public bool Shoot()
        {
            Ai_TurnInfo info;
            if(_aiHasFoundShip)
            {
                info = DestroyShipModule.DestroyShip(_lastOpenedCell);
                _aiHasFoundShip = !info.WasShipDestroyed;
                _lastOpenedCell = info.LastOpenedCell;
            }
            else
            {
                info = FindTargetModule.FindTarget();
                _aiHasFoundShip = info.WasShotSuccessfull && !info.WasShipDestroyed;
                _lastOpenedCell = info.LastOpenedCell;

            }
            return info.WasShotSuccessfull;
        }
    }
}
