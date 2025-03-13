using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLGen.Models.Global
{
    [Flags]
    public enum PlayerMimicTypes
    {
        Emotion = 1,
        Attitude = 2,
        LookAt = 4
    }
}
