using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackAnimation : ComponentTrackBase
    {
        [XmlAttribute]
        public Guid SlotId;

        [XmlIgnore]
        public static readonly Dictionary<string, Guid> slotIdMapping = new()
        {
            { "0", new Guid("e3ff500b-8db3-4fbd-a180-6aad8c3ba4f4") },
            { "1", new Guid("a28f7c2c-ee3a-4cd1-8ea3-fe89e077d608") },
            { "Tentacles", new Guid("debc7e0d-968d-413f-a252-ee11313fcd04") },
            { "Tail", new Guid("461b6dac-3113-458e-bf82-235c2a1ddad9") },
            { "Hair", new Guid("4a506547-00d2-47e1-be19-36765f199812") },
            { "Wings", new Guid("9cc6a1bd-ba90-453f-97fc-43d436b470ea") },
            { "Private Parts", new Guid("20752dd3-b5a7-4578-ba6d-282211bb94a7") }
        };

        public ComponentTrackAnimation()
        {
            Name = "Animation";
            Type = "ComponentTrackAnimation";
            SlotId = slotIdMapping["0"];

            Tracks.Add(new KeyTrackBoolean { Name = "Hide VFX" });
        }
    }
}
