using Battleship_2.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.Models.Ai
{
    internal interface IAi_FindTargetModule
    {
        Ai_TurnInfo FindTarget();
    }
}
