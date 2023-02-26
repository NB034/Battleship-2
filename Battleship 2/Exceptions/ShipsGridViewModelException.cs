using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.Exceptions
{
    internal class ShipsGridViewModelException : Exception
    {
        public ShipsGridViewModelException(string message) : base(message) { }
    }
}
