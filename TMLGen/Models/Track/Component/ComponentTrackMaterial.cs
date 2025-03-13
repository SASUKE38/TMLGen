using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Component;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackMaterial : ComponentTrackBase
    {
        [XmlAttribute]
        public Guid MaterialGroupId;
        [XmlAttribute]
        public Guid VisualResourceId;
        [XmlAttribute]
        public Guid CharacterVisualResourceId;
        [XmlAttribute]
        public bool IsOverlay;
        [XmlAttribute]
        public int Slot;
        [XmlAttribute]
        public float OverlayPriority;

        [XmlIgnore]
        public HashSet<string> subTrackTypes;

        public ComponentTrackMaterial()
        {
            Name = "Material";
            Type = "ComponentTrackMaterial";

            MaterialGroupId = Guid.Empty;
            VisualResourceId = Guid.Empty;
            CharacterVisualResourceId = Guid.Empty;
            IsOverlay = false;
            Slot = -1;
            OverlayPriority = 0f;
            subTrackTypes = [];

            Tracks.Add(new KeyTrackBoolean { Name = ComponentMaterial.visibilityName });
        }
    }
}
