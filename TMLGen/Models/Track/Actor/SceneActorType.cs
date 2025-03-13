using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLGen.Models.Track.Actor
{
    public enum SceneActorType
    {
        None = 0,
        Scene = 1,
        Initiator = 2,
        Peanut = 4,
        Additional = 8,
        MAX = 9,
        Speakers = 14,
        All = 15
    }
}
