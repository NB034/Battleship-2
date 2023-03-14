using Battleship_2.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.Accessories
{
    public static class EnumExtensions
    {
        public static OrientationsEnum DirectionToOrientation(this DirectionsEnum directionEnum)
        {
            if(directionEnum == DirectionsEnum.Unknown) return OrientationsEnum.Unknown;

            return directionEnum == DirectionsEnum.Left || directionEnum == DirectionsEnum.Right 
                ? OrientationsEnum.Horizontal 
                : OrientationsEnum.Vertical;
        }
    }
}
