using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_2.Exceptions
{
    internal class AiException : Exception
    {
        public AiException(string message) : base(message) { }
    }
}
