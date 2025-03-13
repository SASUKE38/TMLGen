using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLGen.Models.Track;

namespace TMLGen.Models
{
    public class TrackContainer
    {
        public List<TrackBase> Tracks;

        public TrackContainer()
        {
            Tracks = new List<TrackBase>();
        }

    }
}
