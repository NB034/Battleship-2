using Battleship_2.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.Models.Logic
{
    public interface IFieldManager
    {
        Field Field { get; }
        BaseCell LastOpenedCell { get; }
    }
}
