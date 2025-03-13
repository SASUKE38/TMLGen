using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Sequences;

namespace TMLGen.Models.Global
{
    public class PlaylistContainer
    {
        [XmlElement]
        public List<Playlist> Playlist;

        public PlaylistContainer()
        {
            Playlist = new List<Playlist>
            {
                new Playlist()
            };
        }
    }

    public class Playlist
    {
        [XmlAttribute]
        public string PlaylistType;
        public List<Sequence> Sequences;

        public Playlist()
        {
            PlaylistType = "Dynamic";
            Sequences = new List<Sequence>();
        }
    }
}
