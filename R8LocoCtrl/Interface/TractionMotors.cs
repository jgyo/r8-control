using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R8LocoCtrl.Interface
{
    [Flags]
    public enum TractionMotors
    {
        tm1 = 0b000001,
        tm2 = 0b000010,
        tm3 = 0b000100,
        tm4 = 0b001000,
        tm5 = 0b010000,
        tm6 = 0b100000,
        flag = 0b10000000
    }
}
