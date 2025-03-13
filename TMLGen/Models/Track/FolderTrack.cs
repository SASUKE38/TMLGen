using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Track
{
    public class FolderTrack : TrackBase
    {
        public FolderTrack()
        {
            this.Type = "FolderTrack";
        }
    }
}
