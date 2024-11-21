using R8LocoCtrl.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R8LocoCtrl.Tools
{
    public class DTMFCommandParameter
    {
        public DTMFToneValues Tone { get; set; }
        public bool IsButtonPressed { get; set; }
    }
}
